using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionSphere : MonoBehaviour
{
    public float destructionTime = 1f;
    public PlayerBehaviour player;

    public RealityController thisReality;

    // Start is called before the first frame update
    void Start()
    {
        //thisReality = FindObjectOfType<RealityController>();
        //player = FindObjectOfType<PlayerBehaviour>();
       // Destroy(gameObject, destructionTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("I'm triggered!!! ");

        if(this.gameObject != null)
        {
            if (other.CompareTag("Obstacle"))
            {
                RealityController.canTeleport = false;

                //thisReality.canTeleport = false;
                Debug.Log("I'm touching an Obstacle! Oh no! ");

                //thisReality.canTeleport = false;
                Debug.Log("Cant teleport. ");
                //player.transform.position = teleportationDistance;
            }
            else
            {
                Debug.Log("Teleport possible. ");

                RealityController.canTeleport = true;
                //thisReality.canTeleport = true;
            }
        }
        
    }
}
