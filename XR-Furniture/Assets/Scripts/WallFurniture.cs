using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallFurniture : Furniture
{
    [SerializeField] private float horizontalThreshold = 0.96f;
    [SerializeField] private LayerMask wallLayerToCheck;
    private float minWallDistance = 0.05f;

    protected override void Start()
    {
        base.Start();
        isPlaceble = true;
        prefabHeight = transform.localScale.y / 2;
        offset = new Vector3(0, prefabHeight, 0);


        // Move the object up along the y-axis
        float moveUpDistance = 1.0f; // Set this to the distance you want the object to move up
        transform.position = new Vector3(transform.position.x, transform.position.y + moveUpDistance, transform.position.z);
    }

    private void FixedUpdate()
    {
        isPlaceble = IsPlacebleOnWall();
    }

    private bool IsPlacebleOnWall()
    {
        return CheckPlacement(transform.forward, out _) || CheckPlacement(-transform.forward, out _);
    }

    private bool CheckPlacement(Vector3 direction, out RaycastHit hit)
    {
        bool raycastResult = Physics.Raycast(transform.position, direction, out hit, minWallDistance, wallLayerToCheck);
        return raycastResult && Mathf.Abs(Vector3.Dot(hit.normal, direction)) >= horizontalThreshold;
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
