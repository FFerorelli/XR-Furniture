using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FurniturePlacement : MonoBehaviour
{


    [SerializeField] private GameObject furniturePrefab;
    [SerializeField] private GameObject furniturePreviewPrefab;
    [SerializeField] private LayerMask meshLayerMask;

    [SerializeField] private Transform leftHand;
    [SerializeField] private Transform rightHand;

    private OVRInput.Controller _activeController = OVRInput.Controller.RTouch;

    private GameObject _furniture;
    private GameObject _furniturePreview;

    private bool _isPlaced;
    private float prefabHeight;
    private Vector3 _offset;

    private (Vector3 point, Vector3 normal, bool hit) _leftHandHit;
    private (Vector3 point, Vector3 normal, bool hit) _rightHandHit;


    // Start is called before the first frame update
    void Start()
    {
        _furniturePreview = Instantiate(furniturePreviewPrefab, transform);
        _furniture = Instantiate(furniturePrefab, transform);
        _furniture.SetActive(false);

        prefabHeight = (furniturePrefab.transform.localScale.y) / 2;
        _offset = new Vector3(0, prefabHeight, 0);
    }

    // Update is called once per frame
    void Update()
    {
        var togglePlacement = false;
        const OVRInput.Button buttonMask = OVRInput.Button.PrimaryIndexTrigger | OVRInput.Button.PrimaryHandTrigger;

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

        var leftRay = new Ray(leftHand.position, leftHand.forward);
        var rightRay = new Ray(rightHand.position, rightHand.forward);

        var leftRaySuccess = Physics.Raycast(leftRay, out var leftHit, 100.0f, meshLayerMask);
        var rightRaySuccess = Physics.Raycast(rightRay, out var rightHit, 100.0f, meshLayerMask);

        _leftHandHit = (leftHit.point, leftHit.normal, leftRaySuccess);
        _rightHandHit = (rightHit.point, rightHit.normal, rightRaySuccess);
        var active = _activeController == OVRInput.Controller.LTouch ? _leftHandHit : _rightHandHit;

        if (togglePlacement && active.hit) TogglePlacement(active.point + _offset, active.normal);

        if (!_isPlaced && active.hit)
        {
            // update the position of the preview to match the raycast.
            var furniturePreviewTransform = _furniturePreview.transform;

            furniturePreviewTransform.position = active.point + _offset;
            furniturePreviewTransform.up = active.normal;
        }
    }

    private void TogglePlacement(Vector3 point, Vector3 normal)
    {
        if (_isPlaced)
        {
            _furniture.SetActive(false);
            _furniturePreview.SetActive(true);
            //pickUpSFX.PlaySfxAtPosition(point);

            _isPlaced = false;
        }
        else
        {
            var blasterTransform = _furniture.transform;
            blasterTransform.position = point;
            blasterTransform.up = normal;

            _furniture.SetActive(true);
            _furniturePreview.SetActive(false);
           //placeDownSFX.PlaySfxAtPosition(point);

            _isPlaced = true;
        }
    }
}
