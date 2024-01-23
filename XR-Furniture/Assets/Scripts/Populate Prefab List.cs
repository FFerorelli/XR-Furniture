using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.IO;
using TMPro;
using System.Collections;
using UnityEngine.Networking;
using System;

public class PopulatePrefabList : MonoBehaviour
{
    public GameObject buttonPrefab;
    public Transform buttonsParent; // Reference to the parent object where buttons will be placed
    public string folderPath; // Path to the folder containing the prefabs
    public string relativePath; // Path to the folder containing the prefabs
    public string assetBundleName;
    [SerializeField] private GameObject[] _myPrefabList;

    void Start()
    {
        // StartCoroutine(CreateGrid());
        CreateGrid();
    }

    void CreateGrid()
    {
        foreach (var prefab in _myPrefabList)
        {
            string prefabName = prefab.name;
            //GameObject prefab = Resources.Load<GameObject>(relativePath + prefabName);
            Debug.Log(prefabName + "---------------");

            GameObject button = Instantiate(buttonPrefab, buttonsParent);

            button.GetComponentInChildren<TextMeshProUGUI>().text = prefabName;

            button.GetComponent<Button>().onClick.AddListener(() => SetCurrentPrefab(prefab));
        }
    }
        //IEnumerator CreateGrid()
        //{
        //    string pathToAssetBundle = Path.Combine(Application.dataPath, "StreamingAssets", assetBundleName.ToLower());
        //    AssetBundleCreateRequest request = AssetBundle.LoadFromFileAsync(pathToAssetBundle);

        //    yield return request;

        //    AssetBundle myLoadedAssetBundle = request.assetBundle;

        //    if (myLoadedAssetBundle == null)
        //    {
        //        Debug.Log("Failed to load AssetBundle!");
        //        yield break;
        //    }

        //    var prefabNames = myLoadedAssetBundle.GetAllAssetNames();

        //    foreach (var path in prefabNames)
        //    {
        //        string prefabName = Path.GetFileNameWithoutExtension(path);
        //        AssetBundleRequest prefabRequest = myLoadedAssetBundle.LoadAssetAsync<GameObject>(prefabName);

        //        yield return prefabRequest;

        //        GameObject prefab = prefabRequest.asset as GameObject;
        //        Debug.Log(prefabName + "---------------");

        //        GameObject button = Instantiate(buttonPrefab, buttonsParent);

        //        button.GetComponentInChildren<TextMeshProUGUI>().text = prefabName;

        //        button.GetComponent<Button>().onClick.AddListener(() => SetCurrentPrefab(prefab));
        //    }
        //}
        //string[] prefabPaths = Directory.GetFiles(folderPath, "*.prefab");

        //foreach (var path in prefabPaths)
        //{

        //    string prefabName = Path.GetFileNameWithoutExtension(path);
        //    GameObject prefab = Resources.Load<GameObject>(relativePath + prefabName);
        //    Debug.Log(prefabName + "---------------");


        //        // Create a button from the prefab
        //    GameObject button = Instantiate(buttonPrefab, buttonsParent);

        //    button.GetComponentInChildren<TextMeshProUGUI>().text = prefabName; // Set button text or image here

        //  button.GetComponent<Button>().onClick.AddListener(() => SetCurrentPrefab(prefab));

        void SetCurrentPrefab(GameObject prefab)
     {
        Debug.Log(prefab);
        FurniturePlacement.Instance.SetNewFurniture(prefab);
     }
}
   



