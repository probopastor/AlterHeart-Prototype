﻿/*****************************************************************************
// File Name: RealityController.cs
// Author: Scott, Billy
// Creation Date: 2/6/2020
//
// Brief Description: Controls movement between "dimensions". When the button is pressed,
the game determines which teleportPoint is closest to the player and teleports them to its partner
in the other map.

At the start of the game, the game creates partners in the mirror dimension for all 
points that do not already have assigned partners
*****************************************************************************/

using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class RealityController : MonoBehaviour
{
    public static bool canTeleport;
    public GameObject collisionSphere;
    
    public GameObject jumpControls;
    public GameObject wallWalkControls;

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

    public GameObject somethingChangedText;

    private void Start()
    {
        teleportationDistance = teleportRealityTwo.position - teleportRealityOne.position;

        //Set initial variables for reality two
        canTeleport = true;
        currentReality = 1;

        foreach(Light item in directionalLights)
        {
            item.color = dimensionLightColor[0]; 
        }

        realitiesPaused = false;

        player.GetComponent<PlayerBehaviour>().jumpForce = player.GetComponent<PlayerBehaviour>().jumpForceDimension1;

        jumpControls.SetActive(true);
        wallWalkControls.SetActive(false);

        DimensionOnePoints = GameObject.FindGameObjectsWithTag("DimensionOnePoints");
        DimensionTwoPoints = new GameObject[DimensionOnePoints.Length];
        GeneratePartnerPoints();
        somethingChangedText.SetActive(false);

    }

    void Update()
    {
        //if (Input.GetButtonDown("Swap Realities"))
        //{ 
        //    StartCoroutine(ChangeRealities());
        //}

        if (Input.GetButtonDown("Swap Realities"))
        {
            StartCoroutine(SwapRealities());
        }
    }

    /// <summary>
    /// Coroutine changes reality player is in based on the mirrored 
    /// current player position. [UNUSED]
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

                    jumpControls.SetActive(false);
                    wallWalkControls.SetActive(true);
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

                    jumpControls.SetActive(true);
                    wallWalkControls.SetActive(false);

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
    /// Teleports player based on the position of the closest teleport point
    /// </summary>
    /// <returns></returns>
    public IEnumerator SwapRealities()
    {
        if (canTeleport) //only if it is possible to teleport
        {
            Vector3 newPos = player.transform.position;

            newPos = ClosestPoint();
            player.transform.position = newPos;

            
            if (currentReality == 1)
            {
                jumpControls.SetActive(true);
                wallWalkControls.SetActive(false);

                currentReality = 2;
                foreach (Light item in directionalLights)
                {
                    item.color = dimensionLightColor[1];
                }
            }
            else if (currentReality == 2)
            {
                jumpControls.SetActive(true);
                wallWalkControls.SetActive(false);
                currentReality = 1;
                foreach (Light item in directionalLights)
                {
                    item.color = dimensionLightColor[0];
                }
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


                Material matchingMaterial = thisPoint.GetComponent<TeleportPoints>().myMaterial;
                newPoint.GetComponent<TeleportPoints>().SetMaterial(matchingMaterial);

                //Assign these to each other as partners
                thisPoint.GetComponent<TeleportPoints>().partner = newPoint;
                newPoint.GetComponent<TeleportPoints>().partner = thisPoint;

                //fill out the second t.point array
                DimensionTwoPoints[i] = newPoint;
            }
        }
    }

    /// <summary>
    /// Finds which teleport point is closest to the player
    /// </summary>
    /// <returns>The position of the closest teleport point's partner</returns>
    private Vector3 ClosestPoint()
    {
        Vector3 result = Vector3.zero;

        if (currentReality == 1) 
        {
            float lowestDist = 1000000; //set a completely unrealistic distance to start with

            for (int i = 0; i < DimensionOnePoints.Length; i++) //check each and every t.point in the array
            {
                float thisDist = DimensionOnePoints[i].GetComponent<TeleportPoints>().CompareDistance(player.transform.position);

                //Figure out which point is closest to the player
                if (thisDist < lowestDist)
                {
                    lowestDist = thisDist;
                    result = DimensionOnePoints[i].GetComponent<TeleportPoints>().partner.transform.position;
                }
            }
        }
        //reversed copy of the above code
        else if (currentReality == 2)
        {
            float lowestDist = 1000000;

            for (int i = 0; i < DimensionTwoPoints.Length; i++)
            {
                float thisDist = DimensionTwoPoints[i].GetComponent<TeleportPoints>().CompareDistance(player.transform.position);

                if (thisDist < lowestDist)
                {
                    lowestDist = thisDist;

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
            jumpControls.SetActive(false);
            wallWalkControls.SetActive(false);
        }
        else if (realitiesPaused)
        {
            realitiesPaused = false;
            if (currentReality == 1)
            {
                jumpControls.SetActive(true);
            }
            else if (currentReality == 2)
            {
                wallWalkControls.SetActive(true);
                
            }
        }

    }

    public IEnumerator TextChange()
    {
        somethingChangedText.SetActive(true);
        yield return new WaitForSeconds(3f);
        somethingChangedText.SetActive(false);
    }
}
