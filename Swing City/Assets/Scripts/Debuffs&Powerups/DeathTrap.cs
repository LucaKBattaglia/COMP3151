using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class DeathTrap : MonoBehaviour
{
    public Transform respawnPoint;  // The point where the player will respawn
    public GameObject BlackOut;      // UI element used for the blackout effect
    public PlayerMovement player;    // Reference to the player's movement script
    public Collider collider;
    Animator anim;
    PlayerCam cam;
    Color clr;
    public string fadeFrom;
    public string fadeTo;
    bool enable;

    void Start()
    {
        collider = GetComponent<Collider>();
        if(BlackOut == null) BlackOut = GameObject.Find("BlackOut");
        anim = BlackOut.GetComponent<Animator>();
        clr = BlackOut.GetComponent<RawImage>().color;
        player = GameObject.Find("Player").GetComponent<PlayerMovement>();
        cam = player.transform.Find("CameraUpdater/PlayerCamera").GetComponent<PlayerCam>();
        enable = true;
    }

    void FixedUpdate() {
        if(anim.GetCurrentAnimatorStateInfo(0).IsName("fadeTo")) {
            cam.freeze = true;
            player.rb.constraints = RigidbodyConstraints.FreezeAll;
            player.rb.velocity = Vector3.zero;
            player.rb.useGravity = false;
            player.grappleGun.StopSwing();
            player.grappleGun.StopGrapple();
            player.onPlatform = false; 
        }
        else if(anim.GetCurrentAnimatorStateInfo(0).IsName("fadeFrom") && !enable) {
            if(player.transform.position == respawnPoint.position) {
                player.rb.velocity = Vector3.zero;
                player.rb.constraints = RigidbodyConstraints.FreezeRotation;
                cam.freeze = false;
                player.gameObject.GetComponentInChildren<Collider>().enabled = true;
                enable = true;
                collider.enabled = true;
            }
            else {
                player.transform.position = respawnPoint.position;
            }
        }
        else {
            //
        }
    }

    void OnCollisionEnter(Collision other) {
        print(enable);
        if (other.gameObject.CompareTag("Player") && enable) {
            enable = false;
            print(enable);
            player.gameObject.GetComponentInChildren<Collider>().enabled = false;
            collider.enabled = false;
            StartCoroutine(HandlePlayerDeath()); // Start the process of fading to black and respawning 
        }
    }

    IEnumerator HandlePlayerDeath() {
        anim.Play(fadeTo);
        yield return new WaitForSeconds(0.5f);
        player.transform.position = respawnPoint.position;
        yield return new WaitForEndOfFrame();
        anim.Play(fadeFrom);
    }
}