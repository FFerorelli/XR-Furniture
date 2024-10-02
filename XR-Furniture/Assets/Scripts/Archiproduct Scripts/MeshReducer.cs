using UnityEngine;
using UnityMeshSimplifier; // Ensure you have the UnityMeshSimplifier library imported

public class MeshReducer : MonoBehaviour
{

    public float simplificationPercentage; // 50% reduction

    void Start()
    {
        ReduceMeshRecursively(transform);
    }

    /// <summary>
    /// Recursively reduces meshes for the given transform and all its children.
    /// </summary>
    /// <param name="current">The current transform to process.</param>
    void ReduceMeshRecursively(Transform current)
    {
        // Process the current GameObject if it has a MeshFilter
        MeshFilter meshFilter = current.GetComponent<MeshFilter>();
        if (meshFilter != null)
        {
            ReduceMesh(meshFilter);
        }

        // Recursively process all child GameObjects
        foreach (Transform child in current)
        {
            ReduceMeshRecursively(child);
        }
    }

    /// <summary>
    /// Reduces the mesh of the given MeshFilter based on the simplification percentage.
    /// </summary>
    /// <param name="meshFilter">The MeshFilter component to simplify.</param>
    void ReduceMesh(MeshFilter meshFilter)
    {
        Mesh originalMesh = meshFilter.sharedMesh;
        if (originalMesh == null)
        {
            Debug.LogWarning($"MeshReducer: No mesh found on {meshFilter.gameObject.name}");
            return;
        }

        // Initialize the simplifier with the original mesh
        MeshSimplifier simplifier = new MeshSimplifier();
        simplifier.Initialize(originalMesh);

        // Perform the simplification
        simplifier.SimplifyMesh(simplificationPercentage);

        // Assign the simplified mesh back to the MeshFilter
        Mesh simplifiedMesh = simplifier.ToMesh();
        if (simplifiedMesh != null)
        {
            meshFilter.mesh = simplifiedMesh;
            Debug.Log($"MeshReducer: Simplified mesh on {meshFilter.gameObject.name} by {simplificationPercentage * 100}%");
        }
        else
        {
            Debug.LogWarning($"MeshReducer: Simplification failed for {meshFilter.gameObject.name}");
        }
    }
}
