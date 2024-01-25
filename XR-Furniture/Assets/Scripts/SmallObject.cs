using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmallObject : Furniture
{
    [SerializeField] private float verticalThreshold = 0.95f;

    void Start()
    {

        Debug.Log("SmallObject");
        isPlaceble = true;
        // offset = new Vector3(0, prefabHeight, 0);
        float offset = 0.005f;
        Collider collider = GetComponent<Collider>();
        objectHeight = collider.bounds.size.y/* - offset*/;
        // prefabHeight = transform.localScale.y / 2 - offset;
        rigidBody = GetComponent<Rigidbody>();
        currentMaterial = GetComponent<MeshRenderer>().material;
    }

    //void OnDrawGizmos()
    //{
    //    float offset = 0.1f; // Adjust this value as needed
    //    Collider collider = GetComponent<Collider>();
    //    float objectHeight = collider.bounds.size.y;
    //    Vector3 sphereCenter = transform.position - new Vector3(0, objectHeight / 2 /*- offset*/, 0);
    //    float sphereRadius = 0.1f; // Adjust this value as needed

    //    Gizmos.color = Color.red;
    //    Gizmos.DrawWireSphere(sphereCenter, sphereRadius);
    //}

    //void OnDrawGizmos()
    //{
    //    float offset = 0.1f; // Adjust this value as needed
    //    MeshCollider meshCollider = GetComponent<MeshCollider>();

    //    if (meshCollider != null)
    //    {
    //        Vector3 boxCenter = meshCollider.bounds.center - new Vector3(0, meshCollider.bounds.extents.y - offset, 0);
    //        Vector3 boxSize = new Vector3(meshCollider.bounds.extents.x * 2, offset * 2, meshCollider.bounds.extents.z * 2);

    //        Gizmos.color = Color.red;
    //        Gizmos.DrawWireCube(boxCenter, boxSize);
    //    }
    //}
    public override void FollowRayHit((Vector3 point, Vector3 normal, bool hit) ray)
    {
        base.FollowRayHit(ray);

        var downRay = new Ray(transform.position/* - offset*/, -transform.up);
        Debug.DrawRay(downRay.origin, downRay.direction * 100.0f, Color.green);
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


        //float offset = 0.1f; // Adjust this value as needed
        //Collider collider = GetComponent<Collider>();
        //float objectHeight = collider.bounds.size.y;
        //Vector3 sphereCenter = transform.position - new Vector3(0, objectHeight / 2/* - offset*/, 0);
        //float sphereRadius = 0.1f; // Adjust this value as needed

        //if (Physics.CheckSphere(sphereCenter, sphereRadius))
        //{
        //    isPlaceble = true;
        //    Debug.Log("Physics.CheckSphere = true");
        //}
        //else
        //{
        //    isPlaceble = false;
        //    Debug.Log("Physics.CheckSphere = false");
        //}




        //float offset = 0.1f; // Adjust this value as needed
        //MeshCollider meshCollider = GetComponent<MeshCollider>();

        //if (meshCollider != null)
        //{
        //    Vector3 boxCenter = meshCollider.bounds.center - new Vector3(0, meshCollider.bounds.extents.y - offset, 0);
        //    Vector3 boxHalfExtents = new Vector3(meshCollider.bounds.extents.x, offset, meshCollider.bounds.extents.z);

        //    Collider[] overlappingColliders = Physics.OverlapBox(boxCenter, boxHalfExtents);
        //    foreach (Collider collider in overlappingColliders)
        //    {
        //        if (collider.transform.position.y < boxCenter.y)
        //        {
        //            isPlaceble = true;
        //            Debug.Log("Physics.OverlapBox = true for collider: " + collider.name);
        //            return; // Exit the method as soon as we find a collider that is below the box
        //        }
        //    }

        //    If we reach this point, none of the colliders are below the box
        //    isPlaceble = false;
        //    Debug.Log("Physics.OverlapBox = false for all colliders");
        //}
        //else
        //{
        //    Debug.Log("No MeshCollider found");
        //}





        // var previewRot = gameObject.transform.rotation;
        // previewRot = Quaternion.FromToRotation(Vector3.up, hit.normal);
        // gameObject.transform.up = hit.normal;

        //var previewPos = gameObject.transform.position;
        //var targetPos = ray.point + offset;
        //Vector3 direction = targetPos - previewPos;
        //float distance = direction.magnitude;
        //float step = distance * Time.fixedDeltaTime * speed;


        //rigidBody.MovePosition(previewPos + direction.normalized * step);

        // Stop moving when close to the hit point
        // if (distance < 0.1f) rigidBody.velocity = Vector3.zero;
        // teleport instead?

        //gameObject.transform.position = ray.point + offset;
        //gameObject.transform.up = hit.normal;

    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            Debug.Log("Collision detected with object tagged as 'Ground'");
            isPlaceble = true;
        }
    }


    //private void OnCollisionExit(Collision collision)
    //{
    //    if (collision.gameObject.CompareTag("Ground"))
    //    {
    //        Debug.Log("Collision with object tagged as 'Furniture' ended");
    //        isPlaceble = true;
    //    }
    //}
}
