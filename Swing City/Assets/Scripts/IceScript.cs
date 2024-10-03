using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceScript : MonoBehaviour
{
     public float slidingFriction = 0.05f; // Lower values = more sliding
    public float accelerationMultiplier = 2f; // Multiplies acceleration on ice

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Set onIce to true in the player's movement script
            PlayerMovement playerMovement = other.GetComponentInParent<PlayerMovement>();
            //print(playerMovement);
            if (playerMovement != null)
            {
                print("hello");
                playerMovement.SetOnIce(true);
                
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Set onIce to false when the player exits the ice
            PlayerMovement playerMovement = other.GetComponentInParent<PlayerMovement>();
            if (playerMovement != null)
            {
                print("goodbye");
                playerMovement.SetOnIce(false);
            }
        }
    }
}
