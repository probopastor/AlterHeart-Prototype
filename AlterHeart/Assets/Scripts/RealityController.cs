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
    public bool canTeleport;
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

    private bool realitiesPaused;
    private bool canSwap = true;

    private void Start()
    {
        canTeleport = false;

        //currentReality = 1;

        //realitiesPaused = false;

        //controlPanelDimension1.SetActive(true);
        //controlPanelDimension2.SetActive(false);
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

        playerTeleportDistance = teleportationDistance + player.transform.position;
        collisionSphere = Instantiate(collisionSphere, playerTeleportDistance, player.transform.rotation);
        //if(canTeleport)
        //{
        //    Debug.Log("canTeleport triggered. ");
        //    player.transform.position = teleportationDistance;
        //}
        //else
        //{
        //    Debug.Log("Can't teleport you ugly human. Think about what you've done! ");
        //}

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

    //public void RealityPanelActivation()
    //{
    //    if(!realitiesPaused)
    //    {
    //        realitiesPaused = true;
    //        controlPanelDimension1.SetActive(false);
    //        controlPanelDimension2.SetActive(false);
    //    }
    //    else if(realitiesPaused)
    //    {
    //        realitiesPaused = false;
    //        if(currentReality == 1)
    //        {
    //            controlPanelDimension1.SetActive(true);
    //        }
    //        else if(currentReality == 2)
    //        {
    //            controlPanelDimension2.SetActive(true);
    //        }
    //    }

    //}
}
