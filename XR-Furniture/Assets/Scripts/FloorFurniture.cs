using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorFurniture : MonoBehaviour, IFurniture
{
    [SerializeField] private float speed = 2.5f;

    private Vector3 offset;
    private float prefabHeight;
    private Rigidbody rigidBody;

    // Start is called before the first frame update
    void Start()
    {
        prefabHeight = transform.localScale.y / 2;
        offset = new Vector3(0, prefabHeight, 0);
        rigidBody = GetComponent<Rigidbody>();
       // Debug.Log(rigidBody);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        
    }

   public void FollowRayHit((Vector3 point, Vector3 normal, bool hit) ray)
    {
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
