using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerJump : MonoBehaviour
{
    private PlayerVault playerVault;
    private PlayerMovement playerMovement;
    // Start is called before the first frame update
    void Start()
    {
        playerVault = GetComponent<PlayerVault>();
        playerMovement = GetComponent<PlayerMovement>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    public void Jump()
    {
        playerMovement.AddVerticalVelocity(PlayerInfo.jumpForce);
        playerVault.TryVault();
    }

    public void WallrunJump()
    {
        // Get look direction & add an Additional Velocity vector in that direction
    }
}
