using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomElement : MonoBehaviour
{
    private MeshRenderer customPart; /*{ get; set; }*/
    private Material defaultMaterial;
    public Material[] availableMaterials; /*{ get; set; }*/


    // Start is called before the first frame update
    void Start()
    {
        MeshRenderer meshRenderer = GetComponent<MeshRenderer>();
        defaultMaterial = availableMaterials[0];

        if (meshRenderer != null)
        {
            Debug.Log($"Assigning default material to {gameObject.name}");
            meshRenderer.material = defaultMaterial;
        }
        else
        {
            Debug.Log($"{gameObject.name} does not have a MeshRenderer. Assigning default material to its children.");
            MeshRenderer[] childRenderers = GetComponentsInChildren<MeshRenderer>();
            foreach (MeshRenderer childRenderer in childRenderers)
            {
                Debug.Log($"Assigning default material to child {childRenderer.gameObject.name}");
                childRenderer.material = defaultMaterial;
            }
        }
    }


    // Update is called once per frame
    void Update()
    {
        
    }
}
