using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;


public class FurniturePlacement : MonoBehaviour
{


    [SerializeField] private GameObject furniturePrefab;
    [SerializeField] private GameObject furniturePreviewPrefab;

    private LayerMask mask;

    [SerializeField] public Transform leftHand;
    [SerializeField] public Transform rightHand;

    [SerializeField]
    private TextMeshProUGUI displayText = null;

    [SerializeField] private float _rotationSpeed = 90f;


   // private OVRInput.Controller _activeController = OVRInput.Controller.RTouch;

    private GameObject _furniture;
    private GameObject _furniturePreview;
    private Furniture _furnitureBehaviour;
    private string currentLayerName;
    private int currentLayerIndex;
    private float prefabHeight;
    private Vector3 _offset;
    private Vector3 _startSpawnPos;

    private (Vector3 point, Vector3 normal, bool hit) _leftHandHit;
    private (Vector3 point, Vector3 normal, bool hit) _rightHandHit;

    public (Vector3 point, Vector3 normal, bool hit) activeRay;

    //private LineRenderer rightLineRenderer;
    //private LineRenderer leftLineRenderer;


    //[Header("Line Render Settings")]
    //[SerializeField]
    //private float lineWidth = 0.01f;

    //[SerializeField]
    //private float lineMaxLength = 50f;

    // Start is called before the first frame update
    void Start()
    {
        prefabHeight = (furniturePrefab.transform.localScale.y) / 2;
        _offset = new Vector3(0, prefabHeight, 0);

        _startSpawnPos = new Vector3(transform.position.x, _offset.y, transform.position.z);
        _furniturePreview = Instantiate(furniturePreviewPrefab, _startSpawnPos, transform.rotation);

        _furnitureBehaviour = _furniturePreview.GetComponent<Furniture>();

    }
    private void Awake()
    {
        //rightLineRenderer = gameObject.GetComponent<LineRenderer>();
        //leftLineRenderer = gameObject.GetComponent<LineRenderer>();
        //rightLineRenderer.widthMultiplier = lineWidth;
        //leftLineRenderer.widthMultiplier = lineWidth;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        mask = _furnitureBehaviour.layer;
       
      //  var leftRay = new Ray(leftHand.position, leftHand.forward);
        var rightRay = new Ray(rightHand.position, rightHand.forward);

        

      //  var leftRayGroundHit = Physics.Raycast(leftRay, out var leftHit, 100.0f, mask);
        var rightRayCast = Physics.Raycast(rightRay, out var rightHit, 100.0f/*, mask*/);
        _rightHandHit = (rightHit.point, rightHit.normal, rightRayCast);

        if (_rightHandHit.hit)
        {
            currentLayerIndex = rightHit.collider.gameObject.layer;
            currentLayerName = LayerMask.LayerToName(currentLayerIndex);
            displayText.text = currentLayerName;
        }



       // _leftHandHit = (leftHit.point, leftHit.normal, leftRayGroundHit);
        // var activeRay = _activeController == OVRInput.Controller.LTouch ? _leftHandHit : _rightHandHit;

        //rightLineRenderer.SetPosition(0, leftHand.position);
        //rightLineRenderer.SetPosition(1, leftHit.point);

        //leftLineRenderer.SetPosition(0, rightHand.position);
        //leftLineRenderer.SetPosition(1, rightHit.point);


        // Check if the hitLayer is included in the specified layerMaskToCheck
        if ((_rightHandHit.hit && (mask.value & (1<< currentLayerIndex)) > 0))
        {

            //// update the position of the preview to match the raycast.
            _furnitureBehaviour.FollowRayHit(_rightHandHit);

            // rotate the preview with the thumbsticks 
             HandleRotation();
            //Debug.Log(_furnitureBehaviour.isPlaceble);
            if (CheckTriggerInput() && _furnitureBehaviour.isPlaceble) TogglePlacement();
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

        Vector2 thumbStickPos = OVRInput.Get(thumbStick, OVRInput.Controller.RTouch);

        if (thumbStickPos != Vector2.zero)
        {
           // Debug.Log(thumbStickPos.x);
            var previewTransform = _furniturePreview.transform;
            float rotateAmount = -thumbStickPos.x * _rotationSpeed * Time.fixedDeltaTime;
            previewTransform.Rotate(Vector3.up, rotateAmount, Space.Self);
        }
    }

    private bool CheckTriggerInput()
    {
        var togglePlacement = false;
        const OVRInput.Button buttonMask = OVRInput.Button.PrimaryIndexTrigger /*| OVRInput.Button.PrimaryHandTrigger*/;

        //if (OVRInput.GetDown(buttonMask, OVRInput.Controller.LTouch))
        //{
           // _activeController = OVRInput.Controller.LTouch;
           if (OVRInput.GetDown(buttonMask, OVRInput.Controller.RTouch)) togglePlacement = true;
            // togglePlacement = true;
            // }
            //else if (OVRInput.GetDown(buttonMask, OVRInput.Controller.RTouch))
            //{
            // _activeController = OVRInput.Controller.RTouch;
            // togglePlacement = true;
            // }

            return togglePlacement;
    }



    private void TogglePlacement()
    {
           _furniture = Instantiate(furniturePrefab, _furniturePreview.transform.position, _furniturePreview.transform.rotation);
    }
}
