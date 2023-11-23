using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FurniturePlacement : MonoBehaviour
{


    [SerializeField] private GameObject furniturePrefab;
    [SerializeField] private GameObject furniturePreviewPrefab;

    [SerializeField] private LayerMask groundLayerMask;

    [SerializeField] private Transform leftHand;
    [SerializeField] private Transform rightHand;


    [SerializeField] private float _rotationSpeed = 90f;


    private OVRInput.Controller _activeController = OVRInput.Controller.RTouch;

    private GameObject _furniture;
    private GameObject _furniturePreview;
    private IFurniture _furnitureBehaviour;

    private float prefabHeight;
    private Vector3 _offset;
    private Vector3 _startSpawnPos;

    private (Vector3 point, bool hit) _leftHandHit;
    private (Vector3 point, bool hit) _rightHandHit;


    // Start is called before the first frame update
    void Start()
    {
        prefabHeight = (furniturePrefab.transform.localScale.y) / 2;
        _offset = new Vector3(0, prefabHeight, 0);

        _startSpawnPos = new Vector3(transform.position.x, _offset.y, transform.position.z);
        _furniturePreview = Instantiate(furniturePreviewPrefab, _startSpawnPos, transform.rotation);
        // _furniture = Instantiate(furniturePrefab, _startSpawnPos, _furniturePreview.transform.rotation);

        // _previewRB = _furniturePreview.GetComponent<Rigidbody>();
        _furnitureBehaviour = _furniturePreview.GetComponent<FloorFurniture>();
        //_furniture.SetActive(false);

    }

    // Update is called once per frame
    void FixedUpdate()
    {

        var leftRay = new Ray(leftHand.position, leftHand.forward);
        var rightRay = new Ray(rightHand.position, rightHand.forward);

        var leftRayGround = Physics.Raycast(leftRay, out var leftHit, 100.0f, groundLayerMask);
        var rightRayGround = Physics.Raycast(rightRay, out var rightHit, 100.0f, groundLayerMask);

        //var leftRayFurniture = Physics.Raycast(leftRay, out var leftFurnitureHit, 100.0f, furnitureLayerMask);
        //var rightRayFurniture = Physics.Raycast(rightRay, out var rightFurnitureHit, 100.0f, furnitureLayerMask);

        _leftHandHit = (leftHit.point, leftRayGround);
        _rightHandHit = (rightHit.point, rightRayGround);
        var activeRayGround = _activeController == OVRInput.Controller.LTouch ? _leftHandHit : _rightHandHit;


        if (activeRayGround.hit)
        {
            activeRayGround.point.y = 0;
            // update the position of the preview to match the raycast.
            _furnitureBehaviour.FollowRayHit(activeRayGround);

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
