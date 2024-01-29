using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowParent : MonoBehaviour
{
    public Transform parentTransform;

    void Update()
    {
        Vector3 newPosition = new Vector3(parentTransform.position.x, 0, parentTransform.position.z);
        transform.position = newPosition;

    }
}
