using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
