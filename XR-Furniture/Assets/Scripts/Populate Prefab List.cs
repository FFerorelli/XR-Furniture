using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.IO;
using TMPro;


public class PopulatePrefabList : MonoBehaviour
{
    public GameObject buttonPrefab;
    public Transform buttonsParent; // Reference to the parent object where buttons will be placed
    public string folderPath; // Path to the folder containing the prefabs
    public string relativePath; // Path to the folder containing the prefabs

    void Start()
    {
        CreateGrid();
    }

    void CreateGrid()
    {
        string[] prefabPaths = Directory.GetFiles(folderPath, "*.prefab");
        
        foreach (var path in prefabPaths)
        {

            string prefabName = Path.GetFileNameWithoutExtension(path);
            GameObject prefab = Resources.Load<GameObject>(relativePath + prefabName);
            Debug.Log(prefabName + "---------------");

            //if (prefab != null)
            //{
                // Create a button from the prefab
            GameObject button = Instantiate(buttonPrefab, buttonsParent);

            button.GetComponentInChildren<TextMeshProUGUI>().text = prefabName; // Set button text or image here

          button.GetComponent<Button>().onClick.AddListener(() => SetCurrentPrefab(prefab));

            //}
            //else
            //{
            //    Debug.LogError("Failed to load prefab: " + path);
            //}
        }
    }


     void SetCurrentPrefab(GameObject prefab)
     {
        Debug.Log(prefab);
        FurniturePlacement.Instance.SetNewFurniture(prefab);
     }
}
