using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetUIPosition : MonoBehaviour
{
    [SerializeField] private Transform _newTransform;
    [SerializeField] private GameObject player;

    public float moveSpeed = 5f;  // Speed of movement
    public float rotationSpeed = 2f;  // Speed of rotation

    private Vector3 targetPosition;
    private Quaternion targetRotation;
    private bool shouldMove = false;

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

            // Set the target position and rotation
            targetPosition = _newTransform.position;
            targetRotation = Quaternion.LookRotation(player.transform.position - targetPosition);
            targetRotation *= Quaternion.Euler(-12, 180, 0);

            // Flag to start moving
            shouldMove = true;
        }

        if (shouldMove)
        {
            // Animate the position and rotation
            transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * moveSpeed);
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);

            // Stop moving when close enough to the target
            if (Vector3.Distance(transform.position, targetPosition) < 0.01f && Quaternion.Angle(transform.rotation, targetRotation) < 0.1f)
            {
                shouldMove = false;  // Movement and rotation are done
            }
        }
    }
}
