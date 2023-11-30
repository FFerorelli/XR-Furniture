using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallFurniture : Furniture
{
    [SerializeField] private float horizontalThreshold = 0.96f;
    [SerializeField] private LayerMask wallLayerToCheck;
    private float minWallDistance = 0.05f;
    void Start()
    {
        Debug.Log("FloorFurniture");
        isPlaceble = true;
        prefabHeight = transform.localScale.y / 2;
        offset = new Vector3(0, prefabHeight, 0);
        rigidBody = GetComponent<Rigidbody>();
        currentMaterial = GetComponent<MeshRenderer>().material;

    }
    private void FixedUpdate()
    {
        isPlaceble = IsPlacebleOnWall();
    }
    public override void FollowRayHit((Vector3 point, Vector3 normal, bool hit) ray)
    {
        base.FollowRayHit(ray);
    }


    private bool IsPlacebleOnWall()
    {
        bool checkForward = CheckPlacement(transform.forward, out var hitForward);
        bool checkBack = CheckPlacement(-transform.forward, out var hitBack);

        Debug.Log(hitForward.normal + " " + hitBack.normal);
        Debug.Log(checkForward);
        Debug.Log(checkBack);

        return checkBack || checkForward;
    }

    private bool CheckPlacement(Vector3 direction, out RaycastHit hit)
    {
        bool raycastResult = Physics.Raycast(transform.position, direction, out hit, minWallDistance, wallLayerToCheck);
        float dot = Vector3.Dot(hit.normal, direction);

        if (raycastResult && Mathf.Abs(dot) >= horizontalThreshold)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Furniture"))
        {
            Debug.Log("Collision detected with object tagged as 'Furniture'");
            isPlaceble = false;
        }
    }


    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Furniture"))
        {
            Debug.Log("Collision with object tagged as 'Furniture' ended");
            isPlaceble = true;
        }
    }

}
