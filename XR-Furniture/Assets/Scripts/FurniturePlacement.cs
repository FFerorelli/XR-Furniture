using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;


public class FurniturePlacement : MonoBehaviour
{

    [SerializeField] public Transform leftHand;
    [SerializeField] public Transform rightHand;

   // [SerializeField] private GameObject furniturePrefab;
    [SerializeField] private GameObject furniturePrefab;

    [SerializeField] private TextMeshProUGUI displayText = null;
    [SerializeField] private float _rotationSpeed = 90f;

    private Material currentMaterial;
    private Color greenColor;
    private Color originalColor;

    private LayerMask mask;
    private GameObject spawnedPrefab;
    private GameObject _furniturePreview;
    private Furniture _furnitureBehaviour;
    private string currentLayerName;
    private int currentLayerIndex;
    private float prefabHeight;
    private Vector3 _offset;
    private Vector3 _startSpawnPos;

    private (Vector3 point, Vector3 normal, bool hit) _rightHandHit;



    // Start is called before the first frame update
    void Start()
    {

        prefabHeight = (furniturePrefab.transform.localScale.y) / 2;
        _offset = new Vector3(0, prefabHeight, 0);
        _startSpawnPos = new Vector3(transform.position.x, _offset.y, transform.position.z);

        _furniturePreview = Instantiate(furniturePrefab, _startSpawnPos, transform.rotation);

        currentMaterial = _furniturePreview.GetComponent<MeshRenderer>().material;
        originalColor = currentMaterial.color;
        _furniturePreview.GetComponent<MeshRenderer>().material.color = Color.green;

        _furnitureBehaviour = _furniturePreview.GetComponent<Furniture>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        mask = _furnitureBehaviour.layer;

        var rightRay = new Ray(rightHand.position, rightHand.forward);
        var rightRayCast = Physics.Raycast(rightRay, out var rightHit, 100.0f);
        _rightHandHit = (rightHit.point, rightHit.normal, rightRayCast);

        if (_rightHandHit.hit)
        {
            currentLayerIndex = rightHit.collider.gameObject.layer;
            currentLayerName = LayerMask.LayerToName(currentLayerIndex);
            displayText.text = currentLayerName;
        }

        // Check if the hitLayer is included in the specified layerMaskToCheck
        if ((_rightHandHit.hit && (mask.value & (1<< currentLayerIndex)) > 0))
        {
            // update the position of the preview to match the raycast.
            _furnitureBehaviour.FollowRayHit(_rightHandHit);

            // rotate the preview with the thumbsticks 
             HandleRotation();

            if (CheckTriggerInput() && _furnitureBehaviour.isPlaceble) TogglePlacement();
        }
    }


    private void HandleRotation()
    {
        var thumbStick = OVRInput.Axis2D.PrimaryThumbstick;

        Vector2 thumbStickPos = OVRInput.Get(thumbStick, OVRInput.Controller.RTouch);

        if (thumbStickPos != Vector2.zero)
        {
            var previewTransform = _furniturePreview.transform;
            float rotateAmount = -thumbStickPos.x * _rotationSpeed * Time.fixedDeltaTime;
            previewTransform.Rotate(Vector3.up, rotateAmount, Space.Self);
        }
    }

    private bool CheckTriggerInput()
    {
        var togglePlacement = false;

        const OVRInput.Button buttonMask = OVRInput.Button.PrimaryIndexTrigger;

        if (OVRInput.GetDown(buttonMask, OVRInput.Controller.RTouch)) togglePlacement = true;

        return togglePlacement;
    }



    private void TogglePlacement()
    {

        spawnedPrefab = Instantiate(furniturePrefab, _furniturePreview.transform.position, _furniturePreview.transform.rotation);
        spawnedPrefab.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
       
        Furniture furnitureScript = spawnedPrefab.GetComponent<Furniture>();
        Destroy(furnitureScript);
        spawnedPrefab.GetComponent<MeshRenderer>().material.color = originalColor;
        spawnedPrefab.tag = "Furniture";
        spawnedPrefab.layer = 8;
        Debug.Log(LayerMask.LayerToName(spawnedPrefab.layer));

    }
}
