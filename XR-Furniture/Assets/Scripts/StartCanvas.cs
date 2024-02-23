using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartCanvas : MonoBehaviour
{
    public Button start;
    public GameObject modesMenu;

    [SerializeField] private Transform _newTransform;
    [SerializeField] private GameObject player;

    private void Start()
    {
        //transform.position = _newTransform.position;
        //transform.LookAt(player.transform.position);
        //transform.Rotate(0, 180, 0);
        start.onClick.AddListener(() => StartApp());
    }

    public void StartApp()
    {
        modesMenu.SetActive(true);
        gameObject.SetActive(false);
    }
}
