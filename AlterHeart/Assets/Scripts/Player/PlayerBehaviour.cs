﻿/*****************************************************************************
// File Name: PlayerBehaviour.cs
// Author:
// Creation Date: 2/6/2020
//
// Brief Description: Allows player movement via addForce(). When in dimension 1, the player can walk on walls. In dimension 2,
the player is able to jump. 
//Known Glitch 1: Movement for the wall-walking forces player to move forward in all axes, not just z and x. 
We are not currently able to stop the player from lifting off the "ground" when in that dimension.
*****************************************************************************/

using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class PlayerBehaviour : MonoBehaviour
{
    public Transform cameraAngle;
    public Transform cameraTransform;

    private Vector2 input;

    public Transform respawnPoint;
    private Rigidbody rb;
    public RealityController rc;
    public float myGravity;

    public float moveSpeed;
    public float moveLimit = 10;
    public float jumpForce;

    public float jumpForceDimension1 = 0f;
    public float jumpForceDimension2 = 1f;

    //Wall walking related
    [HideInInspector] public bool wallWalker = false;
    private readonly float lerpSpeed = 10; // smoothing speed when switching to walls

    private bool isGrounded; //whether or not the character is on the ground
    private float deltaGround = 0.2f; // character is grounded up to this distance
    private float jumpRange = 10; // range to detect target wall
    private float distGround; // distance from character position to ground

    private Vector3 surfaceNormal; // current surface normal
    private Vector3 myNormal; // character normal

    private Vector3 startNormal;
    private Vector3 startForward;

    private bool jumpingToWall = false; 

    private BoxCollider boxCollider; // drag BoxCollider ref in editor

    private bool onWall = false;

    private bool dimensionSwitchedBack; //whether or not you have switched back to the jump dimension


    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        jumpForce = jumpForceDimension2;

        boxCollider = GetComponent<BoxCollider>();
        myNormal = transform.up;
        startNormal = myNormal;
        startForward = Vector3.forward;
        distGround = boxCollider.size.y - boxCollider.center.y; // distance from transform.position to ground

        dimensionSwitchedBack = true;
    }

    private void Update()
    {
        onWall = myNormal != Vector3.up;

        if (rc.currentReality == 2) //Wall walking activated
        {
            dimensionSwitchedBack = false;
            WallWalking();
        }
        else if(rc.currentReality == 1)
        {
            NormMovement();

            if (Input.GetButtonDown("Jump"))
            {
                Jump();
            }
        }
        
    }

    private void LateUpdate()
    {
        rb.AddForce(-myGravity * myNormal);
    }

    private void NormMovement()
    {
        //Switches back to normal movement if back to jumping dimension
        if(!dimensionSwitchedBack)
        {

            //Quaternion targetRot = Quaternion.LookRotation(startForward, startNormal);
            //transform.rotation = Quaternion.Lerp(transform.rotation, targetRot, lerpSpeed * Time.deltaTime);

            //Quaternion playerRot = Quaternion.LookRotation(new Vector3(0, 0, 0), startNormal);

            transform.rotation = Quaternion.Euler(new Vector3(0, 0, 0));
            myNormal = startNormal;
            dimensionSwitchedBack = true;
        }
        

        input = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        input = Vector2.ClampMagnitude(input, 1);

        Vector3 camF = cameraTransform.forward;
        Vector3 camR = cameraTransform.right;

        camF.y = 0;
        camR.y = 0;
        camF = camF.normalized;
        camR = camR.normalized;

        if ((rb.velocity.x < moveLimit && rb.velocity.x > -moveLimit) || (rb.velocity.z < moveLimit && rb.velocity.z > -moveLimit))
        {
            Vector3 targetDirection = new Vector3(input.x, 0f, input.y);
            targetDirection = Camera.main.transform.TransformDirection(targetDirection) * moveSpeed;
            targetDirection.y = 0.0f;
            rb.AddForce(targetDirection);
        }

        //when not on the ground, pull the player downward
        if(!OnGround())
        {
            rb.AddForce(-myGravity * myNormal);
        }
    }

    void WallWalking()
    {
        if (jumpingToWall) return; // don't do any of this while jumping to a wall

        Ray ray;
        RaycastHit hit;

        if (Input.GetButtonDown("Jump")) //Just move to the wall
        {
            ray = new Ray(transform.position, transform.forward);

            if (Physics.Raycast(ray, out hit, jumpRange))
            { // wall ahead?
                JumpToWall(hit.point, hit.normal); // yes: jump to the wall
            }
        }

        // update surface normal and isGrounded:
        ray = new Ray(transform.position, -myNormal); // cast ray downwards

        if (Physics.Raycast(ray, out hit)) //if the ray hits something below player
        {
            isGrounded = hit.distance <= distGround + deltaGround;
            surfaceNormal = hit.normal;
        }
        else
        {
            isGrounded = false;
            surfaceNormal = Vector3.up; // assume usual ground normal to avoid "falling forever"
        }

        myNormal = Vector3.Lerp(myNormal, surfaceNormal, lerpSpeed * Time.deltaTime);

        // find forward direction with new myNormal:
        Vector3 myForward = Vector3.Cross(transform.right, myNormal);

        // align character to the new myNormal while keeping the forward direction:
        Quaternion targetRot = Quaternion.LookRotation(myForward, myNormal);
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRot, lerpSpeed * Time.deltaTime);

        if (isGrounded)
        {
            input = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
            input = Vector2.ClampMagnitude(input, 1);

            if ((rb.velocity.x < moveLimit && rb.velocity.x > -moveLimit) || (rb.velocity.z < moveLimit && rb.velocity.z > -moveLimit))
            {
                Vector3 targetDirection = Camera.main.transform.TransformDirection(new Vector3(input.x, 0, input.y) * moveSpeed);
                rb.AddForce(targetDirection);
            }
        }
        else
        {
            rb.AddForce(-myGravity * myNormal); //pins character to ground whenever they manage to fly upward
        }

    }

    /// <summary>
    /// Determines what transform variables are needed to move the player to the position to which they want to move
    /// </summary>
    /// <param name="point">The exact point that you will jump to on the wall</param>
    /// <param name="normal">The player's current idea of "up"</param>
    private void JumpToWall(Vector3 point, Vector3 normal)
    {
        // jump to wall
        jumpingToWall = true;
        rb.isKinematic = true;

        Vector3 origPos = transform.position;
        Quaternion origRot = transform.rotation;

        Vector3 newPos = point + normal * (distGround + 0.5f); // will jump to 0.5 above wall
        Vector3 myForward = Vector3.Cross(transform.right, normal);
        Quaternion newRot = Quaternion.LookRotation(myForward, normal);

        StartCoroutine(JumpTime(origPos, origRot, newPos, newRot, normal));
    }

    /// <summary>
    /// Slowly moves player to new position
    /// </summary>
    /// <param name="origPos">The player's original position</param>
    /// <param name="origRot">The player's original rotation</param>
    /// <param name="newPos">The player's new position</param>
    /// <param name="newRot">The player's new rotation</param>
    /// <param name="normal">Which direction is "up" for the player</param>
    /// <returns></returns>
    private IEnumerator JumpTime(Vector3 origPos, Quaternion origRot, Vector3 newPos, Quaternion newRot, Vector3 normal)
    {
        for (float t = 0.0f; t < 1.0f;)
        {
            t += Time.deltaTime;
            transform.position = Vector3.Lerp(origPos, newPos, t);
            transform.rotation = Quaternion.Slerp(origRot, newRot, t);
            yield return null; // return here next frame
        }
        myNormal = normal; // update myNormal
        rb.isKinematic = false; // enable physics
        jumpingToWall = false; // jumping to wall finished
    }

    /// <summary>
    /// Causes player to fall back to the floor
    /// </summary>
    void BackToFloor()
    {
        Vector3 floorNormal = Vector3.up;

        //set normal back to floor
        //rotate character to proper floor position
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

    public void Respawn()
    {
        transform.position = respawnPoint.position;
        rb.velocity = Vector3.zero;

        if (rc.currentReality != 1)
        {
            StartCoroutine(rc.SwapRealities()); 
        }
    }

    /// <summary>
    /// Whether or not the player is currently on the ground. Only use for jumping dimension code
    /// </summary>
    /// <returns></returns>
    private bool OnGround()
    {
        bool result = false;
        if (Physics.Raycast(transform.position, Vector3.down, out RaycastHit Hit, 1f))
        {
            if (Hit.transform.gameObject != null)
            {
                result = true;
            }
        }
        
        return result;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Respawn")
        {
            print("Respawning");
            Respawn();
        }
    }
}
