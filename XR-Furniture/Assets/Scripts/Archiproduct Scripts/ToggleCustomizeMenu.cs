using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleCustomizeMenu : MonoBehaviour
{
    private Outline outline;

    [SerializeField] private GameObject menuPrefab;
    private Vector3 CUSTOMIZEMENUSCALE = new Vector3(50, 50, 50);
    private float animationDuration = 0.3f;
    private GameObject instantiatedMenu;
    private bool isMenuActive = false;

    // Start is called before the first frame update
    void Start()
    {
        outline = GetComponent<Outline>();
    }

    // Update is called once per frame
    void Update()
    {
        if (outline != null)
        {
            if (outline.enabled == true && CheckAInput() && !isAnimating)
            {

                Debug.Log("--------------Ainput----------------");
                ToggleMenu();
            }
        }

    }
    private bool isAnimating = false;

    private void ToggleMenu()
    {
        if (isAnimating) return; // Ignore input while animating

        Debug.Log("ToggleCustomizeMenu called.");
        if (instantiatedMenu == null)
        {
            Transform menuTransform = transform.Find("MenuTransform");
            instantiatedMenu = Instantiate(menuPrefab, menuTransform);

            // Set the initial scale to zero for a scaling animation
            instantiatedMenu.transform.localScale = Vector3.zero;

            // Optionally, set initial opacity to zero for a fade-in effect
            CanvasGroup canvasGroup = instantiatedMenu.AddComponent<CanvasGroup>();
            canvasGroup.alpha = 0f;

            // Start the animations and set isAnimating to true
            StartCoroutine(AnimateScale(instantiatedMenu.transform, Vector3.zero, CUSTOMIZEMENUSCALE, animationDuration));
            StartCoroutine(AnimateAlpha(canvasGroup, 0f, 1f, animationDuration));
            StartCoroutine(SetAnimatingFlag(animationDuration)); // Set animating flag for 0.5s

            isMenuActive = true;
        }
        else
        {
            CanvasGroup canvasGroup = instantiatedMenu.GetComponent<CanvasGroup>();

            if (!isMenuActive)
            {
                // Handle menu activation (opening)
                instantiatedMenu.SetActive(true);
                instantiatedMenu.transform.localScale = Vector3.zero;
                canvasGroup.alpha = 0f;

                // Start the animations and set isAnimating to true
                StartCoroutine(AnimateScale(instantiatedMenu.transform, Vector3.zero, CUSTOMIZEMENUSCALE, animationDuration));
                StartCoroutine(AnimateAlpha(canvasGroup, 0f, 1f, animationDuration));
                StartCoroutine(SetAnimatingFlag(animationDuration)); // Set animating flag for 0.5s
            }
            else
            {
                // Handle menu deactivation (closing)
                StartCoroutine(CloseMenuWithAnimation(instantiatedMenu, canvasGroup));
            }
            isMenuActive = !isMenuActive;
        }
    }

    private IEnumerator CloseMenuWithAnimation(GameObject menu, CanvasGroup canvasGroup)
    {
        isAnimating = true;

        // Start the scaling animation
        yield return StartCoroutine(AnimateScale(menu.transform, CUSTOMIZEMENUSCALE, Vector3.zero, animationDuration));

        // Start the fade-out animation
        yield return StartCoroutine(AnimateAlpha(canvasGroup, 1f, 0f, animationDuration));

        // Deactivate the menu after the animation completes
        menu.SetActive(false);

        isAnimating = false;
    }

    private IEnumerator SetAnimatingFlag(float duration)
    {
        isAnimating = true;
        yield return new WaitForSeconds(duration);
        isAnimating = false;
    }



    private bool CheckAInput()
    {
        return OVRInput.GetDown(OVRInput.Button.One, OVRInput.Controller.RTouch);
    }

    private IEnumerator AnimateScale(Transform targetTransform, Vector3 startScale, Vector3 endScale, float duration)
    {
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            // Calculate the interpolation factor
            float t = elapsedTime / duration;

            // Interpolate the scale
            targetTransform.localScale = Vector3.Lerp(startScale, endScale, t);

            // Increment the elapsed time
            elapsedTime += Time.deltaTime;

            // Wait for the next frame
            yield return null;
        }

        // Ensure the final scale is set
        targetTransform.localScale = endScale;
    }

    private IEnumerator AnimateAlpha(CanvasGroup canvasGroup, float startAlpha, float endAlpha, float duration)
    {
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            float t = elapsedTime / duration;

            // Interpolate the alpha value
            canvasGroup.alpha = Mathf.Lerp(startAlpha, endAlpha, t);

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        canvasGroup.alpha = endAlpha;
    }




}
