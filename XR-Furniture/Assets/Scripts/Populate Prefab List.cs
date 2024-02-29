using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.IO;
using TMPro;
using System.Collections;
using UnityEngine.Networking;
using System;
using UnityEditor;


public class PopulatePrefabList : MonoBehaviour
{
    public GameObject buttonPrefab;
    public Transform buttonsParent; // Reference to the parent object where buttons will be placed
    //public string folderPath; // Path to the folder containing the prefabs
    //public string relativePath; // Path to the folder containing the prefabs
    //public string assetBundleName;
    [SerializeField] private GameObject[] _myPrefabList;
    public Dictionary<GameObject, Sprite> prefabToThumbnail;
    public string screenshotDirectory = "Assets/Resources/Thumbnails";
    


    void Start()
    {
        StartCoroutine(CreateGrid());
        //CreateGrid();
    }

    IEnumerator CreateGrid()
    {
        foreach (var prefab in _myPrefabList)
        {
            string prefabName = prefab.name;

           // Debug.Log("Loading: Thumbnails/" + prefabName);
            ResourceRequest resourceRequest = Resources.LoadAsync<Texture2D>("Thumbnails/" + prefabName);

            yield return resourceRequest;

            if (resourceRequest.asset == null)
            {
                Debug.Log("Failed to load: Thumbnails/" + prefabName);
                continue;
            }

            if (resourceRequest.asset is Texture2D prefabThumbnail)
            {
                Sprite buttonImage = Sprite.Create(prefabThumbnail, new Rect(0, 0, prefabThumbnail.width, prefabThumbnail.height), new Vector2(0.5f, 0.5f), 100);

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

            yield return null; // This will ensure that the loop will continue on the next frame
        }
    }

}
   



