﻿//TEST 2
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportShapeController : MonoBehaviour
{
    public GameObject player;
    public GameObject pointToTeleport;

    public void TeleportPlayerToPoint()
    {
        player.transform.position = pointToTeleport.transform.position;
        player.GetComponent<PlayerBehaviour>().secondPhase = true;
    }
}