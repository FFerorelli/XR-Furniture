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
        //isPlaceble = IsPlacebleOnWall();
        base.FollowRayHit(ray);
    }

    private bool IsPlacebleOnWall()
    {

        bool checkForward;
        bool checkBack;

        bool forwardRaycast = Physics.Raycast(transform.position, transform.forward, out var hitForward, minWallDistance, wallLayerToCheck);
        bool backRaycast = Physics.Raycast(transform.position, -transform.forward, out var hitBack, minWallDistance, wallLayerToCheck);

       // int currentLayerIndex = hitForward.collider.gameObject.layer;
        float dot1 = Vector3.Dot(hitForward.normal, transform.forward);

        // Check if the normal of the hit surface is approximately vertical
        if (forwardRaycast && Mathf.Abs(dot1) >= horizontalThreshold)
        {
            checkForward = true;           
        }
        else
        {
            checkForward = false;
        }
        
       // int currentLayerIndex2 = hitBack.collider.gameObject.layer;
        float dot2 = Vector3.Dot(hitBack.normal, -transform.forward);

        // Check if the normal of the hit surface is approximately vertical
        if (backRaycast && Mathf.Abs(dot2) >= horizontalThreshold)
        {
            checkBack = true;
        }
        else
        {
            checkBack = false;
        }
        Debug.Log(dot1 + " " + dot2);
        Debug.Log(checkForward);
        Debug.Log(checkBack);

        return checkBack || checkForward;  



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
