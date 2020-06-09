using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClimbingManager : MonoBehaviour
{
    // Start is called before the first frame update
    public static ClimbingManager instance;
    
    [SerializeField]
    private ContinuousMovement movementManager;

    public List<ClimbingHand> climbingHands;
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
    
    public void handGrabbed(ClimbingHand hand)
    {
        climbingHands.Add(hand);

        if (climbingHands.Count < 2)
        {
            Player.instance.SetMoveState(PlayerMoveState.CLIMBING);
        }
    }

    public void handReleased(ClimbingHand handToRemove)
    {
        foreach (ClimbingHand hand in climbingHands)
        {
            if (hand.name == handToRemove.name)
            {
                climbingHands.Remove(hand);
                break;
            }
        }

        if (climbingHands.Count == 0)
        {
            Player.instance.SetMoveState(PlayerMoveState.IDLE);
            Vector3 releaseVelocity = MomentumManager.instance.CalculateMomentumFromPositions();
            movementManager.SetAdditionalVelocity(releaseVelocity);
        }
    }

    public void UpdateClimbingHand(Vector3 delta)
    {
        movementManager.MoveClimbingPlayer(delta);
    }
}
