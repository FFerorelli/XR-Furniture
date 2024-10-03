using UnityEngine;

public class SmallObject : Furniture
{
    [SerializeField] private float verticalThreshold = 0.95f;
    private float epsilon = 0.03f;

    protected override void Start()
    {
        base.Start();
        isPlaceable = true;
    }

    public override void FollowRayHit((Vector3 point, Vector3 normal, bool hit) ray)
    {
        base.FollowRayHit(ray);

        Ray downRay = new Ray(transform.position, -transform.up);
        if (Physics.Raycast(downRay, out RaycastHit hitInfo, 100.0f))
        {
            float dotProduct = Vector3.Dot(hitInfo.normal.normalized, Vector3.up);
            isPlaceable = dotProduct >= verticalThreshold && hitInfo.distance < epsilon;
        }
        else
        {
            isPlaceable = false;
        }
    }
}
