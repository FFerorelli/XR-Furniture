// PopulatePrefabList.cs
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Collections;
using TMPro;

public class PopulatePrefabList : MonoBehaviour
{
    public GameObject buttonPrefab;
    public Transform buttonsParent;
    [SerializeField] private GameObject[] _myPrefabList;

    private int currentPrefabIndex = 0;
    private Coroutine instantiationCoroutine;

    void Start()
    {
        // No need to start coroutine here
    }

    public void StartGridCreation()
    {
        instantiationCoroutine = StartCoroutine(CreateGrid());
    }

    public void StopGridCreation()
    {
        if (instantiationCoroutine != null)
        {
            StopCoroutine(instantiationCoroutine);
        }
    }

    IEnumerator CreateGrid()
    {
        for (int i = currentPrefabIndex; i < _myPrefabList.Length; i++)
        {
            var prefab = _myPrefabList[i];
            string prefabName = prefab.name;

            ResourceRequest resourceRequest = Resources.LoadAsync<Texture2D>("Thumbnails/" + prefabName);

            yield return resourceRequest;

            if (resourceRequest.asset == null)
            {
                Debug.Log("Failed to load: Thumbnails/" + prefabName);
                continue;
            }

            if (resourceRequest.asset is Texture2D prefabThumbnail)
            {
                // Determine the size to crop to square based on the shorter side
                float squareSize = Mathf.Min(prefabThumbnail.width, prefabThumbnail.height);
                float xOffset = (prefabThumbnail.width - squareSize) / 2.0f; // Center the crop on the longer dimension
                float yOffset = (prefabThumbnail.height - squareSize) / 2.0f;

                Sprite buttonImage = Sprite.Create(prefabThumbnail, new Rect(xOffset, yOffset, squareSize, squareSize), new Vector2(0.5f, 0.5f), 100);

                GameObject button = Instantiate(buttonPrefab, buttonsParent);

                if (button == null)
                {
                    Debug.Log("Failed to instantiate button for: " + prefabName);
                    continue;
                }

                Image buttonImageComponent = button.GetComponentInChildren<Image>();

                if (buttonImageComponent == null)
                {
                    Debug.Log("Failed to get Image component for: " + prefabName);
                    continue;
                }

                buttonImageComponent.sprite = buttonImage;

                button.GetComponent<Button>().onClick.AddListener(() => FurniturePlacement.Instance.SetNewFurniture(prefab));
            }

            currentPrefabIndex = i + 1;

            yield return null;
        }
    }

}
