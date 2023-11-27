using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Furniture : MonoBehaviour
{
    public bool isPlaceble;
    public LayerMask layer;

    [SerializeField] protected float verticalThreshold = 0.95f;
    [SerializeField] protected Material greenMat;
    [SerializeField] protected Material redMat;
    [SerializeField] public float speed = 2.5f;

    protected Material currentMaterial;
    protected double epsilon = 0.001;
    protected Vector3 offset;
    protected Vector3 bottomOffset;
    protected float prefabHeight;
    protected Rigidbody rigidBody;

    private void Start()
    {
        //prefabHeight = transform.localScale.y / 2;
        //offset = new Vector3(0, prefabHeight, 0);
        //rigidBody = GetComponent<Rigidbody>();
        //currentMaterial = GetComponent<MeshRenderer>().material;
    }

    public virtual void FollowRayHit((Vector3 point, Vector3 normal, bool hit) ray) 
    {
    }

}
