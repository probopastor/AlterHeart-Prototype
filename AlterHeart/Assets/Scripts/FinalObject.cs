/*****************************************************************************
// File Name: FinalObject.cs
// Author:
// Creation Date: 3/7/2020
//
// Brief Description: The final collectable at the end of the game. 
Floats toward the player upon reaching a certain distance, then ends the game upon contact.
Disables player movement for this "cutscene"
*****************************************************************************/
using UnityEngine;
using UnityEngine.SceneManagement;
public class FinalObject : MonoBehaviour
{
    private GameObject player;
    private bool activated = false;

    private void Update()
    {
        if(activated) //slowly float toward player
        {
            transform.position = Vector3.MoveTowards(transform.position, player.transform.position, 5 * Time.deltaTime);
        }
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            player = other.gameObject;
            activated = true;
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "Player")
        {
            //TODO: EndGame
            SceneManager.LoadScene("End Menu");
        }
    }
}
