using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrapplingGun : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private LayerMask canGrapple; // What layer should it detect?
    [SerializeField] private Transform grappleTip; // Where will the tip of the grappling gun be at?
    [SerializeField] private Transform camera; // Connect to player camera
    
    [Header("Grapple Settings")]
    private bool isGrappling;
    [SerializeField] private float maxGrappleDistance = 100f; // Maximum distance of grappling
    public float grappleDuration = 1f; // How long grappling should go until you can control again?
    [SerializeField] private float grappleDelayTime; // How long grappling will reach?
    [SerializeField] private float overshootYAxis; // How high you should jump higher than the point?
    [Range(0.1f, 5f)] [SerializeField] private float overshootXZScale = 1f; // How far you should jump farther than the point? (Scaled)
    [SerializeField] private float grappleCooldown; // How long to wait until you can grapple again?
    private float grappleCooldownTimer;
    private Vector3 grapplePoint;

    [Header("Input")]
    [SerializeField] private KeyCode grappleKey = KeyCode.Mouse0; // Input key for grappling
    
    private PlayerMovement player;
    private LineRenderer lr;
    private Vector3 currentGrapplePosition;
    
    void Awake()
    {
        lr = GetComponent<LineRenderer>();
        lr.enabled = false;
    }
    
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player").GetComponent<PlayerMovement>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(grappleKey))
        {
            StartGrapple();
        }
        if (grappleCooldownTimer > 0f)
        {
            grappleCooldownTimer -= Time.deltaTime;
        }
    }

    // Called after Update
    void LateUpdate()
    {
        if (isGrappling)
            DrawRope();
    }

    private void StartGrapple()
    {
        // Prevent if cooldown is still in play
        if (grappleCooldownTimer > 0f) return;

        isGrappling = true;
        player.isFrozen = true; // Freeze the player
        grappleCooldownTimer = grappleCooldown; // Reset cooldown and stick it there until grappling has stopped

        // Raycast to shoot grapple. If miss, set point at the maximum distance of ray
        RaycastHit hit;
        if (Physics.Raycast(camera.position, camera.forward, out hit, maxGrappleDistance, canGrapple))
        {
            grapplePoint = hit.point;
            
            Invoke(nameof(ExecuteGrapple), grappleDelayTime); // execute grappling
        }
        else
        {
            grapplePoint = camera.position + camera.forward * maxGrappleDistance;

            Invoke(nameof(StopGrapple), grappleDelayTime); // stop grappling
        }

        lr.enabled = true;
        currentGrapplePosition = grappleTip.position;
    }

    private void ExecuteGrapple()
    {
        player.isFrozen = false; // unfreeze player

        Vector3 lowestPoint = new Vector3(transform.position.x, transform.position.y - 1f, transform.position.z); // set lowest point

        float grapplePointRelativeYPos = grapplePoint.y - lowestPoint.y; // get relative y position for grapple point
        float highestPointOnArc = grapplePointRelativeYPos + overshootYAxis; // get highest point on jumping arc

        if (grapplePointRelativeYPos < 0) highestPointOnArc = overshootYAxis; // if the grappling point is lower than the player, set it to offset instead

        player.JumpToPosition(grapplePoint, highestPointOnArc, overshootXZScale); // calculate jumping on player's side

        Invoke(nameof(StopGrapple), grappleDuration); // stop grappling line after a set duration
    }

    public void StopGrapple()
    {
        player.isFrozen = false; // unfreeze player
        isGrappling = false;
        lr.enabled = false;
    }

    void DrawRope()
    {
        currentGrapplePosition = Vector3.Lerp(currentGrapplePosition, grapplePoint, Time.deltaTime / grappleDelayTime); // draw grappling line to the point
        
        // Set grapple starting and ending points
        lr.SetPosition(0, grappleTip.position);
        lr.SetPosition(1, currentGrapplePosition);
    }

    // Capture whether you're grappling or not
    public bool IsGrappling()
    {
        return isGrappling;
    }

    // Get the grappling ending point
    public Vector3 GetGrapplePoint()
    {
        return grapplePoint;
    }
}
