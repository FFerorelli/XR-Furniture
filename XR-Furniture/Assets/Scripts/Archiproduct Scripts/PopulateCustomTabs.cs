using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting;
using System;

public class PopulateCustomTabs : MonoBehaviour
{

    public GameObject customizeTab;
    public GameObject materialButtonPrefab;
    public Transform tabParent;
    private CustomElement[] _customPartsList;
    private Furniture _parentObject;
    [SerializeField]
    private TextMeshProUGUI _parentName;

    // Start is called before the first frame update
    void Start()
    {
        _parentObject = transform.root.gameObject.GetComponent<Furniture>();
        Debug.Log("Furniture script found in root parent: " + _parentObject.gameObject.name);

        _parentName.text = _parentObject.gameObject.name.ToUpper();

        _customPartsList = _parentObject.customizableElements;

        StartCoroutine(createTabs());
        //createTabs();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    IEnumerator createTabs()
    {
        Debug.Log("Starting createTabs coroutine");

        for (int i = 0; i < _customPartsList.Length; i++)
        {
            var currentPart = _customPartsList[i];
            Debug.Log($"Processing part {i}: {currentPart.name}");

            GameObject tab = Instantiate(customizeTab, tabParent);
            Debug.Log($"Instantiated tab for part {currentPart.name}");

            // Use the instantiated tab to find the Part Name TextMeshProUGUI component
            TextMeshProUGUI partName = tab.transform.Find("Part Name").GetComponent<TextMeshProUGUI>();
            if (partName == null)
            {
                Debug.LogError("Part Name TextMeshProUGUI not found on the instantiated tab.");
                yield break; // Exit the coroutine if we cannot find the part name
            }
            partName.text = currentPart.name;
            Debug.Log($"Set part name text to {currentPart.name}");

            // Use the instantiated tab to find the Materials list
            GameObject materialsContent = tab.transform.Find("Materials list").gameObject;
            if (materialsContent == null)
            {
                Debug.LogError("Materials list GameObject not found on the instantiated tab.");
                yield break; // Exit the coroutine if we cannot find the materials list
            }
            Debug.Log("Found materials list");

            for (int j = 0; j < currentPart.availableMaterials.Length; j++)
            {
                GameObject materialButton = Instantiate(materialButtonPrefab, materialsContent.transform);
                Debug.Log($"Instantiated material button {j} for part {currentPart.name}");

                // Capture the current value of j
                int capturedIndex = j;
                materialButton.GetComponent<Button>().onClick.AddListener(() =>
                {
                    Debug.Log($"Button {capturedIndex} for part {currentPart.name} clicked");
                    ChangeMaterial(currentPart, currentPart.availableMaterials[capturedIndex]);
                });
                Debug.Log($"Added listener to button {j} for material {currentPart.availableMaterials[capturedIndex].name}");

                // Set the button image to the texture of the material
                Image textureHolderImage = materialButton.transform.Find("TextureHolder").GetComponent<Image>();
                if (textureHolderImage != null && currentPart.availableMaterials[capturedIndex].mainTexture != null)
                {
                    textureHolderImage.sprite = Sprite.Create((Texture2D)currentPart.availableMaterials[capturedIndex].mainTexture,
                                                            new Rect(0, 0, currentPart.availableMaterials[capturedIndex].mainTexture.width, currentPart.availableMaterials[capturedIndex].mainTexture.height),
                                                            new Vector2(0.5f, 0.5f));
                    Debug.Log($"Set image for button {j} to material texture {currentPart.availableMaterials[capturedIndex].name}");
                }
                else
                {
                    Debug.LogWarning($"TextureHolder image or material texture not found for button {j} of part {currentPart.name}");
                }

                // Additional setup for materialButton if needed
                // yield return null; // Uncomment if needed for other processing
            }

            // Example of adding a listener to a button on the instantiated tab, if such button exists
            // Button button = tab.GetComponentInChildren<Button>();
            // if (button != null)
            // {
            //     button.onClick.AddListener(() => FurniturePlacement.Instance.SetNewFurniture(prefab));
            // }

            yield return null; // Continue to the next iteration in the next frame
        }

        Debug.Log("Finished createTabs coroutine");
    }

    private void ChangeMaterial(CustomElement part, Material material)
    {
        Debug.Log($"Changing material of part {part.name} to {material.name}");
        part.GetComponent<MeshRenderer>().material = material;
    }
    public Texture2D RenderMaterialToTexture(Material material, int width, int height)
    {
        RenderTexture renderTexture = new RenderTexture(width, height, 24);
        Camera camera = new GameObject("TempCamera").AddComponent<Camera>();
        camera.backgroundColor = Color.clear;
        camera.clearFlags = CameraClearFlags.Color;
        camera.targetTexture = renderTexture;

        GameObject tempObject = GameObject.CreatePrimitive(PrimitiveType.Quad);
        tempObject.GetComponent<MeshRenderer>().material = material;
        tempObject.transform.position = Vector3.zero;

        camera.transform.position = tempObject.transform.position - tempObject.transform.forward * 2;
        camera.transform.LookAt(tempObject.transform);

        camera.Render();

        RenderTexture.active = renderTexture;
        Texture2D texture = new Texture2D(width, height, TextureFormat.RGB24, false);
        texture.ReadPixels(new Rect(0, 0, width, height), 0, 0);
        texture.Apply();

        RenderTexture.active = null;
        camera.targetTexture = null;

        UnityEngine.Object.Destroy(camera.gameObject);
        UnityEngine.Object.Destroy(tempObject);

        return texture;
    }

}

