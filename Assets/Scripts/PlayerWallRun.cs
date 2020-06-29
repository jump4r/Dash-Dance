using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

 public enum HitFace
    {
        None,
        Front,
        Back,
        Left,
        Right,
    }

public static class PlayerWallRunConstants {
    public const float vertMax = 0.1f;
    public const float vertDamp = -0.12f;
    public const float forwardVel = 3f;
}

public class PlayerWallRun : MonoBehaviour
{

    private XRRig rig;
    private PlayerMovement movement;
    private bool canWallRun = true;

    private HitFace hitFace = HitFace.None;
    // Start is called before the first frame update
    void Start()
    {
        rig = GetComponent<XRRig>();
        movement = GetComponent<PlayerMovement>();
    }

    private void Update()
    {
        if (Player.instance.moveState == PlayerMoveState.WALLRUNNING)
        {
            // Todo: Wallrunning code, need to whiteboard this
            // Player should move in a constant direction up the wall, reaching a max point, then going down.
            movement.Wallrun();
        }
    }

    private void OnControllerColliderHit(ControllerColliderHit col)
    {
        if (!canWallRun || col.gameObject.tag != "Wall")
        {
            return;
        }

        // Only want to call this once, when player starts wallrunning
        else if (Player.instance.moveState == PlayerMoveState.WALLRUNNING) {
            return;
        }

        Quaternion headDir = Quaternion.Euler(0, rig.cameraGameObject.transform.eulerAngles.y, 0);
        RaycastHit hit;
        bool didHit = Physics.Raycast(transform.position, headDir * Vector3.forward, out hit, 5f);

        if (didHit)
        {
           StartWallrun(hit, headDir);
        }
    }

    private void StartWallrun(RaycastHit hit, Quaternion headDir)
    {
        Vector3 lookVec = headDir * Vector3.forward;
        Vector3 normalVec = hit.normal * -1f;

        float collisionAngle = Vector3.Cross(lookVec.normalized, normalVec.normalized).y;
        collisionAngle = Mathf.RoundToInt(collisionAngle * 180); 

        Player.instance.SetMoveState(PlayerMoveState.WALLRUNNING);
        movement.StartWallrun();
    }


    // To remove
    private HitFace GetHitFace(RaycastHit hit)
    {
        
        Vector3 incommingVec = hit.normal - Vector3.up;

        if (incommingVec == new Vector3(0, -1, -1))
        {
            return HitFace.Back;
        }

        if (incommingVec == new Vector3(0, -1, 1))
        {
            return HitFace.Front;
        }

        if (incommingVec == new Vector3(-1, -1, 0))
        {
            return HitFace.Left;
        }

        if (incommingVec == new Vector3(1, -1, 0))
        {
            return HitFace.Right;
        }

        return HitFace.None;
    }
}
