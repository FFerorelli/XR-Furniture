using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleLight : MonoBehaviour
{
    private Outline outline;
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
            if (outline.enabled == true && CheckAInput())
            {
                ToggleChildLights();
                ToggleEmissionOnChild();
            }
        }
    }
    private bool CheckAInput()
    {
        return OVRInput.GetDown(OVRInput.Button.One, OVRInput.Controller.RTouch);
    }
    private void ToggleChildLights()
    {
        // Get all Light components in children of this GameObject.
        Light[] childLights = GetComponentsInChildren<Light>();

        // Loop through all found Light components.
        foreach (Light light in childLights)
        {
            // Toggle the light.
            light.enabled = !light.enabled;
        }
    }
    public void ToggleEmissionOnChild()
    {
        // Find the child GameObject by name.
        Transform bulbTransform = transform.Find("Bulb");

        if (bulbTransform != null)
        {
            // Get the Renderer component from the child.
            Renderer bulbRenderer = bulbTransform.GetComponent<Renderer>();

            if (bulbRenderer != null)
            {
                // Get the material of the Renderer.
                Material bulbMaterial = bulbRenderer.material;

                // Check if emission is currently enabled.
                bool isEmissionEnabled = bulbMaterial.IsKeywordEnabled("_EMISSION");

                // Toggle emission.
                if (isEmissionEnabled)
                {
                    bulbMaterial.DisableKeyword("_EMISSION");
                }
                else
                {
                    bulbMaterial.EnableKeyword("_EMISSION");
                }
            }
        }
        else
        {
            Debug.LogError("Child 'bulb' not found.");
        }
    }
}
