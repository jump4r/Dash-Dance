using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwingingManager : MonoBehaviour
{

     // Start is called before the first frame update
    public static SwingingManager instance;
    
    [SerializeField]
    private PlayerMovement movement;

    public List<SwingingHand> swingingHands;
    // Start is called before the first frame update
     void Start()
    {
        if (!instance)
        {
            instance = this;
        }

        else
        {
            Destroy(this.gameObject);
        }
    }
    
    public void handGrabbed(SwingingHand hand)
    {
        swingingHands.Add(hand);

        if (swingingHands.Count < 2)
        {
            Player.instance.SetMoveState(PlayerMoveState.SWINGING);
        }
    }

     public void handReleased(SwingingHand handToRemove)
    {
        foreach (SwingingHand hand in swingingHands)
        {
            if (hand.name == handToRemove.name)
            {
                swingingHands.Remove(hand);
                break;
            }
        }

        if (swingingHands.Count == 0)
        {
            Player.instance.SetMoveState(PlayerMoveState.IDLE);
        }
    }

    public void UpdateSwingingHand(Vector3 delta)
    {
        movement.MoveSwingingPlayer(delta);
    }

}
