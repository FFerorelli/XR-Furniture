using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenshotCapturer : MonoBehaviour
{
    public List<GameObject> prefabs;
    public Transform prefabPos;
    public string screenshotDirectory = "Assets/Resources/Thumbnails";

    private void Start()
    {
        StartCoroutine(CaptureScreenshots());
    }

    private IEnumerator CaptureScreenshots()
    {
        Camera cam = Camera.main; // Get the main camera
        float cameraAspect = cam.aspect; // Get the camera's aspect ratio

        Vector3 prefabPosition = prefabPos.position;
        Quaternion prefabRotation = prefabPos.rotation;

        prefabPos.gameObject.SetActive(false);
        foreach (var prefab in prefabs)
        {
            GameObject instance = Instantiate(prefab, prefabPosition, prefabRotation);

            yield return new WaitForEndOfFrame();

            // Calculate the bounds of the prefab
            Bounds bounds = instance.GetComponent<Renderer>().bounds;

            // Calculate the necessary distance to fit the bounds in the camera's field of view
            float cameraDistance = Mathf.Max(bounds.size.x / cameraAspect, bounds.size.y) / (2f * Mathf.Tan(0.5f * cam.fieldOfView * Mathf.Deg2Rad));

            // Add an offset to the camera distance
            float offset = 1.0f; // Adjust this value as needed
            cameraDistance += offset;

            // Position the camera at the calculated distance
            cam.transform.position = bounds.center - cam.transform.forward * cameraDistance;

            ScreenCapture.CaptureScreenshot(screenshotDirectory + prefab.name + ".png");
            Debug.Log("Screenshot captured for " + prefab.name);

            Destroy(instance);

            yield return new WaitForSeconds(0.5f);
        }
        prefabPos.gameObject.SetActive(false);
        Debug.Log("All Screenshots captured");
    }
}
