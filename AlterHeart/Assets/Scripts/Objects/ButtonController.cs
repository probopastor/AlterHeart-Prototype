﻿/*****************************************************************************
// File Name: ButtonController.cs
// Author: Billy
//
// Brief Description: Defines the use of buttons in game. When buttons are selected
by the cursor, they change color. They also have an object (assigned in editor) 
that is activated when the button is pressed.
*****************************************************************************/

using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ButtonController : MonoBehaviour
{
    public AudioSource SoundEffectSource;
    public AudioClip buttonSound;

    public bool isPushed;
    public ActivationObject activationObject;

    public ParticleSystem pushParticles;
    GameObject pushParticlesObj;
    public float yParticleModifier = 0f;

    public Material unHighlightColor;
    public Material highlightColor;
    RealityController rc;

    public bool anotherDimensionEffect;

    // Start is called before the first frame update
    void Start()
    {
        rc = GameObject.FindGameObjectWithTag("RealityController").GetComponent<RealityController>();
        isPushed = false;
        
    }


    public void Highlight()
    {
        print("Highlighting");
        GetComponent<MeshRenderer>().material = highlightColor;
    }

    public void UnHighlight()
    {
        if (!isPushed)
        {
            print("Better un highlight");
            GetComponent<MeshRenderer>().material = unHighlightColor;
        }

    }

    public void PushButton()
    {
        if (!isPushed)
        {
            isPushed = true;
            //SoundEffectSource.clip = buttonSound;
            //SoundEffectSource.Play();
            Debug.Log("Button Pushed ");

            Vector3 landParticlesLocation = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y + yParticleModifier, gameObject.transform.position.z);
            pushParticlesObj = Instantiate(pushParticles.gameObject, landParticlesLocation, Quaternion.Euler(-90f, 0f, 0f));
            pushParticles.Play();

            Destroy(pushParticlesObj, 3f);

            activationObject.Activate();

            if(anotherDimensionEffect)
                StartCoroutine(rc.TextChange());
        }
    }

    
}
