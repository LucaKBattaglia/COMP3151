using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
<<<<<<< Updated upstream
    int DEFAULT;
    public bool loop; // whether platform goes back and forth between points or just respawns at the start point
    public int spd; // how fast the platform moves
    public Vector3 moveDir; // the direction the platform is currently moving
    public Vector3 startP; // the starting position of the platform
    public Vector3 endP; // the ending position
    public Vector3 curP; // the current point to move to
    public Transform[] points; // in case we ever wanna do complex moving platforms
    public PlayerMovement pm;
    bool arrDir; // the direction the point array is being looped through (true is ->, false is <-)
    int curInd; // the current index corresponding to curP
=======
    public bool loop; // Whether the platform goes back and forth between points or just respawns at the start point
    public float spd; // How fast the platform moves
    public Vector3 moveDir; // The direction the platform is currently moving
    public Vector3 startP; // The starting position of the platform
    public Vector3 endP; // The ending position
    public Vector3 curP; // The current point to move to
    public Transform[] points; // In case we ever want to do complex moving platforms
    private bool arrDir; // The direction the point array is being looped through (true is ->, false is <-)
    private int curInd; // The current index corresponding to curP
>>>>>>> Stashed changes

    private Vector3 lastPlatformPosition; // Store the platform's position in the last frame
    private bool playerOnPlatform; // Whether the player is on the platform
    private Rigidbody playerRb; // Reference to the player's Rigidbody

    void Start()
    {
        int p = transform.GetSiblingIndex() + 1;
        Transform pObj = transform.parent.GetChild(p);
        points = new Transform[pObj.childCount];
<<<<<<< Updated upstream
        for(int i = 0; i < pObj.childCount; i++) points[i] = pObj.GetChild(i);
        DEFAULT = transform.childCount;
=======
        for (int i = 0; i < pObj.childCount; i++) points[i] = pObj.GetChild(i);
>>>>>>> Stashed changes
        init();
        lastPlatformPosition = transform.position; // Initialize last position
    }

<<<<<<< Updated upstream
    void init() {
        startP = points[0].position; // startP is the first child, so automatically grab that
        endP = points[points.Length-1].position; // ditto for endP but at the last position
        curInd = 1; // current point to move to is the second child
        curP = points[curInd].position;  // get that child
        arrDir = true; // loop forwards through array
        transform.position = startP; // set position to starting point
        moveDir = (curP - transform.position).normalized; // set direction
    }

    void FixedUpdate() {
        if(Vector3.Distance(curP, transform.position) < 0.1f) { // if close enough to the current point
            if(curP == endP || curP == startP) { // if at the end or start 
                if(loop) { // if looping
                    arrDir = !arrDir; //  flip the array direction
                    moveArr(arrDir); // increment arrDir (so that we're not on endP or startP)
                    curP = points[curInd].position; // get the next point to move to
                    moveDir = (curP-transform.position).normalized; // and get the direction
                }
                else {
                    if(transform.childCount > DEFAULT) {
                        removePlayer(transform.GetChild(0).transform);
                    }
                    init(); // if not looping, just reset the platform
                }
            }
            else { // if not at either end (meaning we're at a midpoint)
                moveArr(arrDir); // move to the next point
                print(curInd);
                curP = points[curInd].position; // get that point
                moveDir = (curP-transform.position).normalized; // and get the direction
            }
        }
        else { // otherwise move in the necessary direction
            transform.Translate(moveDir * Time.deltaTime * spd);
        }
    }

    void moveArr(bool d) { // input false for subtraction, true for addition
        curInd = d ? curInd+1 : curInd-1;
    }

    void removePlayer(Transform other) {
        other.transform.SetParent(null);
        pm = null;
    }

    private void OnCollisionEnter(Collision other) {
        if(other.transform.tag == "Player") {
            other.transform.SetParent(transform);
            pm = other.transform.GetComponent<PlayerMovement>();
        }
    }

    private void OnCollisionExit(Collision other) {
        if(other.transform.tag == "Player") {
            removePlayer(other.transform);
=======
    void init()
    {
        startP = points[0].position; // StartP is the first child, so automatically grab that
        endP = points[points.Length - 1].position; // Ditto for endP but at the last position
        curInd = 1; // Current point to move to is the second child
        curP = points[curInd].position; // Get that child
        arrDir = true; // Loop forwards through array
        transform.position = startP; // Set position to starting point
        moveDir = (curP - transform.position).normalized; // Set direction
    }

    void FixedUpdate()
    {
        // Move the platform towards the current point
        if (Vector3.Distance(curP, transform.position) < 0.1f)
        {
            if (curP == endP || curP == startP)
            {
                if (loop)
                {
                    arrDir = !arrDir; // Flip the array direction
                    moveArr(arrDir); // Increment arrDir (so that we're not on endP or startP)
                    curP = points[curInd].position; // Get the next point to move to
                    moveDir = (curP - transform.position).normalized; // And get the direction
                }
                else
                {
                    init(); // If not looping, just reset the platform
                }
            }
            else
            {
                moveArr(arrDir); // Move to the next point
                curP = points[curInd].position; // Get that point
                moveDir = (curP - transform.position).normalized; // And get the direction
            }
        }
        else
        {
            transform.Translate(moveDir * Time.deltaTime * spd); // Move the platform
        }

        // Adjust the player's position without affecting their input movement
        if (playerOnPlatform && playerRb != null)
        {
            Vector3 platformMovement = transform.position - lastPlatformPosition;

            // Directly position the player to avoid pushing them off
            playerRb.MovePosition(playerRb.position + new Vector3(platformMovement.x, 0, platformMovement.z));
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
            playerOnPlatform = true;
            playerRb = other.transform.GetComponent<Rigidbody>(); // Store the player's Rigidbody reference
        }
    }

    // When the player exits the platform's collider, stop tracking them
    private void OnCollisionExit(Collision other)
    {
        if (other.transform.CompareTag("Player"))
        {
            playerOnPlatform = false;
            playerRb = null; // Reset player reference
>>>>>>> Stashed changes
        }
    }
}
