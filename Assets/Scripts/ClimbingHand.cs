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

    // The position character was at when the grab was executed
    private Vector3 initialGrabPosition = Vector3.zero;
    private Vector3 lastPosition = Vector3.zero;
    public Vector3 delta { get; private set; } = Vector3.zero;
    private bool initialGrabFrame = true;
    private bool isPressed = false;

    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<XRController>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        bool pressed = false;
        controller.inputDevice.IsPressed(controller.selectUsage, out pressed);
        lastPosition = transform.localPosition;
        
        if (pressed)
        {
            if (initialGrabFrame) 
            {
                movementManager.SetClimbingHand(this);
                initialGrabFrame = false;
                initialGrabPosition = transform.localPosition;
                isPressed = true;


            }
            Player.instance.SetMoveState(PlayerMoveState.CLIMBING);
        }

        bool released = !pressed && isPressed;
        if (released)
        {
            movementManager.ClearClimbingHand();
            isPressed = false;
        }

        else 
        {
            Player.instance.SetMoveState(PlayerMoveState.IDLE);
            initialGrabFrame = true;
        }
    }

    void LateUpdate() 
    {
        delta = transform.localPosition - lastPosition;
    }
}
