using UnityEngine;

public class AutoBoxColliderForChildren : MonoBehaviour
{
    private void Awake()
    {
        // Remove the call to AddBoxCollider from Awake
        // AddBoxCollider();
    }

    void Start()
    {
        // Remove the call to AddBoxCollider from Start
         AddBoxCollider();
    }

    // Change AddBoxCollider to public
    public void AddBoxCollider()
    {
        Bounds combinedBounds = new Bounds();
        bool hasBounds = false;

        foreach (MeshFilter meshFilter in GetComponentsInChildren<MeshFilter>())
        {
            Mesh mesh = meshFilter.sharedMesh;
            if (mesh != null)
            {
                // Get the mesh bounds in local space
                Bounds meshBounds = mesh.bounds;

                // Transform the bounds to world space
                Vector3 worldCenter = meshFilter.transform.TransformPoint(meshBounds.center);
                Vector3 worldExtents = meshBounds.extents;
                Vector3 worldMin = worldCenter - worldExtents;
                Vector3 worldMax = worldCenter + worldExtents;

                // Update the combined bounds
                if (hasBounds)
                {
                    combinedBounds.Encapsulate(worldMin);
                    combinedBounds.Encapsulate(worldMax);
                }
                else
                {
                    combinedBounds = new Bounds(worldCenter, worldExtents * 2);
                    hasBounds = true;
                }
            }
        }

        if (hasBounds)
        {
            // Create or get the BoxCollider component
            BoxCollider boxCollider = gameObject.GetComponent<BoxCollider>();
            if (boxCollider == null)
            {
                boxCollider = gameObject.AddComponent<BoxCollider>();
            }

            // Adjust the center and size of the BoxCollider
            Vector3 localCenter = transform.InverseTransformPoint(combinedBounds.center);
            Vector3 localSize = combinedBounds.size;

            boxCollider.center = localCenter;
            boxCollider.size = localSize;

            Debug.Log($"BoxCollider added with center: {boxCollider.center} and size: {boxCollider.size}");
        }
        else
        {
            Debug.LogError("No MeshFilter components found in children.");
        }
    }
}
