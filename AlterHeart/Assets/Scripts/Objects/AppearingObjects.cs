/*****************************************************************************
// File Name: AppearingObjects.cs
// Author: Scott Acker
// Creation Date: 3/11/2020
//
// Brief Description: Makes objects appear when the connected button calls Activate
*****************************************************************************/
using UnityEngine;

public class AppearingObjects : ActivationObject
{
    void Start()
    {
        gameObject.SetActive(false);
    }

    public override void Activate()
    {
        base.Activate();
        gameObject.SetActive(true);
    }
}
