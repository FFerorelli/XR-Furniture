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
    [SerializeField] public float _rotationSpeed = 90f;

    protected Material currentMaterial;
    protected double epsilon = 0.005;
    protected Vector3 offset;
    protected Vector3 bottomOffset;
    protected float prefabHeight;
    protected Rigidbody rigidBody;

    private void Start()
    {

    }
    private void Update()
    {
        if (isPlaceble == true) currentMaterial.color = Color.green;
        else currentMaterial.color = Color.red;
    }

    public virtual void FollowRayHit((Vector3 point, Vector3 normal, bool hit) ray) 
    {
    }

    public void HandleRotation()
    {
        var thumbStick = OVRInput.Axis2D.PrimaryThumbstick;

        Vector2 thumbStickPos = OVRInput.Get(thumbStick, OVRInput.Controller.RTouch);

        if (thumbStickPos != Vector2.zero)
        {
            
            float rotateAmount = -thumbStickPos.x * _rotationSpeed * Time.fixedDeltaTime;
            transform.Rotate(Vector3.up, rotateAmount, Space.Self);
        }
    }


}
