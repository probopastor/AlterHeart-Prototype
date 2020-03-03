/*****************************************************************************
// File Name: PlayerBehaviour.cs
// Author:
// Creation Date: 2/6/2020
//
// Brief Description: Allows player movement
*****************************************************************************/

using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class PlayerBehaviour : MonoBehaviour
{
    public Transform cameraAngle;
    public Transform cameraTransform;

    private Vector2 input;

    private Rigidbody rb;
    public RealityController rc;
    public float myGravity;

    public float moveSpeed;
    public float moveLimit = 10;
    public float jumpForce;
    public float fallForce = 2;

    public float jumpForceDimension1 = 0f;
    public float jumpForceDimension2 = 1f;

    public bool secondPhase;
    public GameObject secondPhaseStart;

    //Wall walking related
    [HideInInspector] public bool wallWalker = false;
    private readonly float lerpSpeed = 10; // smoothing speed when switching to walls

    private bool isGrounded; //whether or not the character is on the ground
    private float deltaGround = 0.2f; // character is grounded up to this distance
    private float jumpRange = 10; // range to detect target wall
    private float distGround; // distance from character position to ground

    private Vector3 surfaceNormal; // current surface normal
    private Vector3 myNormal; // character normal

    private bool jumpingToWall = false; 

    public BoxCollider boxCollider; // drag BoxCollider ref in editor

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        secondPhase = false;
        jumpForce = jumpForceDimension2;

        myNormal = transform.up;
        distGround = boxCollider.size.y - boxCollider.center.y; // distance from transform.position to ground
    }

    private void FixedUpdate()
    {
        //Always apply downward force depending on which way is "up"
        rb.AddForce(-myGravity * myNormal);

    }

    private void Update()
    {

        if ((gameObject.transform.position.y <= -1.5f) && !secondPhase)
        {
            SceneManager.LoadScene("ProbuilderTest");
        }
        else if ((gameObject.transform.position.y <= -1.5f) && secondPhase)
        {
            gameObject.transform.position = secondPhaseStart.transform.position;
        }


        if (wallWalker) //Aka dimension 1
        {
            WallWalking();
        }
        else
        {
            NormMovement();

            if (Input.GetButtonDown("Jump"))
            {
                Jump();
            }
        }
        
    }

    void WallWalking()
    {
        if (jumpingToWall) return; // don't do any of this while jumping to a wall

        Ray ray;
        RaycastHit hit;

        if (Input.GetButtonDown("Jump"))
        { 
            ray = new Ray(transform.position, transform.forward);
            if (Physics.Raycast(ray, out hit, jumpRange))
            { // wall ahead?
                JumpToWall(hit.point, hit.normal); // yes: jump to the wall
            }
            else if (isGrounded)
            { // no: if grounded, jump up
                rb.velocity += jumpForce * myNormal;
            }
        }

        // movement code - turn left/right with Horizontal axis:
        
        // update surface normal and isGrounded:
        ray = new Ray(transform.position, -myNormal); // cast ray downwards

        if (Physics.Raycast(ray, out hit))
        { 
            // use it to update myNormal and isGrounded
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

        // move the character forth/back with Vertical axis:
        transform.Translate(0, 0, Input.GetAxis("Vertical") * moveSpeed * Time.deltaTime);
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

        Vector3 dstPos = point + normal * (distGround + 0.5f); // will jump to 0.5 above wall
        Vector3 myForward = Vector3.Cross(transform.right, normal);
        Quaternion dstRot = Quaternion.LookRotation(myForward, normal);

        StartCoroutine(jumpTime(origPos, origRot, dstPos, dstRot, normal));
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
    private IEnumerator jumpTime(Vector3 origPos, Quaternion origRot, Vector3 newPos, Quaternion newRot, Vector3 normal)
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

    private void NormMovement()
    {
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

        if(!OnGround())
        {
            rb.AddForce(0, -fallForce, 0);
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
        if (Physics.Raycast(transform.position, Vector3.down, out RaycastHit Hit, 1f))
        {
            if (Hit.transform.gameObject != null)
            {
                result = true;
            }
        }

        return result;
    }
}
