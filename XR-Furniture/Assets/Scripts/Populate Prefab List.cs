using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.IO;
using TMPro;
using System.Collections;
using UnityEngine.Networking;
using System;
using UnityEditor;
using static System.Net.Mime.MediaTypeNames;

public class PopulatePrefabList : MonoBehaviour
{
    public GameObject buttonPrefab;
    public Transform buttonsParent; // Reference to the parent object where buttons will be placed
    public string folderPath; // Path to the folder containing the prefabs
    public string relativePath; // Path to the folder containing the prefabs
    public string assetBundleName;
    [SerializeField] private GameObject[] _myPrefabList;
    public Dictionary<GameObject, Sprite> prefabToThumbnail;
    public string screenshotDirectory = "Assets/Resources/Thumbnails";
    


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
            // string imagePath = screenshotDirectory + "/Thumbnails" + prefabName + ".png";

            //Texture2D prefabThumbnail = AssetPreview.GetAssetPreview(prefab);

            //byte[] imageBytes = File.ReadAllBytes(imagePath);
            //Texture2D prefabThumbnail = new Texture2D(2, 2);
            //prefabThumbnail.LoadImage(imageBytes);

            Debug.Log("Thumbnails/" + prefabName);
            Texture2D prefabThumbnail = Resources.Load<Texture2D>("Thumbnails/Thumbnails" + prefabName);

            Sprite buttonImage = Sprite.Create(prefabThumbnail, new Rect(0, 0, prefabThumbnail.width, prefabThumbnail.height), new Vector2(0.5f, 0.5f), 100);



            GameObject button = Instantiate(buttonPrefab, buttonsParent);


            button.GetComponentInChildren<UnityEngine.UI.Image>().sprite = buttonImage;

           // button.GetComponentInChildren<TextMeshProUGUI>().text = prefabName;

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
    //IEnumerator LoadThumbnail(GameObject prefab)
    //{
    //    UnityWebRequest www = UnityWebRequestTexture.GetTexture(prefab.name + ".png");
    //    yield return www.SendWebRequest();
    //    Texture2D texture = ((DownloadHandlerTexture)www.downloadHandler).texture;
    //    Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
    //    prefabToThumbnail[prefab] = sprite;
    //}
}
   



