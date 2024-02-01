using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScrollViewBehaviour : MonoBehaviour
{
    [SerializeField] private ScrollRect scrollRect;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // Check if the thumbstick is being moved
        if (!FurniturePlacement.Instance.isPrefabSelected)
        {
            ScrollMenu(OVRInput.Controller.LTouch);
            ScrollMenu(OVRInput.Controller.RTouch);
        }
    }

    private void ScrollMenu(OVRInput.Controller controller)
    {
        if (OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick, controller).magnitude > 0)
        {
            // Get the thumbstick's vertical input
            float verticalInput = OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick, controller).y;

            // Scroll the ScrollView based on the thumbstick's vertical input
            scrollRect.verticalNormalizedPosition += verticalInput * Time.deltaTime;
        }
    }
}
