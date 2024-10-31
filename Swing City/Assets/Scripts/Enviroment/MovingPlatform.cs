using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    public bool loop; // Whether the platform goes back and forth between points or just respawns at the start point
    public float spd; // How fast the platform moves
    public float waitTime; // time to wait after reaching points
    public float waitTimer; // the timer used to count down the wait time
    public Vector3 moveDir; // The direction the platform is currently moving
    public Vector3 startP; // The starting position of the platform
    public Vector3 endP; // The ending position
    public Vector3 curP; // The current point to move to
    public Transform[] points; // In case we ever want to do complex moving platforms
    private bool arrDir; // The direction the point array is being looped through (true is ->, false is <-)
    private int curInd; // The current index corresponding to curP
    private bool playerOnPlatform;
    private Vector3 lastPlatformPosition; // Store the platform's position in the last frame
    private Rigidbody playerRb; // Reference to the player's Rigidbody

    private PlayerMovement player;

    void Start() {
        int p = transform.GetSiblingIndex() + 1;
        Transform pObj = transform.parent.GetChild(p);
        points = new Transform[pObj.childCount];
        player = GameObject.Find("Player").GetComponent<PlayerMovement>();
        for (int i = 0; i < pObj.childCount; i++) points[i] = pObj.GetChild(i);
        init();
        lastPlatformPosition = transform.position; // Initialize last position
        waitTimer = waitTime;
    }

    void init() {
        startP = points[0].position; // StartP is the first child, so automatically grab that
        endP = points[points.Length - 1].position; // Ditto for endP but at the last position
        curInd = 1; // Current point to move to is the second child
        curP = points[curInd].position; // Get that child
        arrDir = true; // Loop forwards through array
        transform.position = startP; // Set position to starting point
        moveDir = (curP - transform.position).normalized; // Set direction
    }

    void FixedUpdate() {
        if(!player.onPlatform) {
            playerOnPlatform = false;
        }
        // Move the platform towards the current point
        if (Vector3.Distance(curP, transform.position) < 0.2f) {
                if (curP == endP || curP == startP) {
                    if(waitTimer <= 0) {
                        if (loop) {
                            arrDir = !arrDir; // Flip the array direction
                            moveArr(arrDir); // Increment arrDir (so that we're not on endP or startP)
                            curP = points[curInd].position; // Get the next point to move to
                            moveDir = (curP - transform.position).normalized; // And get the direction
                        }
                        else {
                            init(); // If not looping, just reset the platform
                        }
                        waitTimer = waitTime;
                    }
                    else {
                        waitTimer -= Time.deltaTime;
                    }
                }
                else {
                    moveArr(arrDir); // Move to the next point
                    curP = points[curInd].position; // Get that point
                    moveDir = (curP - transform.position).normalized; // And get the direction
                }
        }
        else {
            transform.Translate(moveDir * Time.deltaTime * spd); // Move the platform
        }

        // Adjust the player's position without affecting their input movement
        if (playerOnPlatform && playerRb != null)
        {
            Vector3 platformMovement = transform.position - lastPlatformPosition;

            // Directly position the player to avoid pushing them off
            playerRb.MovePosition(playerRb.position + new Vector3(platformMovement.x, platformMovement.y, platformMovement.z));
        }

        lastPlatformPosition = transform.position; // Update last platform position for the next frame
    }

    void moveArr(bool d)
    {
        curInd = d ? curInd + 1 : curInd - 1;
    }

    // When the player enters the platform's collider, start tracking them
    private void OnCollisionEnter(Collision other)
    {
        if (other.transform.CompareTag("Player"))
        {
            player.onPlatform = true;
            playerOnPlatform = true;
            playerRb = other.transform.GetComponent<Rigidbody>(); // Store the player's Rigidbody reference
        }
    }

    // When the player exits the platform's collider, stop tracking them
    private void OnCollisionExit(Collision other)
    {
        if (other.transform.CompareTag("Player"))
        {
            player.onPlatform = false;
            playerOnPlatform = false;
            playerRb = null; // Reset player reference
        }
    }
}
