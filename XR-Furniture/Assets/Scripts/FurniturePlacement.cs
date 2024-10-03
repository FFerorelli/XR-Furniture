using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class FurniturePlacement : MonoBehaviour
{
    public bool isPrefabSelected = false;
    [SerializeField] private Transform rightHand;
    [SerializeField] private Transform _UITransform;
    [SerializeField] private TextMeshProUGUI displayText = null;
    [SerializeField] private Material previewMaterial;
    [SerializeField] private GameObject furniturePrefab;
    [Range(0f, 1f)]
    private GameObject _furniture;
    private Furniture _furnitureBehaviour;
    private GameObject lastHitObject = null;
    private Vector3 _startSpawnPos;
    private Quaternion _startSpawnRot;

    public static FurniturePlacement Instance { get; private set; }

    private void Awake()
    {
        // Singleton pattern
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

    public void SetNewFurniture(GameObject prefab)
    {
        if (_furniture != null)
        {
            Destroy(_furniture);
        }

        if (prefab != null)
        {
            isPrefabSelected = true;
            furniturePrefab = prefab;

            float originalAssetOffset = -prefab.transform.rotation.eulerAngles.x;
            float uiRotation = _UITransform.rotation.eulerAngles.y;
            _startSpawnPos = transform.position;
            _startSpawnRot = Quaternion.Euler(originalAssetOffset, uiRotation, 0);

            _furniture = Instantiate(furniturePrefab, _startSpawnPos, _startSpawnRot);
            _furniture.transform.Rotate(0, 180, 0);

            _furnitureBehaviour = _furniture.GetComponent<Furniture>();
            if (_furnitureBehaviour == null)
            {
                Debug.LogError("Furniture script is missing on the prefab.");
                return;
            }

            // ** Initialize the furniture immediately to assign preview materials **
            _furnitureBehaviour.InitializeFurniture();

            // Start the mesh reduction asynchronously
            StartCoroutine(ReduceMeshAndContinue(_furniture));

            // Collider creation is handled after mesh reduction in the coroutine
        }
    }


    private IEnumerator ReduceMeshAndContinue(GameObject furniture)
    {
        // Get the simplification percentage from the SimplificationSettings component
        SimplificationSettings simplificationSettings = furniture.GetComponent<SimplificationSettings>();
        float simplificationPercentage = simplificationSettings != null ? simplificationSettings.simplificationPercentage : 0.5f;

        // Perform the mesh reduction asynchronously
        MeshReducerAsync meshReducer = new MeshReducerAsync(simplificationPercentage);
        yield return StartCoroutine(meshReducer.ReduceMeshAsync(furniture.transform));

        // ** After mesh reduction, create or update colliders **
        AutoBoxColliderForChildren colliderCreator = furniture.GetComponent<AutoBoxColliderForChildren>();
        if (colliderCreator != null)
        {
            colliderCreator.AddBoxCollider();
        }

        // If any other updates are needed after mesh reduction, add them here
    }


    private void FixedUpdate()
    {
        HandleRaycast();
        HandleInput();
    }

    private void HandleRaycast()
    {
        Ray rightRay = new Ray(rightHand.position, rightHand.forward);

        if (Physics.Raycast(rightRay, out RaycastHit rightHit, 100.0f))
        {
            displayText.text = LayerMask.LayerToName(rightHit.collider.gameObject.layer);

            if (_furnitureBehaviour != null && ((1 << rightHit.collider.gameObject.layer) & _furnitureBehaviour.layer.value) != 0)
            {
                _furnitureBehaviour.FollowRayHit((rightHit.point, rightHit.normal, true));
                _furnitureBehaviour.HandleRotation();
            }
            else if (_furniture != null)
            {
                _furniture.GetComponent<Rigidbody>().velocity = Vector3.zero;
            }

            HandleOutline(rightHit.collider.gameObject);
        }
        else
        {
            DisableOutline(lastHitObject);
            lastHitObject = null;
        }
    }

    private void HandleInput()
    {
        if (_furnitureBehaviour != null && CheckTriggerInput() && _furnitureBehaviour.isPlaceable)
        {
            PlaceFurniture();
        }

        if (lastHitObject != null && lastHitObject.layer == 8 && CheckBInput())
        {
            Debug.Log("Deleted " + lastHitObject.name);
            DeleteFurniture(lastHitObject);
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

    private void PlaceFurniture()
    {
        isPrefabSelected = false;

        // Notify the furniture that it has been placed
        _furnitureBehaviour.Place();

        // Add Outline component to the placed furniture
        var outline = _furniture.AddComponent<Outline>();
        outline.OutlineMode = Outline.Mode.OutlineVisible;
        outline.OutlineColor = Color.yellow;
        outline.OutlineWidth = 3f;
        outline.enabled = false;

        // Assign outline to ToggleCustomizeMenu if applicable
        ToggleCustomizeMenu toggleCustomizeMenu = _furniture.GetComponent<ToggleCustomizeMenu>();
        if (toggleCustomizeMenu != null)
        {
            toggleCustomizeMenu.outline = outline;
        }

        // Set tag and layer
        _furniture.tag = "Furniture";
        _furniture.layer = 8;

        // Remove the Furniture component to prevent further manipulation
        Destroy(_furniture.GetComponent<Furniture>());

        // Clear references
        _furniture = null;
        _furnitureBehaviour = null;

        SetNewFurniture(null);
    }

    private void DeleteFurniture(GameObject objectToDelete)
    {
        Destroy(objectToDelete);
    }

    private void HandleOutline(GameObject hitObject)
    {
        if (hitObject.layer == 8)
        {
            if (lastHitObject != hitObject)
            {
                DisableOutline(lastHitObject);
                EnableOutline(hitObject);
                lastHitObject = hitObject;
            }
        }
        else
        {
            DisableOutline(lastHitObject);
            lastHitObject = null;
        }
    }

    private void EnableOutline(GameObject obj)
    {
        if (obj != null)
        {
            var outline = obj.GetComponent<Outline>();
            if (outline != null)
            {
                outline.enabled = true;
            }
        }
    }

    private void DisableOutline(GameObject obj)
    {
        if (obj != null)
        {
            var outline = obj.GetComponent<Outline>();
            if (outline != null)
            {
                outline.enabled = false;
            }
        }
    }
}
