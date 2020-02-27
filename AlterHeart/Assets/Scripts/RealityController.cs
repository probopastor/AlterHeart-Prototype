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

    public Light directionalLight;
    public Color[] dimensionLightColor;

    public GameObject player;

    private int currentReality = 1;
    private GameObject[] DimensionOnePoints;
    private GameObject[] DimensionTwoPoints;

    private CollisionSphere currentSphere;

    //[0] = normal downward gravity
    //[1] = Pull to the left
    //[2] = Pull to the right
    //[3] = Upside down gravity
    public Vector3[] gravities = new Vector3[4];

    private bool realitiesPaused;

    private void Start()
    {
        teleportationDistance = teleportRealityTwo.position - teleportRealityOne.position;

        //Set initial variables
        canTeleport = true;
        currentReality = 1;
        directionalLight.color = dimensionLightColor[0];


        realitiesPaused = false;

        controlPanelDimension1.SetActive(true);
        controlPanelDimension2.SetActive(false);
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
    /// <returns></returns>
    private IEnumerator ChangeRealities()
    {
        if(canTeleport)
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
                    directionalLight.color = dimensionLightColor[1];

                    controlPanelDimension1.SetActive(false);
                    controlPanelDimension2.SetActive(true);
                    player.GetComponent<PlayerBehaviour>().jumpForce = player.GetComponent<PlayerBehaviour>().jumpForceDimension2;

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
                    directionalLight.color = dimensionLightColor[0];

                    controlPanelDimension1.SetActive(true);
                    controlPanelDimension2.SetActive(false);

                    player.GetComponent<PlayerBehaviour>().jumpForce = player.GetComponent<PlayerBehaviour>().jumpForceDimension1;

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
