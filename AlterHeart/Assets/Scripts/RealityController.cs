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

    public Image whiteFlash;
    public float flashSpeed = 10;
    float flashAlpha;

    public ParticleSystem teleportParticles;

    private bool realitiesPaused;

    private void Start()
    {
        currentReality = 1;

        realitiesPaused = false;

        controlPanelDimension1.SetActive(true);
        controlPanelDimension2.SetActive(false);
        //   flashAlpha = 0;
        //  Color temp = whiteFlash.color;

        // temp.a = flashAlpha;
        //whiteFlash.color = temp;

        //myLighting.color = r1Light;
        DimensionOnePoints = GameObject.FindGameObjectsWithTag("DimensionOnePoints");
        DimensionTwoPoints = GameObject.FindGameObjectsWithTag("DimensionTwoPoints");

        //teleportParticles.Stop();
    }

    IEnumerator ActivateParticles()
    {
        teleportParticles.Play();
        yield return new WaitForSeconds(1f);
        teleportParticles.Stop();
    }

    void Update()
    {
        if (Input.GetButtonDown("Swap Realities"))
        {
            StartCoroutine(SwitchReality());
        }
        if (Input.GetKeyDown(KeyCode.F))
        {
            //StartCoroutine(Flash());
        }
    }

    

    //Decides which TeleportPoint in the current dimension is closest
    private Vector3 ClosestPoint()
    {
        Vector3 result = Vector3.zero;

        if(currentReality == 1)
        {
            float lowestDist = 1000000;
            directionalLight.color = dimensionLightColor[0];

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
        else if(currentReality == 2)
        {
            float lowestDist = 1000000;
            directionalLight.color = dimensionLightColor[1];

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

    //Switches reality by teleporting 
    private IEnumerator SwitchReality()
    {
        //player.GetComponent<PlayerBehaviour>().Jump();
        //StartCoroutine(ActivateParticles());
        yield return new WaitForSeconds(1f);
        Vector3 newPos = player.transform.position;

        newPos = ClosestPoint();
        player.transform.position = newPos;

        if (currentReality == 1)
        {
            currentReality = 2;
            player.GetComponent<PlayerBehaviour>().jumpForce = player.GetComponent<PlayerBehaviour>().jumpForceDimension2;

            controlPanelDimension1.SetActive(false);
            controlPanelDimension2.SetActive(true);
        }
        else if(currentReality == 2)
        {
            currentReality = 1;
            player.GetComponent<PlayerBehaviour>().jumpForce = player.GetComponent<PlayerBehaviour>().jumpForceDimension1;


            controlPanelDimension1.SetActive(true);
            controlPanelDimension2.SetActive(false);
        }
    }

    public void RealityPanelActivation()
    {
        if(!realitiesPaused)
        {
            realitiesPaused = true;
            controlPanelDimension1.SetActive(false);
            controlPanelDimension2.SetActive(false);
        }
        else if(realitiesPaused)
        {
            realitiesPaused = false;
            if(currentReality == 1)
            {
                controlPanelDimension1.SetActive(true);
            }
            else if(currentReality == 2)
            {
                controlPanelDimension2.SetActive(true);
            }

        }

    }

    /*
    IEnumerator Flash()
    {
        Color temp = whiteFlash.color;

        while (flashAlpha < 255)
        {
            yield return new WaitForSeconds(.1f);
            flashAlpha += flashSpeed;
            temp.a = flashAlpha;
            whiteFlash.color = temp;
        }

        yield return new WaitForSeconds(.01f);

        while (flashAlpha > 0)
        {
            yield return new WaitForSeconds(.1f);
            flashAlpha -= flashSpeed;
            temp.a = flashAlpha;
            whiteFlash.color = temp;
        }

    }*/
}
