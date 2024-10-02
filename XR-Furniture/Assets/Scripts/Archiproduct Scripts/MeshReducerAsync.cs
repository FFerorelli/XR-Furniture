using System.Collections;
using System.Threading.Tasks;
using UnityEngine;
using UnityMeshSimplifier;

public class MeshReducerAsync
{

    private float simplificationPercentage;

    public MeshReducerAsync(float simplificationPercentage)
    {
        this.simplificationPercentage = simplificationPercentage;
    }

    public IEnumerator ReduceMeshAsync(Transform root)
    {
        // Start the mesh reduction task
        Task reductionTask = ReduceMeshRecursivelyAsync(root);

        // Wait until the task is completed
        while (!reductionTask.IsCompleted)
        {
            yield return null; // Wait for the next frame
        }

        if (reductionTask.IsFaulted)
        {
            Debug.LogError("Mesh reduction failed: " + reductionTask.Exception);
        }
        else
        {
            Debug.Log("Mesh reduction completed");
        }
    }

    private async Task ReduceMeshRecursivelyAsync(Transform current)
    {
        // Process the current GameObject if it has a MeshFilter
        MeshFilter meshFilter = current.GetComponent<MeshFilter>();
        if (meshFilter != null)
        {
            await ReduceMeshAsync(meshFilter);
        }

        // Recursively process all child GameObjects
        foreach (Transform child in current)
        {
            await ReduceMeshRecursivelyAsync(child);
        }
    }

    private async Task ReduceMeshAsync(MeshFilter meshFilter)
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

        // Perform the simplification asynchronously
        await Task.Run(() =>
        {
            simplifier.SimplifyMesh(simplificationPercentage);
        });

        // Back on the main thread, apply the simplified mesh
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
