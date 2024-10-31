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
    PlayerCam cam;
    Color clr;
    public string fadeFrom;
    public string fadeTo;
    bool enable;

    void Start()
    {
        if(BlackOut == null) BlackOut = GameObject.Find("BlackOut");
        anim = BlackOut.GetComponent<Animator>();
        clr = BlackOut.GetComponent<RawImage>().color;
        player = GameObject.Find("Player").GetComponent<PlayerMovement>();
        cam = player.transform.Find("CameraUpdater/PlayerCamera").GetComponent<PlayerCam>();
        enable = true;
    }

    void OnCollisionEnter(Collision other) {
        if (other.gameObject.CompareTag("Player") && enable) {
            StartCoroutine(HandlePlayerDeath()); // Start the process of fading to black and respawning 
            enable = false;
        }
    }

    IEnumerator HandlePlayerDeath() {
        cam.freeze = true;
        player.rb.velocity = Vector3.zero;
        player.rb.useGravity = false;
        player.grappleGun.StopSwing();
        player.grappleGun.StopGrapple();
        player.gameObject.GetComponentInChildren<Collider>().enabled = false;
        player.onPlatform = false; 
        player.enabled = false;

        yield return new WaitForEndOfFrame();
        anim.Play(fadeTo);
        yield return new WaitForSeconds(0.5f);
        player.gameObject.GetComponentInChildren<Collider>().enabled = true;
        player.rb.velocity = Vector3.zero;
        player.transform.position = respawnPoint.position;
        player.enabled = true;
        cam.freeze = false;
        yield return new WaitForEndOfFrame();

        anim.Play(fadeFrom);
        enable = true;
    }
}