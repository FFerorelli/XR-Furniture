using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class Furniture : MonoBehaviour
{
    public bool isPlaceable = false;
    public LayerMask layer;

    [SerializeField] protected Material greenMat;
    [SerializeField] protected Material redMat;

    protected Rigidbody rigidBody;
    protected List<Renderer> renderers = new List<Renderer>();
    private Dictionary<Renderer, Material[]> originalMaterials = new Dictionary<Renderer, Material[]>();
    private bool isPlaced = false;

    protected float prefabHeight;
    protected Vector3 offset;
    public event Action OnPlaced;

    protected virtual void Start()
    {
        // Initialization moved to InitializeFurniture()
    }

    public void InitializeFurniture()
    {
        rigidBody = GetComponent<Rigidbody>();
        if (rigidBody == null)
        {
            rigidBody = gameObject.AddComponent<Rigidbody>();
            rigidBody.useGravity = false;
            rigidBody.constraints = RigidbodyConstraints.FreezeRotation;
        }

        renderers.AddRange(GetComponentsInChildren<Renderer>());

        // Save Original Materials
        SaveOriginalMaterials();

        // Assign Preview Materials
        AssignPreviewMaterials();

        // ** Collider creation is moved to after mesh reduction **
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
                previewMaterials[i] = isPlaceable ? greenMat : redMat;
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
    }

    protected virtual void Update()
    {
        if (!isPlaced)
        {
            AssignPreviewMaterials();
        }
    }

    public void Place()
    {
        isPlaced = true;
        RestoreOriginalMaterials();
        rigidBody.constraints = RigidbodyConstraints.FreezeAll;

        // Invoke the OnPlaced event
        OnPlaced?.Invoke();
    }

    public virtual void FollowRayHit((Vector3 point, Vector3 normal, bool hit) ray)
    {
        Vector3 direction = ray.point - transform.position;
        float step = Time.fixedDeltaTime * 3.5f; // Speed can be adjusted or made a variable
        Vector3 newPosition = transform.position + direction.normalized * step;

        if ((newPosition - ray.point).sqrMagnitude < step * step)
        {
            newPosition = ray.point;
        }

        rigidBody.MovePosition(newPosition);
    }

    public void HandleRotation()
    {
        Vector2 thumbStickPos = OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick, OVRInput.Controller.RTouch);

        if (thumbStickPos != Vector2.zero)
        {
            float rotateAmount = -thumbStickPos.x * 90f * Time.fixedDeltaTime;

            if (Mathf.Approximately(transform.rotation.eulerAngles.x, 0))
            {
                transform.Rotate(Vector3.up, rotateAmount, Space.Self);
            }
            else
            {
                transform.Rotate(Vector3.forward, rotateAmount, Space.Self);
            }
        }
    }
}
