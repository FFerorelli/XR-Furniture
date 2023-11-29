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

    public override void FollowRayHit((Vector3 point, Vector3 normal, bool hit) ray)
    {

        var downRay = new Ray(transform.position - offset /*+ bottomOffset*/, -transform.up);
        var downRayGroundHit = Physics.Raycast(downRay, out var hit, 100.0f);
        float dotProduct = Vector3.Dot(hit.normal.normalized, Vector3.up);

        //if (dotProduct >= verticalThreshold)
        //{
        //    isPlaceble = true;
        //}
        //else
        //{
        //    isPlaceble = false;
        //}
       
        var previewPos = gameObject.transform.position;
        var targetPos = ray.point + offset;
        Vector3 direction = targetPos - previewPos;
        float distance = direction.magnitude;
        float step = distance * Time.fixedDeltaTime * speed;
        rigidBody.MovePosition(previewPos + direction.normalized * step); // teleport instead?


        // Stop moving when close to the hit point
        if (distance < 0.1f) rigidBody.velocity = Vector3.zero;
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
