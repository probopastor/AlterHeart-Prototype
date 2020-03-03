/*****************************************************************************
// File Name: RealityController.cs
// Author:
// Creation Date: 2/6/2020
//
// Brief Description:
*****************************************************************************/

using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class RealityController : MonoBehaviour
{
    public static bool canTeleport;
    public GameObject collisionSphere;
    private Vector3 teleportationDistance ;

    public GameObject controlPanelDimension1;
    public GameObject controlPanelDimension2;
    public Transform teleportRealityOne;
    public Transform teleportRealityTwo;

    public Light[] directionalLights;
    public Color[] dimensionLightColor;

    public GameObject player;

    private int currentReality = 1;
    //private GameObject[] DimensionOnePoints;
    //private GameObject[] DimensionTwoPoints;

    private bool realitiesPaused;

    private void Start()
    {
        teleportationDistance = teleportRealityTwo.position - teleportRealityOne.position;

        //Set initial variables
        canTeleport = true;
        currentReality = 2;
        foreach(Light item in directionalLights)
        {
            item.color = dimensionLightColor[1];
        }


        realitiesPaused = false;

        player.GetComponent<PlayerBehaviour>().jumpForce = player.GetComponent<PlayerBehaviour>().jumpForceDimension2;
        player.GetComponent<PlayerBehaviour>().wallWalker = false;

        controlPanelDimension1.SetActive(false);
        controlPanelDimension2.SetActive(true);
    }

    void Update()
    {
        if (Input.GetButtonDown("Swap Realities"))
        { 
            StartCoroutine(ChangeRealities());
        }
    }

    /// <summary>
    /// Coroutine changes reality player is in based on the mirrored 
    /// current player position.
    /// </summary>
    private IEnumerator ChangeRealities()
    {
        if(canTeleport) //only if it is possible to teleport
        {
            if (currentReality == 1)
            {
                Vector3 newPos = player.transform.position + teleportationDistance;
                newPos.y += 1;

                //Generates a sphere to collide to see if there's anything at the teleportation location
                GameObject collisionSphereClone = Instantiate(collisionSphere, newPos, player.transform.rotation);
                bool intersectingSphere = collisionSphereClone.GetComponent<CollisionSphere>().intersecting;

                //If it hit nothing, go ahead and teleport
                if (!intersectingSphere)
                {
                    currentReality = 2;
                    player.transform.position = collisionSphereClone.transform.position; //teleports player

                    //sets variables that differentiate each dimension
                    foreach (Light item in directionalLights)
                    {
                        item.color = dimensionLightColor[1];
                    }

                    controlPanelDimension1.SetActive(false);
                    controlPanelDimension2.SetActive(true);
                    //Player cannot jump in this dimension, but can walk on walls
                    player.GetComponent<PlayerBehaviour>().jumpForce = player.GetComponent<PlayerBehaviour>().jumpForceDimension2;
                    player.GetComponent<PlayerBehaviour>().wallWalker = false;


                }

                Destroy(collisionSphereClone, .1f);
            }
            //Reversed copy of the code above
            else if (currentReality == 2)
            {
                Vector3 newPos = player.transform.position - teleportationDistance;
                newPos.y += 1;

                GameObject collisionSphereClone = Instantiate(collisionSphere, newPos, player.transform.rotation);
                bool intersectingSphere = collisionSphereClone.GetComponent<CollisionSphere>().intersecting;

                if (!intersectingSphere)
                {
                    currentReality = 1;

                    player.transform.position = collisionSphereClone.transform.position;

                    foreach (Light item in directionalLights)
                    {
                        item.color = dimensionLightColor[0];
                    }

                    controlPanelDimension1.SetActive(true);
                    controlPanelDimension2.SetActive(false);

                    player.GetComponent<PlayerBehaviour>().jumpForce = player.GetComponent<PlayerBehaviour>().jumpForceDimension1;
                    player.GetComponent<PlayerBehaviour>().wallWalker = true;
                }
                Destroy(collisionSphereClone, .1f);
            }

            yield return new WaitForSeconds(.5f); //Cooldown for teleporting
            canTeleport = true;
        }
    }

    /// <summary>
    /// When game is paused, hide the UI panel. Upon game unpause, reenable correct UI
    /// panel based on dimension player is in.
    /// </summary>
    public void RealityPanelActivation()
    {
        if (!realitiesPaused)
        {
            realitiesPaused = true;
            controlPanelDimension1.SetActive(false);
            controlPanelDimension2.SetActive(false);
        }
        else if (realitiesPaused)
        {
            realitiesPaused = false;
            if (currentReality == 1)
            {
                controlPanelDimension1.SetActive(true);
            }
            else if (currentReality == 2)
            {
                controlPanelDimension2.SetActive(true);
            }
        }

    }
}
