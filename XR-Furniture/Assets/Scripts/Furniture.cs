using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Furniture : MonoBehaviour
{
    public bool isPlaceble;
    public LayerMask layer;

    
    [SerializeField] protected Material greenMat;
    [SerializeField] protected Material redMat;
    [SerializeField] public float speed = 2.5f;
    [SerializeField] public float _rotationSpeed = 90f;
    [SerializeField] protected double epsilon = 0.005;

    protected Material currentMaterial;
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

        var downRay = new Ray(transform.position - offset /*+ bottomOffset*/, -transform.up);
        var downRayGroundHit = Physics.Raycast(downRay, out var hit, 100.0f);
        float dotProduct = Vector3.Dot(hit.normal.normalized, Vector3.up);

        //if (dotProduct >= verticalThreshold)
        //{
        //    isPlaceble = true;
        //}
        //else
        //{
        //    isPlaceble = false;
        //}

        var previewPos = gameObject.transform.position;
        var targetPos = ray.point + offset;
        Vector3 direction = targetPos - previewPos;
        float distance = direction.magnitude;
        float step = distance * Time.fixedDeltaTime * speed;
        rigidBody.MovePosition(previewPos + direction.normalized * step); // teleport instead?

        // Stop moving when close to the hit point
       // if (distance < 0.1f) rigidBody.velocity = Vector3.zero;
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
