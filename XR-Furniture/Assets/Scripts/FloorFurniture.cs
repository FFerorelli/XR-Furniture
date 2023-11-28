using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorFurniture : Furniture
{


    void Start()
    {
        Debug.Log("FloorFurniture");

        prefabHeight = transform.localScale.y / 2;
        offset = new Vector3(0, prefabHeight, 0);
        rigidBody = GetComponent<Rigidbody>();
        currentMaterial = GetComponent<MeshRenderer>().material;

    }
    private void Update()
    {
        if (isPlaceble == true) currentMaterial.color = Color.green;
        else currentMaterial.color = Color.red;
    }
    public override void FollowRayHit((Vector3 point, Vector3 normal, bool hit) ray)
    {

        var downRay = new Ray(transform.position - offset /*+ bottomOffset*/, -transform.up);
        var downRayGroundHit = Physics.Raycast(downRay, out var hit, 100.0f);
        float dotProduct = Vector3.Dot(hit.normal.normalized, Vector3.up);

        //if (dotProduct >= verticalThreshold)
        //{
        //    isPlaceble = true;
        //   // currentMaterial.color = Color.green;
        //}
        //else
        //{
        //    isPlaceble = false;
        //   // currentMaterial.color = Color.red;
        //}

        //if (isPlaceble == true) currentMaterial.color = Color.green;
        //else currentMaterial.color = Color.red;





        var previewPos = gameObject.transform.position;
        var targetPos = ray.point + offset;
        Vector3 direction = targetPos - previewPos;
        float distance = direction.magnitude;
        float step = distance * Time.fixedDeltaTime * speed;
        rigidBody.MovePosition(previewPos + direction.normalized * step); // teleport instead?


        // Stop moving when close to the hit point
        if (distance < 0.1f) rigidBody.velocity = Vector3.zero;
    }
}