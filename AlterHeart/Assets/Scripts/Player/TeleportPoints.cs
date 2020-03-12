/*****************************************************************************
// File Name: TeleportPoints.cs
// Author: Scott
// Creation Date: 2/11/2020
//
// Brief Description: Shares a partner in the opposite dimension, which can be accessed 
in order to determine where the player will appear when they switch dimensions.
If this does not have a partner assigned, it will generate its own partner at start() 
in RealityController().
*****************************************************************************/
using UnityEngine;

public class TeleportPoints : MonoBehaviour
{
    public GameObject partner;
    public Vector3 myLocation;
    public Material myMaterial;
    public ParticleSystem particles;
    private Renderer particleColor;

    private void Start()
    {
        myLocation = transform.position;
        particleColor = particles.GetComponent<ParticleSystemRenderer>();
        particleColor.material = myMaterial;
    }

    public void SetMaterial(Material myNewMaterial)
    {
        myMaterial = myNewMaterial;

        //particleColor.material = myNewMaterial;
    }

    public float CompareDistance(Vector3 player)
    {
        return Vector3.Distance(player, myLocation);
    }
}
