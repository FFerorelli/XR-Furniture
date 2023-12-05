using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CategoryMenu : MonoBehaviour
{
    public GameObject floorFurnitureList;
    public GameObject wallFurnituresList;
    public GameObject smallObjectsFurnituresList;

    public Button floorFurnitures; 
    public Button wallFurnitures; 
    public Button smallObjectsFurnitures; 

    public void UIEnabler(int index)
    {
        GameObject[] categoriesMenus = new GameObject[] { floorFurnitureList , wallFurnituresList, smallObjectsFurnituresList };

        for (int i = 0; i < categoriesMenus.Length; i++)
        {
            categoriesMenus[i].SetActive(i == index);   
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        floorFurnitures.onClick.AddListener(() => UIEnabler(0));
        floorFurnitures.onClick.AddListener(() => UIEnabler(1));
        floorFurnitures.onClick.AddListener(() => UIEnabler(2));
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
