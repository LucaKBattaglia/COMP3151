using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    private float calculateSpeed;
    private float moveSpeed;
    public float walkSpeed;
    public float sprintSpeed;
    private float boostSpeed;
    public float maxSpeed;
    public float wallrunSpeed;
    public float acceleration;
    public float swingMultiplier;
    public bool activeGrapple;
    public bool activeSwing;
    public float groundDrag;
    public bool boosting;

    [Header("Jumping")]
    public float jumpForce;
    public float jumpCooldown;
    public float airMultiplier;
    bool readyToJump;

    [Header("Crouching")]
    public float crouchSpeed;
    public float crouchYScale;
    private float startYScale;

    [Header("Keybinds")]
    public KeyCode jumpKey = KeyCode.Space;
    public KeyCode sprintKey = KeyCode.LeftShift;
    public KeyCode crouchKey = KeyCode.LeftControl;

    [Header("Ground Check")]
    public float playerHeight;
    public LayerMask whatIsGround;
    bool grounded;

    [Header("Slope Handling")]
    public float maxSlopeAngle;
    public float minSlopeAngle;
    private RaycastHit slopeHit;
    private bool exitingSlope;

    [Header("Momentum")]
    public float swingDecceleration = 2f;

    [Header("Icy Momentum")]
    public float iceSpeedMultiplier = 1f;
    public float iceAccelMultiplier = 1f;
    private bool onIce = false;

    // Method called by the ice surface script to set the onIce status
    public void SetOnIce(bool isOnIce)
    {
        onIce = isOnIce;
    }
    
    public PlayerCam playerCam;
    public Transform playerObj;
    public Transform orientation;
    public Transform camPos;

    public GameObject curCheckpoint;

    public GrapplingGun grappleGun;

    float horizontalInput;
    float verticalInput;

    Vector3 moveDirection;

    public Rigidbody rb;

    public MovementState state;
    public enum MovementState
    {
        walking,
        sprinting,
        wallrunning,
        sliding,
        crouching,
        air
    }

    public bool sliding;
    public bool crouching;
    public bool wallrunning;
    public bool onPlatform;

    public fade fadeImg;
    public WallRunning wallRunScript;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        wallRunScript = GetComponent<WallRunning>();
        grappleGun = GameObject.Find("grappleGun").GetComponent<GrapplingGun>();
        rb.freezeRotation = true;
        gameObject.tag = "Player";
        transform.Find("playerObject").tag = "Player";

        readyToJump = true;

        startYScale = transform.localScale.y;
        activeSwing = false;
    }

    private void Update()
    {
        // ground check
        grounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.2f, whatIsGround);
        GetInput(); 
        StateHandler(); 

        if (Input.GetKeyDown(KeyCode.R))
        {
            transform.position = curCheckpoint.transform.position;
        }
    }
    
    private void FixedUpdate()
    {
        SpeedControl();
        MovePlayer();
   
        // handle drag
        if (grounded && !onIce) {
            
            rb.drag = groundDrag;
        }
        else
            rb.drag = 0;
    }


    private void GetInput()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");

        // when to jump
        if(Input.GetKey(jumpKey) && readyToJump && grounded)
        {
            readyToJump = false;

            Jump();

            Invoke(nameof(ResetJump), jumpCooldown);
        }

        // start crouch
        if (Input.GetKeyDown(crouchKey))
        {
            //camPos.transform.localScale = new Vector3(transform.localScale.x, crouchYScale, transform.localScale.z);
            rb.AddForce(Vector3.down * 5f, ForceMode.Impulse);
        }

        // stop crouch
        if (Input.GetKeyUp(crouchKey))
        {
            playerObj.transform.localScale = new Vector3(transform.localScale.x, startYScale, transform.localScale.z);
        }

        if(Input.GetKeyUp(KeyCode.W) && grounded) {
            boosting = false;
        }
    }

    private void StateHandler()
    {
        // Wallrunning
        if (wallrunning)
        {
            state = MovementState.wallrunning;
            calculateSpeed = wallrunSpeed;
        }

        // Mode - Crouching
        if (Input.GetKey(crouchKey))
        {
            playerObj.transform.localScale = new Vector3(transform.localScale.x, crouchYScale, transform.localScale.z);
            if(sliding) {
                state = MovementState.sliding;
            }
            else {
                state = MovementState.crouching;
                calculateSpeed = crouchSpeed;
            }
        }

        else if (boosting) {
            calculateSpeed = boostSpeed;
        }

        // Mode - Walking
        else if (grounded)
        {
            wallRunScript.wall = null;

            // Mode - Sprinting
            if(Input.GetKey(sprintKey)) {
                state = MovementState.sprinting;
                calculateSpeed = sprintSpeed;
            }
            else {
                state = MovementState.walking;
                calculateSpeed = walkSpeed;
            }
        }

        else if (activeSwing)
        {
            calculateSpeed = walkSpeed * swingMultiplier;
        }

        // Mode - Air
        else
        {
            state = MovementState.air;
            if (moveSpeed > walkSpeed)
            {
                calculateSpeed -= Time.deltaTime * swingDecceleration;
            }
            else
            {
                calculateSpeed = walkSpeed;
            }
        }

        if (onIce) moveSpeed = calculateSpeed * iceSpeedMultiplier;
        else moveSpeed = calculateSpeed;
    }

    private void MovePlayer()
    {
        // calculate movement direction
        moveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;

        // on slope
        if (OnSlope() && !exitingSlope)
        {
            if (onIce) rb.AddForce(GetSlopeMoveDirection(moveDirection) * moveSpeed * acceleration * iceAccelMultiplier * 2f, ForceMode.Force);
            else rb.AddForce(GetSlopeMoveDirection(moveDirection) * moveSpeed * acceleration * 2f, ForceMode.Force);

            if (rb.velocity.y > 0)
                rb.AddForce(Vector3.down * moveSpeed, ForceMode.Force);
        }

        // on ground
        else if(grounded) {
            if (onIce) rb.AddForce(moveDirection.normalized * moveSpeed * acceleration * iceAccelMultiplier, ForceMode.Force);
            else rb.AddForce(moveDirection.normalized * moveSpeed * acceleration, ForceMode.Force);
        }

        // in air
        else if(!grounded)
            rb.AddForce(moveDirection.normalized * moveSpeed * acceleration * airMultiplier, ForceMode.Force);

        // turn gravity off while on slope
        rb.useGravity = !OnSlope();
    }

