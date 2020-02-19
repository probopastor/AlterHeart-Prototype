/*****************************************************************************
// File Name: TeleportPoints.cs
// Author:
// Creation Date: 2/11/2020
//
// Brief Description:
*****************************************************************************/
using UnityEngine;

public class TeleportPoints : MonoBehaviour
{
    public GameObject partner;
    public Vector3 myLocation;

    private void Start()
    {
        myLocation = transform.position;
    }

    public float CompareDistance(Vector3 player)
    {
        return Vector3.Distance(player, myLocation);
    }
}
