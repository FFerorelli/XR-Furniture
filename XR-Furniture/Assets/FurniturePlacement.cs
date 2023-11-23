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

    [SerializeField] private float _speed = 2.5f;
    [SerializeField] private float _rotationSpeed = 90f;

    private Rigidbody _previewRB;

    private OVRInput.Controller _activeController = OVRInput.Controller.RTouch;

    private GameObject _furniture;
    private GameObject _furniturePreview;

    private bool _isPlaced;
    private float prefabHeight;
    private Vector3 _offset;
    private Vector3 _startSpawnPos;

    private (Vector3 point, Vector3 normal, bool hit) _leftHandHit;
    private (Vector3 point, Vector3 normal, bool hit) _rightHandHit;


    // Start is called before the first frame update
    void Start()
    {
        prefabHeight = (furniturePrefab.transform.localScale.y) / 2;
        _offset = new Vector3(0, prefabHeight, 0);

        _startSpawnPos = new Vector3(transform.position.x, _offset.y, transform.position.z);
        _furniturePreview = Instantiate(furniturePreviewPrefab, _startSpawnPos, transform.rotation);
       // _furniture = Instantiate(furniturePrefab, _startSpawnPos, _furniturePreview.transform.rotation);

        _previewRB = _furniturePreview.GetComponent<Rigidbody>();

        _furniture.SetActive(false);

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

        _leftHandHit = (leftHit.point, leftHit.normal, leftRayGround);
        _rightHandHit = (rightHit.point, rightHit.normal, rightRayGround);
        var activeRayGround = _activeController == OVRInput.Controller.LTouch ? _leftHandHit : _rightHandHit;


        if (activeRayGround.hit)
        {
            activeRayGround.point.y = 0;
            // update the position of the preview to match the raycast.
            FollowRayHit(activeRayGround);

            // rotate the preview with the thumbsticks 
            HandleRotation();

            if (CheckTriggerInput()) TogglePlacement(activeRayGround.point + _offset/*, activeRayGround.normal*/);
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

    private void FollowRayHit((Vector3 point, Vector3 normal, bool hit) ray)
    {
        Debug.Log(ray.point);
        //ray.point.y = 0;
        var previewPos = _furniturePreview.transform.position;
        var targetPos = ray.point + _offset;
        Vector3 direction = targetPos - previewPos;
        float distance = direction.magnitude;
        float step = distance * Time.fixedDeltaTime * _speed;
        _previewRB.MovePosition(previewPos + direction.normalized * step);

        // Stop moving when close to the hit point
        if (distance < 0.1f) _previewRB.velocity = Vector3.zero;
    }

    private void TogglePlacement(Vector3 point/*, Vector3 normal*/)
    {
       // if (_isPlaced)
       // {
           // _furniture.SetActive(false);
           // _furniturePreview.SetActive(true);
            //pickUpSFX.PlaySfxAtPosition(point);

          //  _isPlaced = false;
      //  }
       // else
       // {

           // _furniture.SetActive(true);
           _furniture = Instantiate(furniturePrefab, _furniturePreview.transform.position, _furniturePreview.transform.rotation);
           // _furniture.transform.position = _furniturePreview.transform.position;
           // _furniture.transform.up = normal;
           // _furniture.transform.rotation = _furniturePreview.transform.rotation;

           // _furniturePreview.SetActive(true);
           //placeDownSFX.PlaySfxAtPosition(point);

          //  _isPlaced = true;
       // }
    }
}
