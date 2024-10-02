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

    private GameObject _furniture;
    private Furniture _furnitureBehaviour;
    private GameObject lastHitObject = null;
    private Vector3 _startSpawnPos;
    private Quaternion _startSpawnRot;
    private Dictionary<Renderer, Material[]> originalMaterials = new Dictionary<Renderer, Material[]>();

    public static FurniturePlacement Instance { get; private set; }

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

            SaveOriginalMaterials(_furniture);

            _furnitureBehaviour = _furniture.GetComponent<Furniture>();
        }
    }

    private void FixedUpdate()
    {
        Ray rightRay = new Ray(rightHand.position, rightHand.forward);

        if (Physics.Raycast(rightRay, out RaycastHit rightHit, 100.0f))
        {
            displayText.text = LayerMask.LayerToName(rightHit.collider.gameObject.layer);

            if (_furnitureBehaviour != null && ((1 << rightHit.collider.gameObject.layer) & _furnitureBehaviour.layer.value) != 0)
            {
                _furnitureBehaviour.FollowRayHit((rightHit.point, rightHit.normal, true));
                _furnitureBehaviour.HandleRotation();

                if (CheckTriggerInput() && _furnitureBehaviour.isPlaceble)
                {
                    TogglePlacement();
                }
            }
            else if (_furniture != null)
            {
                _furniture.GetComponent<Rigidbody>().velocity = Vector3.zero;
            }

            if (rightHit.collider.gameObject.layer == 8)
            {
                if (lastHitObject != rightHit.collider.gameObject)
                {
                    DisableOutline(lastHitObject);
                    EnableOutline(rightHit.collider.gameObject);
                    lastHitObject = rightHit.collider.gameObject;
                }

                if (CheckBInput())
                {
                    Debug.Log("Deleted " + rightHit.collider.gameObject.name);
                    DeleteFurniture(rightHit.collider.gameObject);
                }
            }
            else
            {
                DisableOutline(lastHitObject);
                lastHitObject = null;
            }
        }
        else
        {
            DisableOutline(lastHitObject);
            lastHitObject = null;
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

    private void TogglePlacement()
    {
        isPrefabSelected = false;

        // Revert preview materials to original materials
        RestoreOriginalMaterials(_furniture);

        _furniture.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
        // Add Outline component to the placed furniture
        var outline = _furniture.AddComponent<Outline>();
        outline.OutlineMode = Outline.Mode.OutlineVisible;
        outline.OutlineColor = Color.yellow;
        outline.OutlineWidth = 3f;
        outline.enabled = false;

        ToggleCustomizeMenu toggleCustomizeMenu = _furniture.GetComponent<ToggleCustomizeMenu>();
        toggleCustomizeMenu.outline = outline;
       
        // Set tag and layer
        _furniture.tag = "Furniture";
        _furniture.layer = 8;

        // Remove the Furniture component
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

    private void SaveOriginalMaterials(GameObject furniturePreview)
    {
        originalMaterials.Clear();

        Renderer[] renderers = furniturePreview.GetComponentsInChildren<Renderer>();

        foreach (Renderer rend in renderers)
        {
            originalMaterials[rend] = rend.materials;

            Material[] previewMaterials = new Material[rend.materials.Length];
            for (int i = 0; i < previewMaterials.Length; i++)
            {
                previewMaterials[i] = previewMaterial;
            }
            rend.materials = previewMaterials;
        }
    }

    private void RestoreOriginalMaterials(GameObject furniture)
    {
        foreach (var kvp in originalMaterials)
        {
            if (kvp.Key != null)
            {
                kvp.Key.materials = kvp.Value;
            }
        }
        originalMaterials.Clear();
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
