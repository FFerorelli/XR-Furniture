using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartCanvas : MonoBehaviour
{
    public Button start;
    public GameObject modesMenu;
    private void Start()
    {
        start.onClick.AddListener(() => StartApp());
    }

    public void StartApp()
    {
        modesMenu.SetActive(true);
        gameObject.SetActive(false);
    }
}
