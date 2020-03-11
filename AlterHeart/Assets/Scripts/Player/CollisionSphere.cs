using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*****************************************************************************
// File Name: CollisionSphere.cs
// Author: Billy
// [Currently unused]

// Brief Description: A simple trigger that only returns whether or not it is intersecting
with another object. Spawned in a mirrored position to the player in the opposite dimension
to determine if they are able to teleport.
*****************************************************************************/

public class CollisionSphere : MonoBehaviour
{
    public bool intersecting = false;

    private void OnTriggerEnter(Collider other)
    {

        if (this.gameObject != null)
        {

            if (other.CompareTag("Obstacle"))
            {
                intersecting = true;
            }
            else
            {
                intersecting = false;
            }
        }
        
    }
}
