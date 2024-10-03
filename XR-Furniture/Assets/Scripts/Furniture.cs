
using System.Collections.Generic;
using UnityEngine;

public abstract class Furniture : MonoBehaviour
{
    public bool isPlaceble;
    public LayerMask layer;
    // ** Add this flag to indicate whether the object is placed **
    private bool isPlaced = false;

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
    private Dictionary<Renderer, Material[]> originalMaterials = new Dictionary<Renderer, Material[]>();

    protected virtual void Start()
    {
        rigidBody = GetComponent<Rigidbody>();
        renderers.AddRange(GetComponentsInChildren<Renderer>());

        // ** Save Original Materials **
        SaveOriginalMaterials();

        // ** Assign Preview Materials **
        AssignPreviewMaterials();
    }
    private void SaveOriginalMaterials()
    {
        originalMaterials.Clear();

        foreach (Renderer rend in renderers)
        {
            originalMaterials[rend] = rend.materials;
        }
    }
    private void AssignPreviewMaterials()
    {
        foreach (Renderer rend in renderers)
        {
            Material[] previewMaterials = new Material[rend.materials.Length];
            for (int i = 0; i < previewMaterials.Length; i++)
            {
                previewMaterials[i] = isPlaceble ? greenMat : redMat;
            }
            rend.materials = previewMaterials;
        }
    }
    public void RestoreOriginalMaterials()
    {
        foreach (var kvp in originalMaterials)
        {
            if (kvp.Key != null)
            {
                kvp.Key.materials = kvp.Value;
            }
        }
        originalMaterials.Clear();
    }
    // Modify the Update method to use AssignPreviewMaterials()
    protected virtual void Update()
    {
        Debug.Log("________________________isPlaced = " + isPlaced);
        if (!isPlaced)
        {
            AssignPreviewMaterials();
        }
    }
    // ** Add this method to mark the object as placed **
    public void SetPlaced()
    {
        isPlaced = true;
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