using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetUIPosition : MonoBehaviour
{
    [SerializeField] private Transform _newTransform;
    [SerializeField] private GameObject player;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        if (OVRInput.GetDown(OVRInput.Button.Start))
        {
            Debug.Log("Resetting UI position");
            transform.position = _newTransform.position; 
            transform.LookAt(player.transform.position);
            transform.Rotate(-17, 180, 0);
        } 
    }
}
