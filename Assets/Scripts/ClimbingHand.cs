using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class ClimbingHand : MonoBehaviour
{

    [SerializeField]
    private XRController controller; 
    
    [SerializeField]
    private PlayerMovement movementManager;
    [SerializeField]
    private CharacterController characterController;
    private Vector3 lastPosition = Vector3.zero;
    public Vector3 delta { get; private set; } = Vector3.zero;
    private bool initialGrabFrame = true;
    private bool climbing = false;
    private bool canClimb = false;

    [SerializeField]
    private GameObject staticClimbingHand;

    void Start()
    {
        controller = GetComponent<XRController>();
        lastPosition = transform.position;
    }

    void FixedUpdate()
    {
        bool pressed = false;
        controller.inputDevice.IsPressed(controller.selectUsage, out pressed);

        bool released = !pressed && climbing;

        if (pressed && canClimb)
        {
            if (initialGrabFrame)
            {
                ClimbingManager.instance.handGrabbed(this);
                initialGrabFrame = false;
                climbing = true;
            }
        }

        if (released)
        {
            ClimbingManager.instance.handReleased(this);
            climbing = false;
        }
        
        else if (climbing)
        {
            ClimbingManager.instance.UpdateClimbingHand(delta);
            lastPosition = transform.position;
        }

        else 
        {
            initialGrabFrame = true;
        }

        lastPosition = transform.position;
    }

    void LateUpdate() 
    {
        delta = lastPosition - transform.position;      
    }

    private void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.tag == "Climbable")
        {
            canClimb = true;
        }
    }

    private void OnTriggerExit(Collider col)
    {
        if (col.tag == "Climbable")
        {
            canClimb = false;
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
