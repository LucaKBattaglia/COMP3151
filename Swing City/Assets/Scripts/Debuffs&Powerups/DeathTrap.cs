using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class DeathTrap : MonoBehaviour
{
    public Transform respawnPoint;  // The point where the player will respawn
    public GameObject BlackOut;      // UI element used for the blackout effect
    public PlayerMovement player;    // Reference to the player's movement script

    void Start()
    {
        player = GameObject.Find("Player").GetComponent<PlayerMovement>();
    }

    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            StartCoroutine(HandlePlayerDeath()); // Start the process of fading to black and respawning 
        }
    }

    private IEnumerator HandlePlayerDeath()
    {
        // Re-enable player movement
        SetPlayerMovement(false);
        // First, fade the screen to black
        yield return StartCoroutine(FadeBlackOut(true));

        // After the screen is fully black, move the player to the respawn point
        yield return StartCoroutine(RespawnPlayer());

        // Then, fade the screen back to normal
        yield return StartCoroutine(FadeBlackOut(false));
        SetPlayerMovement(true);
    }

    private IEnumerator RespawnPlayer()
    {
        
        yield return new WaitForSeconds(0.3f);
        // Move the player to the respawn point
        player.respawn(respawnPoint.position);
        yield return null; // Ensure the coroutine completes
    }

     private void SetPlayerMovement(bool canMove)
    {
        player.canMove = canMove;
    }

    public IEnumerator FadeBlackOut(bool fadeToBlack, int fadeSpeed = 1)
    {
        Color objectColor = BlackOut.GetComponent<Image>().color;
        float fadeAmount;
       
        if (fadeToBlack)
        {
            while (BlackOut.GetComponent<Image>().color.a < 1)
            {
                fadeAmount = objectColor.a + (fadeSpeed * Time.deltaTime);

                objectColor = new Color(objectColor.r, objectColor.g, objectColor.b, fadeAmount);
                BlackOut.GetComponent<Image>().color = objectColor;
                yield return null;
            }
        }

        else
        {
            
            while (BlackOut.GetComponent<Image>().color.a > 0)
            {
                fadeAmount = objectColor.a - (fadeSpeed * Time.deltaTime);

                objectColor = new Color(objectColor.r, objectColor.g, objectColor.b, fadeAmount);
                BlackOut.GetComponent<Image>().color = objectColor;
                yield return null;
            }
            yield return new WaitForEndOfFrame();
        }
        yield return new WaitForEndOfFrame();
    }
}