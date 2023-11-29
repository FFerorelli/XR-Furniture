using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorFurniture : Furniture
{


    void Start()
    {
        Debug.Log("FloorFurniture");
        isPlaceble = true;
        prefabHeight = transform.localScale.y / 2;
        offset = new Vector3(0, prefabHeight, 0);
        rigidBody = GetComponent<Rigidbody>();
        currentMaterial = GetComponent<MeshRenderer>().material;

    }

    //public override void FollowRayHit((Vector3 point, Vector3 normal, bool hit) ray)
    //{
    //    base.FollowRayHit(ray);

    //    //// Stop moving when close to the hit point
    //    //if (distance < 0.1f) rigidBody.velocity = Vector3.zero;
    //}

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
