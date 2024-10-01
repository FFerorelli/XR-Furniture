// PopulatePrefabList.cs
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Collections;
using TMPro;

public class PopulateObjectList : MonoBehaviour
{
    public GameObject buttonPrefab;
    public Transform buttonsParent;
    [SerializeField] private GameObject[] _myPrefabList;

    private int currentPrefabIndex = 0;
    private Coroutine instantiationCoroutine;

    void Start()
    {
        StartCoroutine(CreateGrid());
        // No need to start coroutine here
    }

    //public void StartGridCreation()
    //{
    //    instantiationCoroutine = StartCoroutine(CreateGrid());
    //}

    //public void StopGridCreation()
    //{
    //    if (instantiationCoroutine != null)
    //    {
    //        StopCoroutine(instantiationCoroutine);
    //    }
    //}

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
                // Create the sprite from the texture
                Sprite buttonImage = Sprite.Create(prefabThumbnail,
                                                   new Rect(0, 0, prefabThumbnail.width, prefabThumbnail.height),
                                                   new Vector2(0.5f, 0.5f),
                                                   100);

                // Instantiate the button prefab
                GameObject button = Instantiate(buttonPrefab, buttonsParent);

                if (button == null)
                {
                    Debug.Log("Failed to instantiate button for: " + prefabName);
                    continue;
                }

                // Find the ThumbnailImage child
                Image buttonImageComponent = button.transform.Find("ThumbnailImage")?.GetComponent<Image>();

                if (buttonImageComponent == null)
                {
                    Debug.Log("Failed to find ThumbnailImage component for: " + prefabName);
                    continue;
                }

                // Assign the sprite to the ThumbnailImage
                buttonImageComponent.sprite = buttonImage;

                // Ensure preserve aspect is set to true to maintain image proportions
                buttonImageComponent.preserveAspect = true;

                // Add the onClick listener
                Button buttonComponent = button.GetComponent<Button>();
                if (buttonComponent != null)
                {
                    buttonComponent.onClick.AddListener(() => FurniturePlacement.Instance.SetNewFurniture(prefab));
                }
                else
                {
                    Debug.Log("Button component missing on: " + prefabName);
                }
            }

            currentPrefabIndex = i + 1;
            yield return null;
        }
    }


}
