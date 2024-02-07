using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmallObject : Furniture
{
    [SerializeField] private float verticalThreshold = 0.95f;

    protected override void Start()
    {
        base.Start();
        isPlaceble = true;
        Collider collider = GetComponent<Collider>();
        objectHeight = collider.bounds.size.y;
    }

    public override void FollowRayHit((Vector3 point, Vector3 normal, bool hit) ray)
    {
        base.FollowRayHit(ray);

        var downRay = new Ray(transform.position, -transform.up);
        var downRayGroundHit = Physics.Raycast(downRay, out var hit, 100.0f);
        float dotProduct = Vector3.Dot(hit.normal.normalized, Vector3.up);

        isPlaceble = dotProduct >= verticalThreshold && hit.distance < epsilon;
    }

    //private void OnCollisionEnter(Collision collision)
    //{
    //    if (collision.gameObject.CompareTag("Ground"))
    //    {
    //        isPlaceble = true;
    //    }
    //}
}
