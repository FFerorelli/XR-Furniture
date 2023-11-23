using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IFurniture 
{
     void FollowRayHit((Vector3 point, bool hit) ray);

}
