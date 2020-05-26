using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class ClimbingHand : MonoBehaviour
{

    private XRController controller; 
    
    [SerializeField]
    private ContinuousMovement movementManager;

    // The position character was at when the grab was executed
    private Vector3 initialGrabPosition = Vector3.zero;
    private bool initialGrabFrame = true;

    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<XRController>();
    }

    // Update is called once per frame
    void Update()
    {
        bool pressed = false;
        controller.inputDevice.IsPressed(controller.selectUsage, out pressed);
        
        if (pressed)
        {
            if (initialGrabFrame) 
            {
                initialGrabFrame = false;
                initialGrabPosition = movementManager.transform.position;
            }
            // Ideally this should be line piece of code, but I need to let the 
            controller.climbing = true;
            Player.instance.SetMoveState(PlayerMoveState.CLIMBING);

            movementManager.transform.position += controller.handPositionDelta;

            Debug.Log("New Movement Manager Position: " + movementManager.transform.position);
            Debug.Log("Hnd Delta" + controller.handPositionDelta);
        }

        else 
        {
            controller.climbing = false;
            Player.instance.SetMoveState(PlayerMoveState.IDLE);
            initialGrabFrame = true;
        }
    }
}
