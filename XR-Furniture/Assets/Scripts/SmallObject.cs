using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmallObject : Furniture
{


    void Start()
    {

        Debug.Log("SmallObject");

        prefabHeight = transform.localScale.y / 2;
        offset = new Vector3(0, prefabHeight, 0);
        rigidBody = GetComponent<Rigidbody>();
        currentMaterial = GetComponent<MeshRenderer>().material;
    }

    public override void FollowRayHit((Vector3 point, Vector3 normal, bool hit) ray)
    {
        
        var downRay = new Ray(transform.position - offset, -transform.up);
        var downRayGroundHit = Physics.Raycast(downRay, out var hit, 100.0f);
        float dotProduct = Vector3.Dot(hit.normal.normalized, Vector3.up);


        if (dotProduct >= verticalThreshold && hit.distance < epsilon)
        {
            isPlaceble = true;
        }
        else
        {
            isPlaceble = false;
        }

       // var previewRot = gameObject.transform.rotation;
       // previewRot = Quaternion.FromToRotation(Vector3.up, hit.normal);
       // gameObject.transform.up = hit.normal;

        var previewPos = gameObject.transform.position;
        var targetPos = ray.point + offset;
        Vector3 direction = targetPos - previewPos;
        float distance = direction.magnitude;
        float step = distance * Time.fixedDeltaTime * speed;


        rigidBody.MovePosition(previewPos + direction.normalized * step);

        // Stop moving when close to the hit point
        // if (distance < 0.1f) rigidBody.velocity = Vector3.zero;
        // teleport instead?

        //gameObject.transform.position = ray.point + offset;
        //gameObject.transform.up = hit.normal;



    }
}
