using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenshotCapturer : MonoBehaviour
{
    public List<GameObject> prefabs;
    public Transform spawnTransform; // Use this Transform to set spawn position and rotation in the Inspector
    public string screenshotDirectory = "Assets/Resources/Thumbnails";
    public float desiredScreenHeightPortion = 0.5f; // The portion of the screen height the object should fill

    private Camera mainCamera;

    private void Awake()
    {
        mainCamera = Camera.main; // Ensure the camera is set to orthographic in the editor
        mainCamera.orthographic = true; // Set camera to orthographic just in case it's not set in the editor
    }

    private void Start()
    {
        StartCoroutine(CaptureScreenshots());
    }

    private IEnumerator CaptureScreenshots()
    {
        foreach (var prefab in prefabs)
        {
            // Check if the prefab name contains "electronics" and adjust the position if it does
            Vector3 spawnPosition = spawnTransform.position;
            if (prefab.name.ToLower().Contains("electronics"))
            {
                spawnPosition.y += 0.2f; // Move up along the y-axis by 0.2 units
            }

            GameObject instance = Instantiate(prefab, spawnPosition, spawnTransform.rotation);
            Bounds bounds = CalculateObjectBounds(instance);

            AdjustCameraSize(bounds);

            yield return new WaitForEndOfFrame(); // Wait for the object to be rendered

            string screenshotPath = $"{screenshotDirectory}/{prefab.name}.png";
            ScreenCapture.CaptureScreenshot(screenshotPath);
            Debug.Log($"Screenshot captured for {prefab.name} at {screenshotPath}");

            Destroy(instance);

            yield return new WaitForSeconds(0.5f); // Wait a bit before capturing the next screenshot
        }

        Debug.Log("All screenshots captured.");
    }

    private Bounds CalculateObjectBounds(GameObject obj)
    {
        var renderers = obj.GetComponentsInChildren<Renderer>();
        Bounds bounds = new Bounds(spawnTransform.position, Vector3.zero);
        foreach (var renderer in renderers)
        {
            bounds.Encapsulate(renderer.bounds);
        }
        return bounds;
    }

    private void AdjustCameraSize(Bounds bounds)
    {
        // Calculate the orthographic size needed to maintain the desired portion of screen height
        float objectSize = Mathf.Max(bounds.extents.x * 2.0f, bounds.extents.y * 2.0f, bounds.extents.z * 2.0f);
        float cameraHeight = objectSize / desiredScreenHeightPortion;
        mainCamera.orthographicSize = cameraHeight / 2.0f; // Camera size is half of the cameraHeight because it measures from the center to the top
    }
}

