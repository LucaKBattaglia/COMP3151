using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceScript : MonoBehaviour
{
    void OnCollisionEnter(Collision other)
    {
        if (other.collider.CompareTag("Player"))
        {
            // Set onIce to true in the player's movement script
            PlayerMovement playerMovement = other.collider.GetComponentInParent<PlayerMovement>();
            //print(playerMovement);
            if (playerMovement != null)
            {
                print("hello");
                playerMovement.SetOnIce(true);
                
            }
        }
    }

    void OnCollisionExit(Collision other)
    {
        if (other.collider.CompareTag("Player"))
        {
            // Set onIce to false when the player exits the ice
            PlayerMovement playerMovement = other.collider.GetComponentInParent<PlayerMovement>();
            if (playerMovement != null)
            {
                print("goodbye");
                playerMovement.SetOnIce(false);
            }
        }
    }
}
