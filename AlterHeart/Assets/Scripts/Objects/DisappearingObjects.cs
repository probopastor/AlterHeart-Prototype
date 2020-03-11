/*****************************************************************************
// File Name: DisappearingObjects.cs
// Author: Scott 
// Creation Date: 3/11/2020
//
// Brief Description: Objects that disappear when buttons are pressed
*****************************************************************************/
using UnityEngine;

public class DisappearingObjects : ActivationObject
{
    void Start()
    {
        gameObject.SetActive(true);
    }

    public override void Activate()
    {
        base.Activate();
        gameObject.SetActive(false);
    }
}
