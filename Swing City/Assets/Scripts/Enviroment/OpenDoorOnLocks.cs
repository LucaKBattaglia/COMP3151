using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenDoorOnLocks : MonoBehaviour
{
    public GameObject InnerDoor;       // Reference to the InnerDoor object (child of DoorFrame)
    public float shrinkSpeed = 1f;     // Speed at which the InnerDoor shrinks
    public ButtonLock lock1;           // Reference to lock1
    public ButtonLock lock2;           // Reference to lock2
    public ButtonLock lock3;           // Reference to lock3

    private bool shouldShrink = false; // Flag to start shrinking
    private Vector3 originalScale;     // Original scale of the InnerDoor

    private void Start()
    {
        // Store the original scale of the InnerDoor
        if (InnerDoor != null)
        {
            originalScale = InnerDoor.transform.localScale;
        }
    }

    private void Update()
    {
        // Check if all locks are unlocked
        if (AllLocksUnlocked())
        {
            shouldShrink = true;
        }

        // Handle shrinking of the InnerDoor
        if (shouldShrink && InnerDoor != null)
        {
            Vector3 currentScale = InnerDoor.transform.localScale;
            float shrinkAmount = shrinkSpeed * Time.deltaTime;

            if (currentScale.y > 0)
            {
                currentScale.y = Mathf.Max(0, currentScale.y - shrinkAmount);
                InnerDoor.transform.localScale = currentScale;
                InnerDoor.transform.localPosition -= new Vector3(0, shrinkAmount * 0.5f, 0);
            }
            else
            {
                shouldShrink = false;
            }
        }
    }

    // Method to check if all locks are unlocked
    private bool AllLocksUnlocked()
    {
        return lock1.isUnlocked && lock2.isUnlocked && lock3.isUnlocked;
    }
}
