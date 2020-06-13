using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerVault : MonoBehaviour
{

    public bool canVault { get; private set; } = false;
    // Start is called before the first frame update
    private void OnTriggerEnter(Collider col)
    {
        if (col.tag == "VaultEntry")
        {
            canVault = true;
        }
    }

    private void OnTriggerExit(Collider col)
    {
        if (col.tag == "VaultEntry")
        {
            canVault = false;
        }
    }

    public void TryVault()
    {
        if (canVault)
        {
            Debug.Log("Start Vaulting");
            Player.instance.SetMoveState(PlayerMoveState.BEGIN_VAULT);
        }
    }
}
