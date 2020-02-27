﻿/*****************************************************************************
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

    private IEnumerator ChangeRealities()
    {
        //Debug.Log("Swap realities coroutine started. ");
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

                //playerTeleportDistance = teleportationDistance + player.transform.position;
                //GameObject collisionSphereClone = Instantiate(collisionSphere, playerTeleportDistance, player.transform.rotation);
            }


            yield return new WaitForSeconds(.5f); //Cooldown for teleporting
            canTeleport = true;
        }
    }
        


    //Decides which TeleportPoint in the current dimension is closest
    //private Vector3 ClosestPoint()
    //{
    //    Vector3 result = Vector3.zero;

    //    if(currentReality == 1)
    //    {
    //        float lowestDist = 1000000;
    //        directionalLight.color = dimensionLightColor[0];

    //        for (int i = 0; i < DimensionOnePoints.Length; i++)
    //        {
    //            float thisDist = DimensionOnePoints[i].GetComponent<TeleportPoints>().CompareDistance(player.transform.position);

    //            if (thisDist < lowestDist)
    //            {
    //                lowestDist = thisDist;
    //                result = DimensionOnePoints[i].GetComponent<TeleportPoints>().partner.transform.position;
    //            }
    //        }
    //    }
    //    else if(currentReality == 2)
    //    {
    //        float lowestDist = 1000000;
    //        directionalLight.color = dimensionLightColor[1];

    //        for (int i = 0; i < DimensionTwoPoints.Length; i++)
    //        {
    //            float thisDist = DimensionTwoPoints[i].GetComponent<TeleportPoints>().CompareDistance(player.transform.position);

    //            if (thisDist < lowestDist)
    //            {
    //                lowestDist = thisDist;

    //                result = DimensionTwoPoints[i].GetComponent<TeleportPoints>().partner.transform.position;
    //            }
    //        }
    //    }

    //    return result;
    //}

    //Switches reality by teleporting 
    //private IEnumerator SwitchReality()
    //{
    //    if(canSwap)
    //    {
    //        canSwap = false;
    //        Vector3 newPos = player.transform.position;

    //        newPos = ClosestPoint();
    //        player.transform.position = newPos;

    //        if (currentReality == 1)
    //        {
    //            currentReality = 2;
    //            player.GetComponent<PlayerBehaviour>().jumpForce = player.GetComponent<PlayerBehaviour>().jumpForceDimension2;

    //            controlPanelDimension1.SetActive(false);
    //            controlPanelDimension2.SetActive(true);
    //        }
    //        else if (currentReality == 2)
    //        {
    //            currentReality = 1;
    //            player.GetComponent<PlayerBehaviour>().jumpForce = player.GetComponent<PlayerBehaviour>().jumpForceDimension1;


    //            controlPanelDimension1.SetActive(true);
    //            controlPanelDimension2.SetActive(false);
    //        }
    //        yield return new WaitForSeconds(.5f); //cooldown for swapping realities
    //        canSwap = true;
    //    }
    //}

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
