/*****************************************************************************
// File Name: ActivationObject.cs
// Author:
// Creation Date: 
//
// Brief Description: A basic constructor for objects which are interactable through
buttons. 
*****************************************************************************/
using UnityEngine;

public class ActivationObject : MonoBehaviour
{
    public virtual void Activate()
    {
        print("I have been activated");
    }
}
