using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

// Moves Player
public class ContinuousMovement : MonoBehaviour
{

    public XRNode inputSource;
    public XRNode secondaryInputSource;
    public float speed = 3f;
    public float gravity = -9.81f;
    private float verticalVelocity = 0;
    public float additionalHeight = 0.2f;
    public float jumpForce = 10f;
    public float rotationDegree = 45f;

    private Vector3 frameMovement;
    private XRRig rig;
    private Vector2 inputAxis;
    private Vector2 secondaryInputAxis;
    private bool jumpButton;
    
    public CharacterController character;
    private bool readyToSnapTurn = true;    
    
    // Start is called before the first frame update
    void Start()
    {
        character = GetComponent<CharacterController>();
        rig = GetComponent<XRRig>();
    }

    // Update is called once per frame
    void Update()
    {
        InputDevice device = InputDevices.GetDeviceAtXRNode(inputSource);
        InputDevice secondaryDevice = InputDevices.GetDeviceAtXRNode(secondaryInputSource);

        device.TryGetFeatureValue(CommonUsages.primary2DAxis, out inputAxis);
        secondaryDevice.TryGetFeatureValue(CommonUsages.primary2DAxis, out secondaryInputAxis);
        secondaryDevice.TryGetFeatureValue(CommonUsages.primaryButton, out jumpButton);
    }

    private void FixedUpdate()
    {
        if (ClimbingManager.instance.climbingHands.Count > 0)
        {
            ClimbingHand hand = ClimbingManager.instance.climbingHands[0];
            character.Move(new Vector3(-1 * hand.delta.x, -1 * hand.delta.y, -1 * hand.delta.z));
            return;
        }

        frameMovement = Vector3.zero;

        // Capsule follows the headset if player moves in real life
        CapsuleFollowHeadset();

        Quaternion headYaw = Quaternion.Euler(0, rig.cameraGameObject.transform.eulerAngles.y, 0);
        Vector3 direction = headYaw * new Vector3(inputAxis.x, 0, inputAxis.y);
        
        frameMovement += direction * Time.deltaTime * speed;

        // Calculate Vertical Velocity Due to Gravity
        if (CheckIsGrounded())
        {
            verticalVelocity = 0f;
        } else {
            verticalVelocity += gravity * Time.fixedDeltaTime;
        }

        // Handle Jump
        if (CheckIsGrounded() && jumpButton)
        {
            verticalVelocity += jumpForce;
        }

        frameMovement += new Vector3(0, verticalVelocity, 0);

        // Move and Rotate Character
        character.Move(frameMovement);
        RotateCharacter();
    }

    private void CapsuleFollowHeadset() {
        character.height = rig.cameraInRigSpaceHeight + additionalHeight;
        Vector3 capsuleCenter = transform.InverseTransformPoint(rig.cameraGameObject.transform.position);
        character.center = new Vector3(capsuleCenter.x, character.height / 2, capsuleCenter.z);
    }

    private bool CheckIsGrounded()
    {
        Vector3 rayStart = transform.TransformPoint(character.center);
        float rayLength = character.center.y + 0.01f;
        bool hasHit = Physics.SphereCast(rayStart, character.radius, Vector3.down, out RaycastHit hitInfo, rayLength);
        return hasHit;
    }

    private void RotateCharacter() 
    {
        Vector3 euler = transform.rotation.eulerAngles;

        if (secondaryInputAxis.x < 0f) 
        {
            if (readyToSnapTurn)
            {
                euler.y -= rotationDegree;
                readyToSnapTurn = false;
            } 
        } 
        
        else if (secondaryInputAxis.x > 0f)
        {
            if (readyToSnapTurn)
            {
                euler.y += rotationDegree;
                readyToSnapTurn = false;
            }
        }

        else 
        {
            readyToSnapTurn = true;
        }

        transform.rotation = Quaternion.Euler(euler);
    }
}
