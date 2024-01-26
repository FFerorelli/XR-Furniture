using UnityEngine;

public abstract class Furniture : MonoBehaviour
{
    public bool isPlaceble;
    public LayerMask layer;
    [SerializeField] protected Material greenMat;
    [SerializeField] protected Material redMat;
    [SerializeField] public float speed = 2.5f;
    [SerializeField] public float _rotationSpeed = 90f;
    protected double epsilon = 0.01;
    protected Material currentMaterial;
    protected Vector3 offset;
    protected Vector3 bottomOffset;
    protected float prefabHeight;
    protected float objectHeight;
    protected Rigidbody rigidBody;

    protected virtual void Start()
    {
        rigidBody = GetComponent<Rigidbody>();
        currentMaterial = GetComponent<MeshRenderer>().material;
    }

    protected virtual void Update()
    {
        currentMaterial.color = isPlaceble ? Color.green : Color.red;
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

        rigidBody.MovePosition(newPosition);
    }

    public void HandleRotation()
    {
        Vector2 thumbStickPos = OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick, OVRInput.Controller.RTouch);

        if (thumbStickPos != Vector2.zero)
        {
            float rotateAmount = -thumbStickPos.x * _rotationSpeed * Time.fixedDeltaTime;
            transform.Rotate(Vector3.up, rotateAmount, Space.Self);
        }
    }
}