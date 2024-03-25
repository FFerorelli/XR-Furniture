using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CategoryMenu : MonoBehaviour
{

    public GameObject tablesList;
    public GameObject chairsAndSofaList;
    public GameObject bedsList;
    public GameObject closetsList;
    public GameObject bathroomList;
    public GameObject wallArtList;
    public GameObject electronicsList;
    public GameObject kitchenList;
    public GameObject lightsList;
    public GameObject othersList;

    public Button tables; 
    public Button chairsAndSofa; 
    public Button beds; 
    public Button closets; 
    public Button bathroom; 
    public Button wallArt; 
    public Button electronics; 
    public Button kitchen; 
    public Button lights; 
    public Button others;

    public PopulatePrefabList[] populatePrefabLists;

    public void UIEnabler(int index)
    {
        // Activate the correct category UI
        GameObject[] categoriesMenus = new GameObject[] { tablesList, chairsAndSofaList, bedsList, closetsList, bathroomList, wallArtList, electronicsList, kitchenList, lightsList, othersList };

        for (int i = 0; i < categoriesMenus.Length; i++)
        {
            categoriesMenus[i].SetActive(i == index);
        }

        // Stop the instantiation process for all PopulatePrefabList scripts
        foreach (var populatePrefabList in populatePrefabLists)
        {
            populatePrefabList.StopGridCreation();
        }

        // Start the instantiation process for the selected category
        populatePrefabLists[index].StartGridCreation();
    }

    // Start is called before the first frame update
    void Start()
    {

        tables.onClick.AddListener(() => UIEnabler(0));
        chairsAndSofa.onClick.AddListener(() => UIEnabler(1));
        beds.onClick.AddListener(() => UIEnabler(2));
        closets.onClick.AddListener(() => UIEnabler(3));
        bathroom.onClick.AddListener(() => UIEnabler(4));
        wallArt.onClick.AddListener(() => UIEnabler(5));
        electronics.onClick.AddListener(() => UIEnabler(6));
        kitchen.onClick.AddListener(() => UIEnabler(7));
        lights.onClick.AddListener(() => UIEnabler(8));
        others.onClick.AddListener(() => UIEnabler(9));
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
