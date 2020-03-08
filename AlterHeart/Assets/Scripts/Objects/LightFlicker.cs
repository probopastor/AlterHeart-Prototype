/*****************************************************************************
// File Name: LightFlicker.cs
// Author:
// Creation Date: 
//
// Brief Description:
*****************************************************************************/
using UnityEngine;

public class LightFlicker : MonoBehaviour
{
    private Light thisLight;

    public float flickerDuration = 2f;
    public float minIntensity = 2f;
    public float maxIntensity = 5f;

    public float intensityModifier = 5f;

    void Start()
    {
        thisLight = gameObject.GetComponent<Light>();
    }

    void Update()
    {
        PulsingLight();
    }

    private void PulsingLight()
    {
        float phi = (Time.time / flickerDuration) * 2 * Mathf.PI;
        float amplitude = Mathf.Cos(phi) * 0.5f + 0.5f;
        float newIntensity = amplitude * intensityModifier;

        if (newIntensity > maxIntensity)
        {
            newIntensity = maxIntensity;
        }

        if (newIntensity < minIntensity)
        {
            newIntensity = minIntensity;
        }

        thisLight.intensity = newIntensity;
    }
}
