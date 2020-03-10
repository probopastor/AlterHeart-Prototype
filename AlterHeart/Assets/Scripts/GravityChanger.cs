/*****************************************************************************
// File Name: GravityChanger.cs
// Author:
// Creation Date: 
//
// Brief Description:
*****************************************************************************/
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GravityChanger : MonoBehaviour
{
    private float moveSpeed = 6; // move speed
    private float turnSpeed = 90; // turning speed (degrees/second)
    private float lerpSpeed = 10; // smoothing speed
    private float gravity = 10; // gravity acceleration
    private bool isGrounded;
    private float deltaGround = 0.2f; // character is grounded up to this distance
    private float jumpSpeed = 10; // vertical jump initial speed
    private float jumpRange = 10; // range to detect target wall
    private Vector3 surfaceNormal; // current surface normal
    private Vector3 myNormal; // character normal
    private float distGround; // distance from character position to ground
    private bool jumping = false; // flag &quot;I'm jumping to wall&quot;
    private float vertSpeed = 0; // vertical jump current speed

    private Transform myTransform;
    public BoxCollider boxCollider; // drag BoxCollider ref in editor

    private Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        myNormal = transform.up; // normal starts as character up direction
        myTransform = transform;
        rb.freezeRotation = true; // disable physics rotation
                                         
        distGround = boxCollider.extents.y - boxCollider.center.y; // distance from transform.position to ground

    }

    private void FixedUpdate()
    {
        // apply constant weight force according to character normal:
        rb.AddForce(-gravity * rb.mass * myNormal);
    }

    private void Update()
    {
        // jump code - jump to wall or simple jump
        if (jumping) return; // abort Update while jumping to a wall

        Ray ray;
        RaycastHit hit;

        if (Input.GetButtonDown("Jump"))
        { // jump pressed:
            ray = new Ray(myTransform.position, myTransform.forward);
            if (Physics.Raycast(ray, out hit, jumpRange))
            { // wall ahead?
                JumpToWall(hit.point, hit.normal); // yes: jump to the wall
            }
            else if (isGrounded)
            { // no: if grounded, jump up
                rb.velocity += jumpSpeed * myNormal;
            }
        }

        // movement code - turn left/right with Horizontal axis:
        myTransform.Rotate(0, Input.GetAxis("Horizontal") * turnSpeed * Time.deltaTime, 0);
        // update surface normal and isGrounded:
        ray = new Ray(myTransform.position, -myNormal); // cast ray downwards
        if (Physics.Raycast(ray, out hit))
        { // use it to update myNormal and isGrounded
            isGrounded = hit.distance <= distGround + deltaGround;
            surfaceNormal = hit.normal;
        }
        else
        {
            isGrounded = false;
            // assume usual ground normal to avoid "falling forever"
            surfaceNormal = Vector3.up;
        }
        myNormal = Vector3.Lerp(myNormal, surfaceNormal, lerpSpeed * Time.deltaTime);
        // find forward direction with new myNormal:
        Vector3 myForward = Vector3.Cross(myTransform.right, myNormal);
        // align character to the new myNormal while keeping the forward direction:
        Quaternion targetRot = Quaternion.LookRotation(myForward, myNormal);
        myTransform.rotation = Quaternion.Lerp(myTransform.rotation, targetRot, lerpSpeed * Time.deltaTime);
        // move the character forth/back with Vertical axis:
        //myTransform.Translate(0, 0, Input.GetAxis("Vertical") * moveSpeed * Time.deltaTime);
        //GetComponent<Rigidbody>().AddForce(0, 0, Input.GetAxis("Vertical") * Time.deltaTime);
    }

    private void JumpToWall(Vector3 point, Vector3 normal)
    {
        // jump to wall
        jumping = true; // signal it's jumping to wall
        rb.isKinematic = true; // disable physics while jumping
        Vector3 orgPos = myTransform.position;
        Quaternion orgRot = myTransform.rotation;
        Vector3 dstPos = point + normal * (distGround + 0.5f); // will jump to 0.5 above wall
        Vector3 myForward = Vector3.Cross(myTransform.right, normal);
        Quaternion dstRot = Quaternion.LookRotation(myForward, normal);

        StartCoroutine(jumpTime(orgPos, orgRot, dstPos, dstRot, normal));
        //jumptime
    }

    private IEnumerator jumpTime(Vector3 orgPos, Quaternion orgRot, Vector3 dstPos, Quaternion dstRot, Vector3 normal)
    {
        for (float t = 0.0f; t < 1.0f;)
        {
            t += Time.deltaTime;
            myTransform.position = Vector3.Lerp(orgPos, dstPos, t);
            myTransform.rotation = Quaternion.Slerp(orgRot, dstRot, t);
            yield return null; // return here next frame
        }
        myNormal = normal; // update myNormal
        rb.isKinematic = false; // enable physics
        jumping = false; // jumping to wall finished
    }
}
