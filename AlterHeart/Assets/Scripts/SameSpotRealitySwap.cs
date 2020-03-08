/*****************************************************************************
// File Name: SameSpotRealitySwap.cs
// Author:
// Creation Date: 
//
// Brief Description:
*****************************************************************************/
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class SameSpotRealitySwap : MonoBehaviour
{
    public Transform realityOne;
    public Transform realityTwo;
    public GameObject player;

    private Vector3 teleportDistance;

    //lighting
    public Light myLighting;
    public Color r1Light;
    public Color r2Light;


    private int currentReality;
    private void Start()
    {
        currentReality = 1;
        teleportDistance = realityTwo.position - realityOne.position;

        myLighting.color = r1Light;
    }

    void Update()
    {
        print(currentReality);
        if (Input.GetButtonDown("Swap Realities"))
        {
            StartCoroutine(SwitchReality());
            print("Switching Realities");
        }
    }

    /// <summary>
    /// Switches player position to the equivalent position on the other "dimension"
    /// </summary>
    /// <returns></returns>
    private IEnumerator SwitchReality()
    {
        player.GetComponent<PlayerBehaviour>().Jump();
        player.GetComponent<Rigidbody>().velocity = Vector3.zero;

        Vector3 newPos = player.transform.position;
        yield return new WaitForSeconds(.2f);

        if (currentReality == 1)
        {
            newPos += teleportDistance;
            currentReality = 2;
            myLighting.color = r2Light;
        }
        else if (currentReality == 2)
        {
            newPos -= teleportDistance;

            currentReality = 1;
            myLighting.color = r1Light;
        }

        player.transform.position = newPos;
    }
}
