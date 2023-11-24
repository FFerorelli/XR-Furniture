using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmallObject : Furniture
{
    [SerializeField] private float speed = 2.5f;
    private Material currentMaterial;
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
        float dotProduct = Vector3.Dot(ray.normal.normalized, Vector3.up);

        if (dotProduct >= verticalThreshold)
        {
            Debug.Log(dotProduct);
            isPlaceble = true;
            currentMaterial.color = Color.green;
        }
        else
        {
            Debug.Log(dotProduct);
            isPlaceble = false;
            currentMaterial.color = Color.red;
        }
        Debug.Log(isPlaceble);
        var previewPos = gameObject.transform.position;
        var targetPos = ray.point + offset;
        Vector3 direction = targetPos - previewPos;
        float distance = direction.magnitude;
        float step = distance * Time.fixedDeltaTime * speed;
        rigidBody.MovePosition(previewPos + direction.normalized * step);

        // Stop moving when close to the hit point
        if (distance < 0.1f) rigidBody.velocity = Vector3.zero;
    }
}
