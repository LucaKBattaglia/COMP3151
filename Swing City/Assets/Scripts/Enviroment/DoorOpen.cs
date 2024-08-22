using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenDoor : MonoBehaviour
{
    public GameObject DoorFrame;       // Reference to the DoorFrame object
    public GameObject InnerDoor;       // Reference to the InnerDoor object (child of DoorFrame)
    public float shrinkSpeed = 1f;     // Speed at which the InnerDoor shrinks
    public float growSpeed = 1f;       // Speed at which the InnerDoor grows back to its original size
    public float cooldownTime = 3f;    // Time in seconds before the door starts to grow back
    public string requiredKey = "Key1"; // The key required to shrink the door

    private bool shouldShrink = false; // Flag to start shrinking
    private bool shouldGrow = false;   // Flag to start growing
    private Vector3 originalScale;     // Original scale of the InnerDoor
    private Vector3 originalPosition;  // Original position of the InnerDoor
    private float doorCooldownTimer = 0f; // Timer to track the cooldown period

    private void Start()
    {
        // Store the original scale and position of the InnerDoor
        if (InnerDoor != null)
        {
            originalScale = InnerDoor.transform.localScale;
            originalPosition = InnerDoor.transform.localPosition;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        // Check if the object entering the collider is the player
        if (other.CompareTag("Player"))
        {
            // Check if the player has the required key
            if (GameManager.instance.HasKey(requiredKey))
            {
                // Start shrinking the InnerDoor
                shouldShrink = true;
                shouldGrow = false; // Stop growing if the player is inside

                // Reset the cooldown timer
                doorCooldownTimer = cooldownTime;
            }
            else
            {
                Debug.LogWarning("Player does not have " + requiredKey + ".");
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // Check if the object exiting the collider is the player
        if (other.CompareTag("Player"))
        {
            // Start the cooldown timer
            doorCooldownTimer = cooldownTime;
        }
    }

    private void Update()
    {
        // Handle shrinking of the InnerDoor
        if (shouldShrink && InnerDoor != null)
        {
            // Get the current local scale of the InnerDoor
            Vector3 currentScale = InnerDoor.transform.localScale;

            // Calculate the amount to shrink
            float shrinkAmount = shrinkSpeed * Time.deltaTime;

            // Shrink the door's Y scale over time
            if (currentScale.y > 0)
            {
                // Reduce the Y scale
                currentScale.y = Mathf.Max(0, currentScale.y - shrinkAmount);

                // Update the local scale of the InnerDoor
                InnerDoor.transform.localScale = currentScale;

                // Adjust the position to ensure the bottom stays fixed
                InnerDoor.transform.localPosition -= new Vector3(0, shrinkAmount * 0.5f, 0);
            }
            else
            {
                // Once the InnerDoor is fully shrunk, stop shrinking
                shouldShrink = false;
            }
        }

        // Handle growing of the InnerDoor
        if (shouldGrow && InnerDoor != null)
        {
            // Get the current local scale of the InnerDoor
            Vector3 currentScale = InnerDoor.transform.localScale;

            // Calculate the amount to grow
            float growAmount = growSpeed * Time.deltaTime;

            // Grow the door's Y scale over time
            if (currentScale.y < originalScale.y)
            {
                // Increase the Y scale
                currentScale.y = Mathf.Min(originalScale.y, currentScale.y + growAmount);

                // Update the local scale of the InnerDoor
                InnerDoor.transform.localScale = currentScale;

                // Adjust the position to ensure the bottom stays fixed
                InnerDoor.transform.localPosition += new Vector3(0, growAmount * 0.5f, 0);
            }
            else
            {
                // Once the InnerDoor is back to its original size, stop growing
                shouldGrow = false;
            }
        }

        // Handle cooldown timer
        if (!shouldShrink && !shouldGrow)
        {
            // Decrease the cooldown timer while the player is not inside
            if (doorCooldownTimer > 0)
            {
                doorCooldownTimer -= Time.deltaTime;
            }
            else if (doorCooldownTimer <= 0)
            {
                // Start growing the door back to its original size
                shouldGrow = true;
            }
        }
    }
}
