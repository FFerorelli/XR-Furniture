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
    private CustomizableObject _parentObject;
    [SerializeField]
    private TextMeshProUGUI _parentName;

    // Start is called before the first frame update
    void Start()
    {
        _parentObject = transform.root.gameObject.GetComponent<CustomizableObject>();
        if (_parentObject == null)
        {
            Debug.LogError("CustomizableObject script not found in root parent.");
            return;
        }

        string parentName = _parentObject.gameObject.name.Replace("(Clone)", "").Trim();
        Debug.Log("CustomizableObject script found in root parent: " + parentName);

        _parentName.text = parentName.ToUpper();

        _customPartsList = _parentObject.customizableElements;

        StartCoroutine(createTabs());
        //createTabs();
    }


    // Update is called once per frame
    void Update()
    {
        
    }
    public IEnumerator createTabs()
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

                // Set the button image or color based on the material
                Image textureHolderImage = materialButton.transform.Find("TextureHolder").GetComponent<Image>();
                if (textureHolderImage != null)
                {
                    if (currentPart.availableMaterials[capturedIndex].mainTexture != null)
                    {
                        // Use the main texture
                        textureHolderImage.sprite = Sprite.Create((Texture2D)currentPart.availableMaterials[capturedIndex].mainTexture,
                                                                new Rect(0, 0, currentPart.availableMaterials[capturedIndex].mainTexture.width, currentPart.availableMaterials[capturedIndex].mainTexture.height),
                                                                new Vector2(0.5f, 0.5f));
                        Debug.Log($"Set image for button {j} to material texture {currentPart.availableMaterials[capturedIndex].name}");
                    }
                    else
                    {
                        // Use the material color
                        textureHolderImage.color = currentPart.availableMaterials[capturedIndex].color;
                        Debug.Log($"Set color for button {j} to material color {currentPart.availableMaterials[capturedIndex].color}");
                    }
                }
                else
                {
                    Debug.LogWarning($"TextureHolder image component not found for button {j} of part {currentPart.name}");
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
        Debug.Log($"Attempting to change material of part {part.name} to {material.name}");

        MeshRenderer meshRenderer = part.GetComponent<MeshRenderer>();
        if (meshRenderer != null)
        {
            Debug.Log($"Changing material of part {part.name}");
            meshRenderer.material = material;
        }
        else
        {
            Debug.Log($"Part {part.name} does not have a MeshRenderer. Changing material of its children.");
            MeshRenderer[] childRenderers = part.GetComponentsInChildren<MeshRenderer>();
            foreach (MeshRenderer childRenderer in childRenderers)
            {
                Debug.Log($"Changing material of child {childRenderer.gameObject.name}");
                childRenderer.material = material;
            }
        }
    }



}

