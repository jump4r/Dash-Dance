using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClimbingManager : MonoBehaviour
{
    // Start is called before the first frame update
    public static ClimbingManager instance;
    
    [SerializeField]
    private ContinuousMovement movementManager;
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
        movementManager.SetClimbingHand(hand);
    }

    public void handReleased()
    {
        movementManager.ClearClimbingHand();
    }
}
