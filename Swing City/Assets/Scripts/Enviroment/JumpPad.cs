using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpPad : MonoBehaviour
{
    [Header("Settings")]
    [Range(1f, 10f)] [SerializeField] private float jumpBoostMultiplier = 2f; // How much should it multiply the jumping force of the player
    
    private PlayerMovement player;
    private float playerJumpForce;
    
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player").GetComponent<PlayerMovement>();
        playerJumpForce = player.jumpForce;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // If the player is on the pad, multiply the jumping force
    void OnTriggerStay(Collider collider)
    {
        if (collider.tag == "Player")
        {
            player.jumpForce = playerJumpForce * jumpBoostMultiplier;
        }
    }

    // If the player leaves the pad, set the jump force back to normal
    void OnTriggerExit(Collider collider)
    {
        if (collider.tag == "Player")
        {
            player.jumpForce = playerJumpForce;
        }
    }
}
