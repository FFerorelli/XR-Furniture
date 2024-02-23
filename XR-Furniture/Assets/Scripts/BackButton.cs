using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BackButton : MonoBehaviour
{

    public Button backButton;
    public GameObject[] objectsToDisable;
    public GameObject modesMenu;

    // Start is called before the first frame update
    void Start()
    {
        backButton.onClick.AddListener(() => BackToModes());
    }

    private void BackToModes()
    {
        modesMenu.SetActive(true);
        foreach (var item in objectsToDisable)
        {
            item.SetActive(false);              
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
