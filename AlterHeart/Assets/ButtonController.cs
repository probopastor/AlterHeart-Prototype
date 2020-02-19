using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonController : MonoBehaviour
{
    public AudioSource SoundEffectSource;
    public AudioClip buttonSound;

    public bool isPushed;
    public GameObject activationObject;

    public ParticleSystem pushParticles;
    GameObject pushParticlesObj;
    public float yParticleModifier = 0f;

    // Start is called before the first frame update
    void Start()
    {
        isPushed = false;
        activationObject.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void PushButton()
    {
        if(!isPushed)
        {
            isPushed = true;
            SoundEffectSource.clip = buttonSound;
            SoundEffectSource.Play();
            Debug.Log("Button Pushed ");

            Vector3 landParticlesLocation = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y + yParticleModifier, gameObject.transform.position.z);
            pushParticlesObj = Instantiate(pushParticles.gameObject, landParticlesLocation, Quaternion.Euler(-90f, 0f, 0f));
            pushParticles.Play();

            Destroy(pushParticlesObj, 3f);

            activationObject.SetActive(false);
        }
    }
}
