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
    public Vector3 teleportationDistance = new Vector3(0, 0, 0);
    private Vector3 playerTeleportDistance;

    public GameObject controlPanelDimension1;
    public GameObject controlPanelDimension2;

    public Light directionalLight;
    public Color[] dimensionLightColor;

    public Transform teleportRealityOne;
    public Transform teleportRealityTwo;

    public GameObject player;

    private int currentReality = 1;
    private GameObject[] DimensionOnePoints;
    private GameObject[] DimensionTwoPoints;

    public ParticleSystem teleportParticles;

    private CollisionSphere currentSphere;

    private bool realitiesPaused;

    private void Start()
    {
        canTeleport = false;

        currentReality = 1;

        realitiesPaused = false;

        controlPanelDimension1.SetActive(true);
        controlPanelDimension2.SetActive(false);
        ////myLighting.color = r1Light;
        //DimensionOnePoints = GameObject.FindGameObjectsWithTag("DimensionOnePoints");
        //DimensionTwoPoints = GameObject.FindGameObjectsWithTag("DimensionTwoPoints");

        //teleportParticles.Stop();
    }

    //IEnumerator ActivateParticles()
    //{
    //    teleportParticles.Play();
    //    yield return new WaitForSeconds(1f);
    //    teleportParticles.Stop();
    //}

    void Update()
    {
        //if (Input.GetButtonDown("Swap Realities"))
        //{
        //    Debug.Log("Swap realities button pressed. ");
        //    //StartCoroutine(SwitchReality());
        //    StartCoroutine(ChangeRealities());
        //}
        //if (Input.GetKeyDown(KeyCode.F))
        //{
        //    //StartCoroutine(Flash());
        //}

        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            //Debug.Log("Swap realities button pressed. ");
            StartCoroutine(ChangeRealities());
        }
    }

    private IEnumerator ChangeRealities()
    {
        //Debug.Log("Swap realities coroutine started. ");

        if (currentReality == 1)
        {
            //playerTeleportDistance = teleportationDistance - player.transform.position;
            //GameObject collisionSphereClone = Instantiate(collisionSphere, new Vector3(-playerTeleportDistance.x, playerTeleportDistance.y, -playerTeleportDistance.z), player.transform.rotation);

            GameObject collisionSphereClone = Instantiate(collisionSphere, new Vector3(119.9f, 1.75f, 282.8f), player.transform.rotation);
            currentSphere = FindObjectOfType<CollisionSphere>();

            if (canTeleport)
            {
                currentReality = 2;
                player.transform.position = currentSphere.transform.position;
                controlPanelDimension1.SetActive(false);
                controlPanelDimension2.SetActive(true);
            }
            Destroy(collisionSphereClone, 1);
        }
        else if (currentReality == 2)
        {
            GameObject collisionSphereClone = Instantiate(collisionSphere, new Vector3(119.9f, 1.75f, 190.73f), player.transform.rotation);
            currentSphere = FindObjectOfType<CollisionSphere>();

            if (canTeleport)
            {
                currentReality = 1;

                player.transform.position = currentSphere.transform.position;
                controlPanelDimension1.SetActive(true);
                controlPanelDimension2.SetActive(false);
            }
            Destroy(collisionSphereClone, 1);

            //playerTeleportDistance = teleportationDistance + player.transform.position;
            //GameObject collisionSphereClone = Instantiate(collisionSphere, playerTeleportDistance, player.transform.rotation);
        }


        yield return new WaitForSeconds(0.1f);
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
