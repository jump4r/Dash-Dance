using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VaultHand : MonoBehaviour
{

    public PlayerMovement movement;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Vault" && Player.instance.moveState == PlayerMoveState.BEGIN_VAULT)
        {
            collision.gameObject.layer = 12; // Set To IgnorePlayer layer
            movement.Vault();
            Player.instance.SetMoveState(PlayerMoveState.VAULTING);
        }
    }
}
