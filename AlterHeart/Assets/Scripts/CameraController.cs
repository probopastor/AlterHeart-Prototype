//TEST 1

using UnityEngine;
using UnityEngine.UI;
public class CameraController : MonoBehaviour
{
    public Image crossHair;
    public Sprite crossHairUnselected;
    public Sprite crossHairSelected;

    public float clickDistance = 1f;

    public LayerMask raycastLayer;
    public LayerMask raycastLayerTeleporter;
    public LayerMask winLayer;

    public float spherecastRadius = 1f;

    public float mouseSensitivity = 100f;
    public Transform playerBody;
    private float xRotation = 0f;

    private Vector3 startPos;

    float mouseX;
    float mouseY;

    ButtonController lastButton;

    private void Start()
    {
        crossHair.sprite = crossHairUnselected;
        mouseX = 0;
        mouseY = 0;

        //Cursor.SetCursor(cursorTexture, hotSpot, cursorMode);
        //Cursor.lockState = CursorLockMode.None;
        //Cursor.visible = false;
        //startPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90, 90);

        transform.localRotation = Quaternion.Euler(xRotation, 0, 0);

        playerBody.Rotate(Vector3.up * mouseX);

        RaycastHit hit;
        ///Ray ray = Camera.main.ScreenPointToRay(crossHair.transform.position);


        
        // Does the ray intersect any objects excluding the player layer?
        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, clickDistance, raycastLayer))
        {
            crossHair.sprite = crossHairSelected;
            
            //highlight the selected button
            if (hit.collider.GetComponent<ButtonController>() != null)
            {
                lastButton = hit.collider.GetComponent<ButtonController>();
                lastButton.Highlight();
            }

            //Debug.DrawRay(ray, transform.TransformDirection(Vector3.forward) * hit.distance, Color.yellow);

            if (Input.GetKeyDown(KeyCode.Mouse0) && lastButton != null)
            {
                lastButton.PushButton();
            }
        }
        else if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, clickDistance, raycastLayerTeleporter))
        {
            crossHair.sprite = crossHairSelected;

            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                if (hit.collider.GetComponent<TeleportShapeController>() != null)
                {
                    hit.collider.GetComponent<TeleportShapeController>().TeleportPlayerToPoint();
                }
            }
        }
        else if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, clickDistance, winLayer))
        {
            crossHair.sprite = crossHairSelected;

            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                if (hit.collider.GetComponent<WinShapeController>() != null)
                {
                    hit.collider.GetComponent<WinShapeController>().Win();
                }
            }
        }
        else
        {
            crossHair.sprite = crossHairUnselected;
            if (lastButton != null) //unhighlights the last button and erases its reference as soon as the player moves away
            {
                lastButton.UnHighlight();
                lastButton = null;
            }
        }

        /*
        // Spherecast to determine object hit
        if (Physics.SphereCast(Camera.main.ScreenToWorldPoint(Input.mousePosition), spherecastRadius, transform.forward, out hit, Mathf.Infinity, raycastLayer, QueryTriggerInteraction.UseGlobal))
        {
            Debug.Log("Hovering Over Button ");
            
            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                if (hit.collider.GetComponent<ButtonController>() != null)
                {
                    hit.collider.GetComponent<ButtonController>().PushButton();
                }
            }

            //Debug.Log(hit.collider.isTrigger);  
        }*/
    }


    void LateUpdate()
    {

        //transform.position = playerBody.position + startPos;
    }
}