private void SpeedControl()
{
    // limiting speed on slope
    if (OnSlope() && !exitingSlope)
    {
        if (rb.velocity.magnitude > moveSpeed)
        {
            rb.velocity = rb.velocity.normalized * moveSpeed;
        }
    }
    // limiting speed on ground or in air
    else
    {
        Vector3 flatVel = new Vector3(rb.velocity.x, 0f, rb.velocity.z); // Get only horizontal velocity

        // Limit velocity if the player is not on ice
        if (flatVel.magnitude > moveSpeed)
        {
            Vector3 limitedVel = flatVel.normalized * moveSpeed;
            rb.velocity = new Vector3(limitedVel.x, rb.velocity.y, limitedVel.z); // Preserve the y component for jumping

            StartCoroutine(ReduceSpeedGradually(flatVel));
            limitedVel = flatVel.normalized * moveSpeed;
            rb.velocity = new Vector3(limitedVel.x, rb.velocity.y, limitedVel.z);
        }

        else {
            StartCoroutine(ReduceSpeedGradually(flatVel));
        }
    }
    
}

// Coroutine to reduce the player's speed by 33% of the speed exceeding the moveSpeed over 3 seconds
private IEnumerator ReduceSpeedGradually(Vector3 initialFlatVel)
{
    float elapsedTime = 0f;
    float duration = 5f; // 3 seconds duration
    float initialExcessSpeed = initialFlatVel.magnitude - moveSpeed; // Calculate the amount of excess speed
    Vector3 direction = initialFlatVel.normalized; // Keep the direction of the initial movement

    while (elapsedTime < duration)
    {
        elapsedTime += Time.deltaTime;

        // Recalculate the flat velocity, this ensures we check for any updated input
        Vector3 flatVel = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        // If the player has stopped moving or changes direction, stop reducing speed
        if (flatVel.magnitude <= moveSpeed || flatVel.normalized != direction)
        {
            yield break; // Stop the coroutine
        }

        // Reduce speed by 33% of the excess speed over the target speed over 3 seconds
        float decelerationFactor = Mathf.Lerp(1f, 0.67f, elapsedTime / duration);
        float newSpeed = moveSpeed + initialExcessSpeed * decelerationFactor;

        // Clamp the speed so it does not go below moveSpeed
        newSpeed = Mathf.Max(newSpeed, moveSpeed);

        // Apply the decelerated velocity while preserving the y component (vertical velocity)
        rb.velocity = new Vector3(direction.x * newSpeed, rb.velocity.y, direction.z * newSpeed);

        yield return null; // Wait for the next frame
    }

    // After 3 seconds, make sure the velocity does not exceed moveSpeed
    rb.velocity = new Vector3(direction.x * moveSpeed, rb.velocity.y, direction.z * moveSpeed);
}


    private void Jump()
    {
        exitingSlope = true;

        // reset y velocity
        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
    }
    private void ResetJump()
    {
        readyToJump = true;

        exitingSlope = false;
    }

    public bool OnSlope()
    {
        if(Physics.Raycast(transform.position, Vector3.down, out slopeHit, playerHeight * 0.5f + 0.5f))
        {
            float angle = Vector3.Angle(Vector3.up, slopeHit.normal);
            return angle < maxSlopeAngle && angle >= minSlopeAngle;
        }

        return false;
    }

    public Vector3 GetSlopeMoveDirection(Vector3 dir)
    {
        return Vector3.ProjectOnPlane(dir, slopeHit.normal).normalized;
    }

    public void boost(Vector3 dir, int spd, Vector3 pos) {
        transform.position += pos;
        rb.AddForce(dir * spd, ForceMode.Impulse);
        StartCoroutine(boostTime());
    }

    public IEnumerator boostTime() {
        boosting = true;
        boostSpeed = maxSpeed;
        yield return null;
        while(moveSpeed > sprintSpeed) {
            print(moveSpeed);
            boostSpeed-=2;
            yield return new WaitForSeconds(1);
        }
        boosting = false;
    }

    //--------------------------------------------------------------------------------------//
    // Grappling Section
    //--------------------------------------------------------------------------------------//
    // variable for movement detection
    private bool enableMovementOnNextTouch;

    // variable for grapple jump scaling
    private float velocityXZScale;
    public void JumpToPosition(Vector3 targetPosition, float trajectoryHeight, float velocityXZScale)
    {
        activeGrapple = true;
        this.velocityXZScale = velocityXZScale;

        // use jumping velocity when player pulls the grapple to the point
        velocityToSet = CalculateJumpVelocity(transform.position, targetPosition, trajectoryHeight);
        Invoke(nameof(SetVelocity), 0.1f);

        // restore player controls after set duration
        Invoke(nameof(ResetRestrictions), grappleGun.grappleDuration);
    }

    // set velocity to rigidbody
    private Vector3 velocityToSet;
    private void SetVelocity()
    {
        enableMovementOnNextTouch = true;
        velocityToSet = new Vector3(velocityToSet.x * velocityXZScale, velocityToSet.y, velocityToSet.z * velocityXZScale);
        rb.velocity = velocityToSet;
    }

    public void ResetRestrictions()
    {
        activeGrapple = false;
    }

    // reset player controls when colliding
    private void OnCollisionEnter(Collision collision)
    {
        if (enableMovementOnNextTouch)
        {
            enableMovementOnNextTouch = false;
            ResetRestrictions();

            grappleGun.StopGrapple();
        }
    }

    // main math method for calculating jump velocity
    public Vector3 CalculateJumpVelocity(Vector3 startPoint, Vector3 endPoint, float trajectoryHeight)
    {
        float gravity = Physics.gravity.y;
        float displacementY = endPoint.y - startPoint.y;
        Vector3 displacementXZ = new Vector3(endPoint.x - startPoint.x, 0f, endPoint.z - startPoint.z);

        Vector3 velocityY = Vector3.up * Mathf.Sqrt(-2 * gravity * trajectoryHeight);
        Vector3 velocityXZ = displacementXZ / (Mathf.Sqrt(-2 * trajectoryHeight / gravity)
            + Mathf.Sqrt(2 * (displacementY - trajectoryHeight) / gravity));

        return velocityXZ + velocityY;
    }

}