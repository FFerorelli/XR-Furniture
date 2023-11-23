using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IFurniture 
{
     void FollowRayHit((Vector3 point, Vector3 normal, bool hit) ray);

}
