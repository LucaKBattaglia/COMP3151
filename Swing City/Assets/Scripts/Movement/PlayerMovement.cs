using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerMovement : MonoBehaviour
{
//------------------------------------------------------------------------------------------//
// Start of Paramaters Section
// The script is split up into seperate sections to help improve readability and identifying what section is in charge of what mechanics / functions.
//------------------------------------------------------------------------------------------//
    [Header("Player")] // Player RigidBody/Body
    Rigidbody rb;
//------------------------------------------------------------------------------------------//
    [Header("Movement")] // Movement speeds & direction 
    public float moveSpeed;
    private float setMoveSpeed;
    Vector3 moveDirection;
    [HideInInspector] public float walkSpeed;
    [HideInInspector] public float sprintSpeed;
    public Transform orientation;    
    public float groundDrag; // Drag | Air-Resistance when on ground | Friction
    public bool isFrozen;
    public bool activeGrapple;
//------------------------------------------------------------------------------------------//
    [Header("Keybinds")] // Keybinds & player controls
    public float jumpForce;
    public float jumpCooldown;
    public float airMultiplier;
    bool readyToJump;
//------------------------------------------------------------------------------------------//
    [Header("Keybinds")] // Keybinds & Player Input
    public KeyCode jumpKey = KeyCode.Space; // Jump button 
    float horizontalInput;
    float verticalInput;
//------------------------------------------------------------------------------------------//
    [Header("Ground Check")] // Controls: What is Ground, what surface is solid & am l currently on ground?
    public float playerHeight;
    public LayerMask whatIsGround;
    bool grounded;
    [HideInInspector] public TextMeshProUGUI text_speed;
//------------------------------------------------------------------------------------------//
// End of Paramaters Section
//------------------------------------------------------------------------------------------//

//------------------------------------------------------------------------------------------//
// Start of Functions Section
//------------------------------------------------------------------------------------------//

    //--------------------------------------------------------------------------------------//
    // Function Start & Update Section
    //--------------------------------------------------------------------------------------//
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;

        readyToJump = true;

        setMoveSpeed = moveSpeed;
    }

    private void Update()
    {
        // ground check
        grounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.2f, whatIsGround);

        MyInput();
        SpeedControl();

        // handle drag
        if (grounded)
            rb.drag = groundDrag;
        else
            rb.drag = 0;
    }

    private void FixedUpdate()
    {
        MovePlayer();
    }

    //--------------------------------------------------------------------------------------//
    // Player Controls & Input Section
    //--------------------------------------------------------------------------------------//
    private void MyInput()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");

        // can player currently jump
        if(Input.GetKey(jumpKey) && readyToJump && grounded)
        {
            readyToJump = false; // makes it so player cannot jump mid jump

            Jump();

            Invoke(nameof(ResetJump), jumpCooldown);
        }
    }

    //--------------------------------------------------------------------------------------//
    // Player Speed & Drag Section
    //--------------------------------------------------------------------------------------//
    private void MovePlayer()
    {
        if (activeGrapple) return;

        // calculate player movement direction
        moveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;

        // reset player movement speed
        moveSpeed = setMoveSpeed;

        // if player is frozen, stop them in place
        if (isFrozen)
        {
            moveSpeed = 0;
            rb.velocity = Vector3.zero;
        }
        
        // when player is on ground
        else if(grounded)
            rb.AddForce(moveDirection.normalized * moveSpeed * 10f, ForceMode.Force);

        // when player is in air
        else if(!grounded)
            rb.AddForce(moveDirection.normalized * moveSpeed * 10f * airMultiplier, ForceMode.Force);
    }
    private void SpeedControl()
    {
        if (activeGrapple) return;
        
        Vector3 flatVel = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        // limit velocity if needed
        if(flatVel.magnitude > moveSpeed)
        {
            Vector3 limitedVel = flatVel.normalized * moveSpeed;
            rb.velocity = new Vector3(limitedVel.x, rb.velocity.y, limitedVel.z);
        }

        //text_speed.SetText("Speed: " + flatVel.magnitude);
    }

    //--------------------------------------------------------------------------------------//
    // Jump Ability Section
    //--------------------------------------------------------------------------------------//
    private void Jump()
    {
        // reset y velocity
        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
    }

    private void ResetJump()
    {
        readyToJump = true;
    }

    //--------------------------------------------------------------------------------------//
    // Grappling Section
    //--------------------------------------------------------------------------------------//
    // variable for movement detection
    private bool enableMovementOnNextTouch;
    public void JumpToPosition(Vector3 targetPosition, float trajectoryHeight)
    {
        activeGrapple = true;

        // use jumping velocity when player pulls the grapple to the point
        velocityToSet = CalculateJumpVelocity(transform.position, targetPosition, trajectoryHeight);
        Invoke(nameof(SetVelocity), 0.1f);

        // restore player controls after set duration
        Invoke(nameof(ResetRestrictions), GameObject.Find("grappleGun").GetComponent<GrapplingGun>().grappleDuration);
    }

    // set velocity to rigidbody
    private Vector3 velocityToSet;
    private void SetVelocity()
    {
        enableMovementOnNextTouch = true;
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

            GameObject.Find("grappleGun").GetComponent<GrapplingGun>().StopGrapple();
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
//------------------------------------------------------------------------------------------//
// End of Functions Section
//------------------------------------------------------------------------------------------//

