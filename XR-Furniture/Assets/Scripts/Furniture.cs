
using System.Collections.Generic;
using UnityEngine;

public abstract class Furniture : MonoBehaviour
{
    public bool isPlaceble;
    public LayerMask layer;

    [SerializeField] protected Material greenMat;
    [SerializeField] protected Material redMat;

   // public CustomElement[] customizableElements;

    protected float speed = 3.5f;
    protected float _rotationSpeed = 90f;
    protected double epsilon = 0.03;
    protected Material currentMaterial;
    protected Vector3 offset;
    protected Vector3 bottomOffset;
    protected float prefabHeight;
    protected float objectHeight;
    protected Rigidbody rigidBody;
    protected List<Renderer> renderers = new List<Renderer>(); // List to store all MeshRenderer components

    protected virtual void Start()
    {
        rigidBody = GetComponent<Rigidbody>();

        // currentMaterial = GetComponent<MeshRenderer>().material;
        // Get all Renderers in this object and its children
        renderers.AddRange(GetComponentsInChildren<Renderer>());
    }

    protected virtual void Update()
    {
        //currentMaterial = isPlaceble ? greenMat : redMat;
        //GetComponent<MeshRenderer>().material = currentMaterial;
        // Debug.Log("rot.x ----------" + transform.rotation.eulerAngles.x);


        // Choose the material based on the isPlaceable flag
        Material targetMaterial = isPlaceble ? greenMat : redMat;

        // Loop through all renderers and change their material
        foreach (Renderer rend in renderers)
        {
            // If the renderer has multiple materials, change all of them
            Material[] materials = rend.materials;
            for (int i = 0; i < materials.Length; i++)
            {
                materials[i] = targetMaterial;
            }
            rend.materials = materials; // Apply the updated materials
        }
    }

    public virtual void FollowRayHit((Vector3 point, Vector3 normal, bool hit) ray)
    {
        Vector3 direction = ray.point - transform.position;
        float step = Time.fixedDeltaTime * speed;
        Vector3 newPosition = transform.position + direction.normalized * step;

        if ((newPosition - ray.point).sqrMagnitude < step * step)
        {
            newPosition = ray.point;
        }
        //Debug.Log("newPosition ----------" + newPosition);
        //Debug.Log("rigidBody ----------" + rigidBody.name);
        rigidBody.MovePosition(newPosition);
    }

    public void HandleRotation()
    {
        Vector2 thumbStickPos = OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick, OVRInput.Controller.RTouch);

        if (thumbStickPos != Vector2.zero)
        {
            float rotateAmount = -thumbStickPos.x * _rotationSpeed * Time.fixedDeltaTime;
            
            if (transform.rotation.eulerAngles.x == 0)
            {
                //Debug.Log("transform.rotation.eulerAngles.x == 0 ----------" + transform.rotation.eulerAngles.x);
                transform.Rotate(Vector3.up, rotateAmount, Space.Self);
            }
            else
            {
                //Debug.Log("NOT 0 ----------" + transform.rotation.eulerAngles.x);
                transform.Rotate(Vector3.forward, rotateAmount, Space.Self);
            }

        }
    }
}