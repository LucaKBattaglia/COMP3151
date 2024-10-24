using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonLock : MonoBehaviour
{
    public GameObject lockCube;   // Reference to the lock cube (lock1, lock2, or lock3)
    public Color unlockedColor = Color.green;  // Color when unlocked
    public Color lockedColor = Color.red;      // Initial color (locked)
    public bool isUnlocked = false;            // Status of the lock

    private Renderer buttonRenderer;
    private Renderer lockRenderer;
    private Material buttonMaterial;
    private Material lockMaterial;

    private void Start()
    {
        // Get the renderers for both the button and the lock
        buttonRenderer = GetComponent<Renderer>();
        lockRenderer = lockCube.GetComponent<Renderer>();

        // Get the materials of both the button and the lock (for emission changes)
        buttonMaterial = buttonRenderer.material;
        lockMaterial = lockRenderer.material;

        // Set initial colors and emission (red = locked)
        SetMaterialColorAndEmission(buttonMaterial, lockedColor);
        SetMaterialColorAndEmission(lockMaterial, lockedColor);
    }

    private void OnTriggerEnter(Collider other)
    {
        // Check if the player touches the switch (button)
        if (other.CompareTag("Player") && !isUnlocked)
        {
            // Unlock the button and the lock (turn them green)
            isUnlocked = true;
            SetMaterialColorAndEmission(buttonMaterial, unlockedColor);
            SetMaterialColorAndEmission(lockMaterial, unlockedColor);
        }
    }

    // Helper method to change both color and emission of the material
    private void SetMaterialColorAndEmission(Material material, Color color)
    {
        // Set the base color
        material.color = color;

        // Set the emission color (with the same color as base, you can tweak intensity here)
        material.SetColor("_EmissionColor", color * Mathf.LinearToGammaSpace(0.8f)); // Adjust the multiplier to make the emission more intense if needed

        // Enable emission (just in case it is not enabled)
        material.EnableKeyword("_EMISSION");
    }
}
