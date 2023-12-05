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

    void Start()
    {
        CreateGrid();
    }

    void CreateGrid()
    {
        string[] prefabPaths = Directory.GetFiles(folderPath, "*.prefab");
        
        foreach (var path in prefabPaths)/* (int i = 0; i < prefabPaths.Length; i++)*/
        {

            string prefabName = Path.GetFileNameWithoutExtension(path);
            GameObject prefab = Resources.Load<GameObject>(prefabName);

            //if (prefab != null)
            //{
                // Create a button from the prefab
                GameObject button = Instantiate(buttonPrefab, buttonsParent);

                button.GetComponentInChildren<TextMeshProUGUI>().text = prefabName; // Set button text or image here

                // Add functionality if needed (e.g., onClick listeners)
                // button.GetComponent<Button>().onClick.AddListener(() => YourFunction(prefab));

                // You may adjust the position or layout here based on your requirements 
            //}
            //else
            //{
            //    Debug.LogError("Failed to load prefab: " + path);
            //}
        }
    }

    // Example function for button onClick listener
    // void YourFunction(GameObject prefab)
    // {
    //     Instantiate(prefab, Vector3.zero, Quaternion.identity);
    // }
}
