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
        customPart = GetComponent<MeshRenderer>();
        defaultMaterial = availableMaterials[0];
        customPart.material = defaultMaterial;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
