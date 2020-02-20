using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportShapeController : MonoBehaviour
{
    public GameObject player;
    public GameObject pointToTeleport;

    public void TeleportPlayerToPoint()
    {
        Debug.Log("wonk? ");
        player.transform.position = pointToTeleport.transform.position;
    }
}
