using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

// Moves Player
public static class PlayerInfo
{
    public static float additionalHeight = 0.2f;
    public static float speed = 3f;
    public static float gravity = -0.2f;
    public static float jumpForce = 0.05f;
    public static float rotationDegree = 45f;
    public static float vaultSpeed = 5f;
}

public class PlayerMovement : MonoBehaviour
{

    public XRNode inputSource;
    public XRNode secondaryInputSource;
    private float verticalVelocity = 0;
    private Vector3 additionalVelocity = Vector3.zero;

    private Vector3 frameMovement;
    private XRRig rig;
    private Vector2 inputAxis;
    private Vector2 secondaryInputAxis;

    // Jump Variables
    private bool jumpButton;
    
    public CharacterController character;
    private PlayerVault playerVault;

    // Rotation
    private bool readyToSnapTurn = true;    
    
    // Start is called before the first frame update
    void Start()
    {
        character = GetComponent<CharacterController>();
        playerVault = GetComponent<PlayerVault>();
        rig = GetComponent<XRRig>();
    }

    private void Update()
    {
        InputDevice device = InputDevices.GetDeviceAtXRNode(inputSource);
        InputDevice secondaryDevice = InputDevices.GetDeviceAtXRNode(secondaryInputSource);

        device.TryGetFeatureValue(CommonUsages.primary2DAxis, out inputAxis);
        secondaryDevice.TryGetFeatureValue(CommonUsages.primary2DAxis, out secondaryInputAxis);
        secondaryDevice.TryGetFeatureValue(CommonUsages.primaryButton, out jumpButton);

        frameMovement = Vector3.zero;

        if (Player.instance.moveState == PlayerMoveState.CLIMBING)
        {
            // Movement Handled by MoveClimbingPlayer, which is called from ClimbingManager.
            return;
        }

        else if (Player.instance.moveState == PlayerMoveState.SWINGING)
        {
            // Movement Handled By MoveSwingingPlayer
            return;
        }

        // Capsule follows the headset if player moves in real life
        CapsuleFollowHeadset();

        Quaternion headYaw = Quaternion.Euler(0, rig.cameraGameObject.transform.eulerAngles.y, 0);
        Vector3 direction = headYaw * new Vector3(inputAxis.x, 0, inputAxis.y);
        
        frameMovement += direction * Time.deltaTime * PlayerInfo.speed;

        // Calculate Vertical Velocity Due to Gravity
        if (CheckIsGrounded())
        {
            verticalVelocity = 0f;
            additionalVelocity = Vector3.zero;
        } else {
            verticalVelocity += PlayerInfo.gravity * Time.fixedDeltaTime;
        }

        // Handle Jump
        if (CheckIsGrounded() && jumpButton)
        {
            Jump();
        }

        frameMovement += new Vector3(0, verticalVelocity, 0);

        // Move any additional Velocity as determined by other Managers
        frameMovement += additionalVelocity;

        // Move and Rotate Character
        character.Move(frameMovement);

        RotateCharacter();
    }

    private void CapsuleFollowHeadset() {
        character.height = rig.cameraInRigSpaceHeight + PlayerInfo.additionalHeight;
        Vector3 capsuleCenter = transform.InverseTransformPoint(rig.cameraGameObject.transform.position);
        character.center = new Vector3(capsuleCenter.x, character.height / 2, capsuleCenter.z);
    }

    private bool CheckIsGrounded()
    {
        Vector3 rayStart = transform.TransformPoint(character.center);
        float rayLength = character.center.y + 0.01f;
        int floorMask = LayerMask.GetMask("Floor");
        
        bool hasHit = Physics.SphereCast(rayStart, character.radius, Vector3.down, out RaycastHit hitInfo, rayLength, floorMask);

        if (hasHit && Player.instance.moveState == PlayerMoveState.IDLE)
        {
            Player.instance.SetMoveState(PlayerMoveState.IDLE);
        }

        return hasHit;
    }

    private void RotateCharacter() 
    {
        Vector3 euler = transform.rotation.eulerAngles;

        if (secondaryInputAxis.x < -0.5f || secondaryInputAxis.x > 0.5f) 
        {
            if (readyToSnapTurn)
            {
                euler.y = euler.y + (secondaryInputAxis.x > 0 ? PlayerInfo.rotationDegree : PlayerInfo.rotationDegree * -1f);
                readyToSnapTurn = false;
            } 
        } 

        else 
        {
            readyToSnapTurn = true;
        }

        transform.rotation = Quaternion.Euler(euler);
    }

    private void Jump()
    {
        verticalVelocity += PlayerInfo.jumpForce;
        playerVault.TryVault();
    }

    public void Vault()
    {
        Quaternion headYaw = Quaternion.Euler(0, rig.cameraGameObject.transform.eulerAngles.y, 0);
        Vector3 vaultVel = (headYaw * Vector3.forward) * PlayerInfo.vaultSpeed * Time.deltaTime;
        SetAdditionalVelocity(vaultVel);
    }

    public void SetAdditionalVelocity(Vector3 velocity)
    {
        additionalVelocity = velocity;
    }

    public void MoveClimbingPlayer(Vector3 delta)
    {
        frameMovement = delta;
        character.Move(frameMovement);
    }

    public void MoveSwingingPlayer(Vector3 delta)
    {
        frameMovement = delta;
        character.Move(frameMovement);
    }
}
