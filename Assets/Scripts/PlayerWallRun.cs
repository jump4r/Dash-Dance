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

public class PlayerWallRun : MonoBehaviour
{

    private XRRig rig;
    private bool canWallRun = true;

    private HitFace hitFace = HitFace.None;
    // Start is called before the first frame update
    void Start()
    {
        rig = GetComponent<XRRig>();
    }

    private void Update()
    {
        if (Player.instance.moveState == PlayerMoveState.WALLRUNNING)
        {
            // Todo: Wallrunning code, need to whiteboard this
            // Player should move in a constant direction up the wall, reaching a max point, then going down.
        }
    }

    private void OnControllerColliderHit(ControllerColliderHit col)
    {

        Debug.Log(col.gameObject.tag);

        if (!canWallRun || col.gameObject.tag != "Wall")
        {
            return;
        }

        Quaternion headDir = Quaternion.Euler(0, rig.cameraGameObject.transform.eulerAngles.y, 0);
        RaycastHit hit;
        bool didHit = Physics.Raycast(transform.position, headDir * Vector3.forward, out hit, 5f);

        if (didHit)
        {
            hitFace = GetHitFace(hit);
        }

        Player.instance.SetMoveState(PlayerMoveState.WALLRUNNING);
    }

    private void OnTriggerExit(Collider col)
    {
        Player.instance.SetMoveState(PlayerMoveState.IDLE);
    }

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
