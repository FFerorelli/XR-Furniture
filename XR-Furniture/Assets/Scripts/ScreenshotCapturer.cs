using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenshotCapturer : MonoBehaviour
{
    public List<GameObject> prefabs;
    public Transform prefabPos;
    public string screenshotDirectory = "Assets/Resources/Thumbnails";
    public float maxObjectSize = 2.0f; // Adjust this value to set the maximum size of objects in the screenshots
    public float minFieldOfView = 20.0f; // Adjust this value to set the minimum field of view for smaller objects

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

            // Set a fixed distance for the camera
            float fixedDistance = 10.0f; // Adjust this value based on your scene

            // Calculate the field of view to fill the screen with the object
            float fieldOfView = CalculateFieldOfView(bounds, fixedDistance, maxObjectSize, minFieldOfView, cameraAspect);

            // Set the calculated field of view
            cam.fieldOfView = fieldOfView;

            // Position the camera at the fixed distance
            cam.transform.position = bounds.center - cam.transform.forward * fixedDistance;

            ScreenCapture.CaptureScreenshot(screenshotDirectory + "/" + prefab.name + ".png");
            Debug.Log("Screenshot captured for " + prefab.name);

            Destroy(instance);

            yield return new WaitForSeconds(0.5f);
        }
        prefabPos.gameObject.SetActive(false);
        Debug.Log("All Screenshots captured");
    }

    private float CalculateFieldOfView(Bounds bounds, float distance, float maxObjectSize, float minFieldOfView, float cameraAspect)
    {

        float objectSize = Mathf.Max(bounds.size.x / cameraAspect, bounds.size.y);
        float normalizedSize = Mathf.Clamp01(objectSize / maxObjectSize);
        float fieldOfView = Mathf.Lerp(minFieldOfView, Camera.main.fieldOfView, normalizedSize);
        return fieldOfView;
    }
}
