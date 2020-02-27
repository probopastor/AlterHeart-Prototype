/*****************************************************************************
// File Name: PlayerBehaviour.cs
// Author:
// Creation Date: 2/6/2020
//
// Brief Description: Allows player movement
*****************************************************************************/

using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerBehaviour : MonoBehaviour
{
    public Transform cameraAngle;
    public Transform cameraTransform;

    private float heading = 0f;
    private Vector2 input;

    private Rigidbody rb;
    public RealityController rc;
    public Vector3 myGravity;

    public float moveSpeed;
    public float moveLimit = 10;
    public float jumpForce;
    public float fallForce = 2;

    public float jumpForceDimension1 = 0f;
    public float jumpForceDimension2 = 1f;

    public bool secondPhase;
    public GameObject secondPhaseStart;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        secondPhase = false;
        jumpForce = jumpForceDimension1;
    }

    private void Update()
    {
        //Allways apply force downward when in dimension where jumping is possible
        rb.AddForce(myGravity);

        if ((gameObject.transform.position.y <= -1.5f) && !secondPhase)
        {
            SceneManager.LoadScene("ProbuilderTest");
        }
        else if ((gameObject.transform.position.y <= -1.5f) && secondPhase)
        {
            gameObject.transform.position = secondPhaseStart.transform.position;
        }

        if (Input.GetButtonDown("Jump"))
        {
            Jump();
        }

        Movement();
    }

    
    private void Movement()
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
