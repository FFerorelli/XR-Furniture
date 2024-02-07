using TMPro;
using UnityEngine;

public class FurniturePlacement : MonoBehaviour
{
    public bool isPrefabSelected = false;
    [SerializeField] public Transform leftHand;
    [SerializeField] public Transform rightHand;
    [SerializeField] private Transform _parentTransform;
    [SerializeField] private Transform _UITransform;
    [SerializeField] private TextMeshProUGUI displayText = null;
    [SerializeField] private Material previewMaterial;

    private GameObject furniturePrefab;
    private Material originalMaterial;
    private GameObject spawnedPrefab;
    private GameObject _furniturePreview;
    private GameObject lastHitObject = null;
    private Furniture _furnitureBehaviour;
    private Vector3 _startSpawnPos;
    private Quaternion _startSpawnRot;
    private Outline outline;

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
        if (_furniturePreview != null)
        {
            Destroy(_furniturePreview);
        }

        if (prefab != null)
        {
            isPrefabSelected = true;
            furniturePrefab = prefab;

            float uiRotation = _UITransform.rotation.eulerAngles.y;           
            _startSpawnPos = transform.position;
            _startSpawnRot = Quaternion.Euler(0, uiRotation, 0);

            _furniturePreview = Instantiate(furniturePrefab, _startSpawnPos, _startSpawnRot);


            // _furniturePreview.transform.LookAt(transform.position);
            _furniturePreview.transform.Rotate(0, 180, 0);

            var meshRenderer = _furniturePreview.GetComponent<MeshRenderer>();
            originalMaterial = meshRenderer.material;
            // originalColor = currentMaterial.color;
            meshRenderer.material = previewMaterial;

            _furnitureBehaviour = _furniturePreview.GetComponent<Furniture>(); 
        }
    }

    // FixedUpdate is called once per frame, but with a fixed time interval
    void FixedUpdate()
    {
        // Create a ray from the right hand's position, pointing forward
        Ray rightRay = new Ray(rightHand.position, rightHand.forward);

        // Perform a raycast using the rightRay, store the result in rightHit and check if it hit something
        if (Physics.Raycast(rightRay, out RaycastHit rightHit, 100.0f))
        {
            // Display the name of the layer of the hit object in the UI
            displayText.text = LayerMask.LayerToName(rightHit.collider.gameObject.layer);

            // If a furniture is selected from the UI and the hit object's layer is included in the furniture's layer mask
            if (_furnitureBehaviour != null && ((1 << rightHit.collider.gameObject.layer) & _furnitureBehaviour.layer.value) != 0)
            {
                // Make the furniture follow the ray hit
                _furnitureBehaviour.FollowRayHit((rightHit.point, rightHit.normal, true));
                // Handle the rotation of the furniture
                _furnitureBehaviour.HandleRotation();

                // If the trigger input is pressed and the furniture is placeable
                if (CheckTriggerInput() && _furnitureBehaviour.isPlaceble)
                {
                    // Toggle the placement of the furniture
                    TogglePlacement();
                }
            }
            else if (_furniturePreview != null) // If the hit object's layer is not included in the furniture's layer mask
            {
                // Stop the furniture's movement
                _furniturePreview.GetComponent<Rigidbody>().velocity = Vector3.zero;
            }
            // if the layer you are pointing to is furniture and you press B it will delete the furniture
            if (rightHit.collider.gameObject.layer == 8)
            {
                // If the ray hit a different object than last time
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

                if (CheckBInput())
                {
                    Debug.Log("Deleted " + rightHit.collider.gameObject.name);
                    DeleteFurniture(rightHit.collider.gameObject);
                }
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

    private void TogglePlacement()
    {
        isPrefabSelected = false;
       
        spawnedPrefab = Instantiate(furniturePrefab, _furniturePreview.transform.position, _furniturePreview.transform.rotation);
        spawnedPrefab.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;

        var outline = spawnedPrefab.AddComponent<Outline>();
        outline.OutlineMode = Outline.Mode.OutlineAll;
        outline.OutlineColor = Color.yellow;
        outline.OutlineWidth = 5f;
        outline.enabled = false;

        var meshRenderer = spawnedPrefab.GetComponent<MeshRenderer>();
        meshRenderer.material = originalMaterial;

        spawnedPrefab.tag = "Furniture";
        spawnedPrefab.layer = 8;

        Destroy(spawnedPrefab.GetComponent<Furniture>());

        SetNewFurniture(null);
    }
    private void DeleteFurniture(GameObject objectToDelete)
    {
        Destroy(objectToDelete);
    }
}