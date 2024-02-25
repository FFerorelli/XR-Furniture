using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class RepaintMode : MonoBehaviour
{
    //public bool isPrefabSelected = false;
    [SerializeField] public Transform leftHand;
    [SerializeField] public Transform rightHand;
    [SerializeField] private TextMeshProUGUI displayText = null;
    private GameObject lastHitObject = null;
    private Outline outline;

    public static RepaintMode Instance { get; private set; }
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        // Create a ray from the right hand's position, pointing forward
        Ray rightRay = new Ray(rightHand.position, rightHand.forward);
        if (Physics.Raycast(rightRay, out RaycastHit rightHit, 100.0f))
        {
            displayText.text = LayerMask.LayerToName(rightHit.collider.gameObject.layer);

            if (rightHit.collider.gameObject.layer == 6 || rightHit.collider.gameObject.layer == 7)
            {
                Debug.Log(rightHit.collider.gameObject.name);
                if (lastHitObject != rightHit.collider.gameObject)
                {
                    // Disable the outline of the last hit object
                    if (lastHitObject != null)
                    {
                        var lastOutline = lastHitObject.GetComponent<Outline>();
                        if (lastOutline != null)
                        {
                            lastOutline.enabled = false;
                        }
                    }

                    // Enable the outline of the new hit object
                    var newOutline = rightHit.collider.gameObject.GetComponent<Outline>();
                    if (newOutline != null)
                    {
                        newOutline.enabled = true;
                    }

                    // Update the last hit object
                    lastHitObject = rightHit.collider.gameObject;
                }
                else
                {
                    // If the ray didn't hit a furniture object, disable the outline of the last hit object
                    if (lastHitObject != null)
                    {
                        var lastOutline = lastHitObject.GetComponent<Outline>();
                        if (lastOutline != null)
                        {
                            lastOutline.enabled = false;
                        }
                        lastHitObject = null;
                    }
                }
            }
        }
        else
        {
            // If the ray didn't hit anything, disable the outline of the last hit object
            if (lastHitObject != null)
            {
                var lastOutline = lastHitObject.GetComponent<Outline>();
                if (lastOutline != null)
                {
                    lastOutline.enabled = false;
                }
                lastHitObject = null;
            }
        }

    }

    private bool CheckTriggerInput()
    {
        return OVRInput.GetDown(OVRInput.Button.PrimaryIndexTrigger, OVRInput.Controller.RTouch);
    }
    private bool CheckBInput()
    {
        return OVRInput.GetDown(OVRInput.Button.Two, OVRInput.Controller.RTouch);
    }
}
