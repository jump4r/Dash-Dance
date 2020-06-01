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

    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<XRController>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!readyToClimb)
        {
            return;
        }

        bool pressed = false;
        controller.inputDevice.IsPressed(controller.selectUsage, out pressed);

        bool released = !pressed && isPressed;
        
        if (pressed)
        {
            if (initialGrabFrame)
            {
                ClimbingManager.instance.handGrabbed(this);
                initialGrabFrame = false;
                isPressed = true;
            }
        }

        else if (released)
        {
            ClimbingManager.instance.handReleased(this);
            isPressed = false;
        }

        else 
        {
            initialGrabFrame = true;
        }

        lastPosition = transform.localPosition;
    }

    void LateUpdate() 
    {
        delta = transform.localPosition - lastPosition;
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
}
