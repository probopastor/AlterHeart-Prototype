﻿/*****************************************************************************
// File Name: PlayerBehaviour.cs
// Author:
// Creation Date: 2/6/2020
//
// Brief Description: Allows player movement
*****************************************************************************/

using UnityEngine;

public class PlayerBehaviour : MonoBehaviour
{
    public Transform cameraAngle;
    public Transform cameraTransform;

    private float heading = 0f;
    private Vector2 input;

    private Rigidbody rb;
    public RealityController rc;

    public float moveSpeed;
    public float jumpForce;



    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }
    private void Update()
    {
        //Rotate camera 180 degrees per second based on Mouse X Movement
        // heading += Input.GetAxis("Mouse X") * Time.deltaTime * mouseSensitivity;
        //cameraAngle.rotation = Quaternion.Euler(0, heading, 0);


        input = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        input = Vector2.ClampMagnitude(input, 1);

        Vector3 camF = cameraTransform.forward;
        Vector3 camR = cameraTransform.right;

        camF.y = 0;
        camR.y = 0;
        camF = camF.normalized;
        camR = camR.normalized;
        transform.position += (camF * input.y + camR * input.x) * Time.deltaTime * moveSpeed;

        if (Input.GetButtonDown("Jump"))
        {
            Jump();
        }
    }

    private void Movement()
    {
        float xMove = Input.GetAxis("Horizontal");
        float zMove = Input.GetAxis("Vertical");

        xMove *= moveSpeed * Time.deltaTime;
        zMove *= moveSpeed * Time.deltaTime;

        if (xMove != 0 || zMove != 0)
        {
            rb.velocity = Vector3.Lerp(rb.velocity, new Vector3(xMove, rb.velocity.y, zMove), 1);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<GravityChanger>() != null)
        {

        }
    }

    private void GravityChange()
    {

    }

    /// <summary>
    /// Applies upward force if on the ground
    /// </summary>
    public void Jump()
    {
        if (OnGround())
        {
            rb.AddForce(Vector3.up * jumpForce);
        }
    }

    /// <summary>
    /// Whether or not the player is currently on the ground
    /// </summary>
    /// <returns></returns>
    private bool OnGround()
    {
        bool result = false;
        RaycastHit Hit;
        if (Physics.Raycast(transform.position, Vector3.down, out Hit, 1f))
        {
            if (Hit.transform.gameObject != null)
            {
                result = true;
            }
        }

        return result;
    }
}
