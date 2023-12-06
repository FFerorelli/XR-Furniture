using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;


public class FurniturePlacement : MonoBehaviour
{

    [SerializeField] public Transform leftHand;
    [SerializeField] public Transform rightHand;

    /*[SerializeField]*/ public GameObject furniturePrefab;

    [SerializeField] private TextMeshProUGUI displayText = null;
   // [SerializeField] private float _rotationSpeed = 90f;

    private Material currentMaterial;
   // private Color greenColor;
    private Color originalColor;

   // private LayerMask mask;
    private GameObject spawnedPrefab;
    private GameObject _furniturePreview;
    private Furniture _furnitureBehaviour;
   // private string currentLayerName;
   // private int currentLayerIndex;
    private float prefabHeight;
    private Vector3 _offset;
    private Vector3 _startSpawnPos;

    private (Vector3 point, Vector3 normal, bool hit) _rightHandHit;

    
    public static FurniturePlacement Instance { get; private set; }



    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // This will persist the GameManager object between scenes
        }
        else
        {
            Destroy(gameObject);
        }
    }


        // Start is called before the first frame update
    void Start()
    {
        //SetNewFurniture(furniturePrefab);
    }

    public void SetNewFurniture(GameObject prefab)
    {
        Destroy(_furniturePreview);
        furniturePrefab = prefab;
        //prefabHeight = (prefab.transform.localScale.y) / 2;
        //_offset = new Vector3(0, prefabHeight, 0);
        _startSpawnPos = new Vector3(transform.position.x, transform.position.y/*_offset.y*/, transform.position.z);

        _furniturePreview = Instantiate(furniturePrefab, _startSpawnPos, transform.rotation);

        currentMaterial = _furniturePreview.GetComponent<MeshRenderer>().material;
        originalColor = currentMaterial.color;
        _furniturePreview.GetComponent<MeshRenderer>().material.color = Color.green;

        _furnitureBehaviour = _furniturePreview.GetComponent<Furniture>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {

        Ray rightRay = new Ray(rightHand.position, rightHand.forward);

        bool rightRayCast = Physics.Raycast(rightRay, out RaycastHit rightHit, 100.0f);
        _rightHandHit = (rightHit.point, rightHit.normal, rightRayCast);

        if (_rightHandHit.hit)
        {

            int currentLayerIndex = rightHit.collider.gameObject.layer;
            string currentLayerName = LayerMask.LayerToName(currentLayerIndex);
            displayText.text = currentLayerName;


            //If a furniture is selected from the UI

            if (_furnitureBehaviour != null)
            {

                LayerMask mask = _furnitureBehaviour.layer;

                // Check if the hitLayer is included in the specified layerMaskToCheck
                if ((mask.value & (1 << currentLayerIndex)) > 0)
                {
                    _furnitureBehaviour.FollowRayHit(_rightHandHit);

                    _furnitureBehaviour.HandleRotation();

                    if (CheckTriggerInput() && _furnitureBehaviour.isPlaceble)
                    {
                        TogglePlacement();
                    }
                }
                else
                {
                    _furniturePreview.GetComponent<Rigidbody>().velocity = Vector3.zero;
                } 
            }
        }

        else displayText.text = string.Empty;
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
