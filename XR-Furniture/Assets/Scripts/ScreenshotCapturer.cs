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
        Vector3 prefabPosition = prefabPos.position;
        Quaternion prefabRotation = prefabPos.rotation;

        //Destroy(prefabPos.gameObject);
        prefabPos.gameObject.SetActive(false);
        foreach (var prefab in prefabs)
        {
            // Instantiate the prefab at the prefabPos position
            GameObject instance = Instantiate(prefab, prefabPosition, prefabRotation);

            // Wait for the end of the frame to ensure the prefab has been rendered
            yield return new WaitForEndOfFrame();

            // Capture a screenshot and save it as an image file
            ScreenCapture.CaptureScreenshot(screenshotDirectory + prefab.name + ".png");

            // Destroy the prefab instance
            Destroy(instance);

            // Wait for a short delay to ensure the screenshot has been saved
            yield return new WaitForSeconds(0.5f);
        }
        prefabPos.gameObject.SetActive(false);
    }
}
