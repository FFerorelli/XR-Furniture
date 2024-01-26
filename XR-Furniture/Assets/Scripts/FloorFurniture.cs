using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorFurniture : Furniture
{
    protected override void Start()
    {
        base.Start();
        isPlaceble = true;
        prefabHeight = transform.localScale.y / 2;
        offset = new Vector3(0, prefabHeight, 0);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Furniture"))
        {
            isPlaceble = false;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Furniture"))
        {
            isPlaceble = true;
        }
    }
}
