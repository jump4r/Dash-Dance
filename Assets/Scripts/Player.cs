using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlayerMoveState {
    IDLE,
    CLIMBING,
    BEGIN_VAULT,
    VAULTING,
    SWINGING,
    WALLRUNNING,
}
public class Player : MonoBehaviour
{
    // Start is called before the first frame update
    public static Player instance = null;

    public PlayerMoveState moveState { get; private set; } = PlayerMoveState.IDLE;
    void Awake()
    {
        if (instance == null) 
        {
            instance = this;
        }

        else
        {
            Destroy(this.gameObject);
        }
        
    }

    public void SetMoveState(PlayerMoveState newState)
    {
        moveState = newState;
    }
}
