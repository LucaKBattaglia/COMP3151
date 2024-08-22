using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CubeColorChanger : MonoBehaviour
{
    private Renderer cubeRenderer;

    void Start()
    {
        // Get the Renderer component attached to the cube
        cubeRenderer = GetComponent<Renderer>();
    }

    // Detect collisions with the player (using OnCollisionEnter)
    // private void OnCollisionEnter(Collision collision)
    // {
    //     // Check if the object collided with has the tag "Player"
    //     if (collision.gameObject.CompareTag("Player"))
    //     {
    //         // Check if the player has collected Key1
    //         if (GameManager.instance.HasKey("Key1"))
    //         {
    //             // Change the cube's color to green if Key1 is collected
    //             cubeRenderer.material.color = Color.green;
    //         }
    //         else
    //         {
    //             // Change the cube's color to red if Key1 is not collected
    //             cubeRenderer.material.color = Color.red;
    //         }
    //     }
    // }

    // Alternatively, use OnTriggerEnter if using triggers
    private void OnTriggerEnter(Collider other)
    {
        // Check if the object entered has the tag "Player"
        if (other.CompareTag("Player"))
        {
            // Check if the player has collected Key1
            if (GameManager.instance.HasKey("Key1"))
            {
                // Change the cube's color to green if Key1 is collected
                cubeRenderer.material.color = Color.green;
                SceneManager.LoadScene("MainMenu");
            }
            else
            {
                // Change the cube's color to red if Key1 is not collected
                cubeRenderer.material.color = Color.red;
            }
        }
    }

}
