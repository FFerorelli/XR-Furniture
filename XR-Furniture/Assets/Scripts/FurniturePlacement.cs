using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

public class FurniturePlacement : MonoBehaviour
{


    [SerializeField] private GameObject furniturePrefab;
    [SerializeField] private GameObject furniturePreviewPrefab;

    private LayerMask mask;

    [SerializeField] public Transform leftHand;
    [SerializeField] public Transform rightHand;


    [SerializeField] private float _rotationSpeed = 90f;


    private OVRInput.Controller _activeController = OVRInput.Controller.RTouch;

    private GameObject _furniture;
    private GameObject _furniturePreview;
    private Furniture _furnitureBehaviour;

    private float prefabHeight;
    private Vector3 _offset;
    private Vector3 _startSpawnPos;

    private (Vector3 point, bool hit) _leftHandHit;
    private (Vector3 point, bool hit) _rightHandHit;

    public (Vector3 point, bool hit) activeRay;

    // Start is called before the first frame update
    void Start()
    {
        prefabHeight = (furniturePrefab.transform.localScale.y) / 2;
        _offset = new Vector3(0, prefabHeight, 0);

        _startSpawnPos = new Vector3(transform.position.x, _offset.y, transform.position.z);
        _furniturePreview = Instantiate(furniturePreviewPrefab, _startSpawnPos, transform.rotation);

        _furnitureBehaviour = _furniturePreview.GetComponent<Furniture>();

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        mask = _furnitureBehaviour.layer;
       
        var leftRay = new Ray(leftHand.position, leftHand.forward);
        var rightRay = new Ray(rightHand.position, rightHand.forward);

        var leftRayGround = Physics.Raycast(leftRay, out var leftHit, 100.0f, mask);
        var rightRayGround = Physics.Raycast(rightRay, out var rightHit, 100.0f, mask);


        _leftHandHit = (leftHit.point, leftRayGround);
        _rightHandHit = (rightHit.point, rightRayGround);
        var activeRay = _activeController == OVRInput.Controller.LTouch ? _leftHandHit : _rightHandHit;


        if (activeRay.hit)
        {
            //// update the position of the preview to match the raycast.
            _furnitureBehaviour.FollowRayHit(activeRay);

            // rotate the preview with the thumbsticks 
             HandleRotation();

            if (CheckTriggerInput()) TogglePlacement();
        }
    }


    private void HandleRotation()
    {
        var thumbStick = OVRInput.Axis2D.PrimaryThumbstick;

        //if (OVRInput.Get(thumbStick, OVRInput.Controller.LTouch) != Vector2.zero)
        //{
        //    _activeController = OVRInput.Controller.LTouch;                              
        //}
        //else if (OVRInput.Get(thumbStick, OVRInput.Controller.RTouch) != Vector2.zero)
        //{
        //    _activeController = OVRInput.Controller.RTouch;
        //}

        Vector2 thumbStickPos = OVRInput.Get(thumbStick, _activeController);

        if (thumbStickPos != Vector2.zero)
        {
           // Debug.Log(thumbStickPos.x);
            var previewTransform = _furniturePreview.transform;
            float rotateAmount = -thumbStickPos.x * _rotationSpeed * Time.fixedDeltaTime;
            previewTransform.Rotate(Vector3.up, rotateAmount);
        }
    }

    private bool CheckTriggerInput()
    {
        var togglePlacement = false;
        const OVRInput.Button buttonMask = OVRInput.Button.PrimaryIndexTrigger /*| OVRInput.Button.PrimaryHandTrigger*/;

        if (OVRInput.GetDown(buttonMask, OVRInput.Controller.LTouch))
        {
            _activeController = OVRInput.Controller.LTouch;
            togglePlacement = true;
        }
        else if (OVRInput.GetDown(buttonMask, OVRInput.Controller.RTouch))
        {
            _activeController = OVRInput.Controller.RTouch;
            togglePlacement = true;
        }

        return togglePlacement;
    }



    private void TogglePlacement()
    {
           _furniture = Instantiate(furniturePrefab, _furniturePreview.transform.position, _furniturePreview.transform.rotation);
    }
}
