using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class SwingingHand : MonoBehaviour
{
    // Start is called before the first frame update
    private XRController controller; 
    
    [SerializeField]
    private PlayerMovement movementManager;
    [SerializeField]
    private CharacterController characterController;
    private Vector3 lastPosition = Vector3.zero;
    public Vector3 delta { get; private set; } = Vector3.zero;
    private bool initialGrabFrame = true;

    private bool swinging = false;
    private bool canSwing = false;
    void Start()
    {
        
    }

    void FixedUpdate()
    {
        bool pressed = false;
        controller.inputDevice.IsPressed(controller.selectUsage, out pressed);

        bool released = !pressed && swinging;

        if (pressed && canSwing)
        {
            if (initialGrabFrame)
            {
                SwingingManager.instance.handGrabbed(this);
                initialGrabFrame = false;
                swinging = true;
            }
        }

        if (released)
        {
            SwingingManager.instance.handReleased(this);
            swinging = false;
        }
        
        else if (swinging)
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
    private void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.tag == "Swingable")
        {
            canSwing = true;
        }

    }

    private void OnTriggerExit(Collider col)
    {
        if (col.tag == "Swingable")
        {
            canSwing = false;
        }
    }
}
