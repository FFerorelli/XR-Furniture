using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorFurniture : Furniture
{
    [SerializeField] private float speed = 2.5f;

    void Start()
    {

        Debug.Log("FloorFurniture");
        prefabHeight = transform.localScale.y / 2;
        offset = new Vector3(0, prefabHeight, 0);
        rigidBody = GetComponent<Rigidbody>();

    }

    public override void FollowRayHit((Vector3 point, Vector3 normal, bool hit) ray)
    {

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
