using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class DeathTrap : MonoBehaviour
{
    public Transform respawnPoint;  // The point where the player will respawn
    public GameObject BlackOut;      // UI element used for the blackout effect
    public PlayerMovement player;    // Reference to the player's movement script
    Animator anim;
    public string fadeFrom;
    public string fadeTo;

    void Start()
    {
        anim = BlackOut.GetComponent<Animator>();
        player = GameObject.Find("Player").GetComponent<PlayerMovement>();
    }

    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            StartCoroutine(HandlePlayerDeath()); // Start the process of fading to black and respawning 
        }
    }

    IEnumerator HandlePlayerDeath()
    {
        player.rb.velocity = Vector3.zero;
        player.enabled = false;
        anim.Play(fadeTo);
        yield return new WaitForSeconds(1.5f);
        player.enabled = true;
        player.respawn(respawnPoint.position);
        anim.Play(fadeFrom);
    }
}