using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Furniture : MonoBehaviour
{
    protected Vector3 offset;
    protected float prefabHeight;
    protected Rigidbody rigidBody;
    // FurniturePlacement placement;
    public LayerMask layer;
    public virtual void FollowRayHit((Vector3 point, bool hit) ray) 
    {
    }

}
