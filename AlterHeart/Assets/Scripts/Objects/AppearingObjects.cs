/*****************************************************************************
// File Name: AppearingObjects.cs
// Author:
// Creation Date: 
//
// Brief Description:
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
