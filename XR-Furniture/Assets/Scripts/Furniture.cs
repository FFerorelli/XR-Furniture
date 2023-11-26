using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Furniture : MonoBehaviour
{
    public bool isPlaceble;
    [SerializeField] protected float verticalThreshold = 0.95f;
    [SerializeField] protected Material greenMat;
    [SerializeField] protected Material redMat;

    protected Vector3 offset;
    protected float prefabHeight;
    protected Rigidbody rigidBody;
    public LayerMask layer;


    public virtual void FollowRayHit((Vector3 point, Vector3 normal, bool hit) ray) 
    {
    }

}
