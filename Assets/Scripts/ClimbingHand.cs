using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class ClimbingHand : MonoBehaviour
{

    [SerializeField]
    private XRController controller; 
    
    [SerializeField]
    private ContinuousMovement movementManager;
    [SerializeField]
    private CharacterController characterController;
    private Vector3 lastPosition = Vector3.zero;
    public Vector3 delta { get; private set; } = Vector3.zero;
    private bool initialGrabFrame = true;
    private bool isPressed = false;
    private bool readyToClimb = false;

    [SerializeField]
    private GameObject staticClimbingHand;
    private int lines = 0;

    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<XRController>();
        lastPosition = transform.position;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        delta = lastPosition - transform.position;

        bool pressed = false;
        controller.inputDevice.IsPressed(controller.selectUsage, out pressed);

        bool released = pressed && !isPressed;
        
        if (!pressed)
        {
            if (gameObject.name == "RightHand")
            {
                Debug.Log("X Distance Before: " + (Mathf.Round(Mathf.Abs(lastPosition.x - transform.position.x) * 1000f) / 1000f));
            }

            if (initialGrabFrame)
            {
                ClimbingManager.instance.handGrabbed(this);
                initialGrabFrame = false;
                isPressed = true;
                // LockHand();
            }

            movementManager.MoveClimbingPlayer(delta);
            lastPosition = transform.position;
            if (gameObject.name == "RightHand")
            {
                Debug.Log("X Distance After: " + (Mathf.Round(Mathf.Abs(lastPosition.x - transform.position.x) * 1000f) / 1000f));
            }

        }

        else if (released)
        {
            ClimbingManager.instance.handReleased(this);
            isPressed = false;
            // UnlockHand();
        }

        else 
        {
            initialGrabFrame = true;
        }
    }

    void LateUpdate() 
    {
        if (gameObject.name == "RightHand")
        {
            Debug.DrawLine(lastPosition, transform.position, Color.green, 5f);
        }
        lastPosition = transform.position;
        
    }

    private void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.tag == "Climbable")
        {
            readyToClimb = true;
        }
    }

    private void OnTriggerExit(Collider col)
    {
        if (col.tag == "Climbable")
        {
            readyToClimb = false;
        }
    }

    // Render locked hand mesh, while disabling the primary MeshRenderer.
    // For climbing, this is so the player can't move as they climb
    private void LockHand()
    {
        staticClimbingHand.transform.position = transform.position;
        staticClimbingHand.SetActive(true);

        gameObject.GetComponentInChildren<MeshRenderer>().enabled = false;
    }

    private void UnlockHand()
    {
        staticClimbingHand.SetActive(false);

        gameObject.GetComponentInChildren<MeshRenderer>().enabled = true;
    }
}
