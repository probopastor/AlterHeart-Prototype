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
    
    public GameObject controlPanelDimension1;
    public GameObject controlPanelDimension2;

    public Transform teleportRealityOne;
    public Transform teleportRealityTwo;
    private Vector3 teleportationDistance;

    private GameObject[] DimensionOnePoints;
    private GameObject[] DimensionTwoPoints;
    public GameObject teleportPointPrefab;

    public Light[] directionalLights;
    public Color[] dimensionLightColor;

    public GameObject player;

    public int currentReality = 1;
    private bool realitiesPaused;

    private void Start()
    {
        teleportationDistance = teleportRealityTwo.position - teleportRealityOne.position;

        //Set initial variables for reality two
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
    /// Creates partners for teleport points and puts them in the right position in 
    /// </summary>
    private void GeneratePartnerPoints()
    {
        for(int i = 0; i < DimensionOnePoints.Length; i++)
        {
            GameObject thisPoint = DimensionOnePoints[i]; //Stores the current point being checked

            if (thisPoint.GetComponent<TeleportPoints>().partner == null) //Only create a new point if there is not already one assigned
            {
                Vector3 oppositePos = thisPoint.transform.position + teleportationDistance; //The matching position in the new dimension
                GameObject newPoint = Instantiate(teleportPointPrefab, oppositePos, Quaternion.identity); //new point at that destination

                //Assign these to each other
                thisPoint.GetComponent<TeleportPoints>().partner = newPoint;

            }
            
            

            
        }
    }

    private Vector3 ClosestPoint()
    {
        Vector3 result = Vector3.zero;

        if (currentReality == 1)
        {
            float lowestDist = 1000000;

            for (int i = 0; i < DimensionOnePoints.Length; i++)
            {
                float thisDist = DimensionOnePoints[i].GetComponent<TeleportPoints>().CompareDistance(player.transform.position);
                //Debug.Log("thisDist A: " + thisDist);

                if (thisDist < lowestDist)
                {
                    lowestDist = thisDist;
                    //Debug.Log("A " + lowestDist);
                    result = DimensionOnePoints[i].GetComponent<TeleportPoints>().partner.transform.position;
                }
            }
        }
        else if (currentReality == 2)
        {
            float lowestDist = 1000000;

            for (int i = 0; i < DimensionTwoPoints.Length; i++)
            {
                float thisDist = DimensionTwoPoints[i].GetComponent<TeleportPoints>().CompareDistance(player.transform.position);
                //Debug.Log("thisDist B: " + thisDist);

                if (thisDist < lowestDist)
                {
                    lowestDist = thisDist;

                    //Debug.Log("B " + lowestDist);
                    result = DimensionTwoPoints[i].GetComponent<TeleportPoints>().partner.transform.position;
                }
            }
        }

        return result;
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
