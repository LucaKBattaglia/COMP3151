using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GrapplingGun : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private LayerMask canGrapple; // What layer should it detect?
    [SerializeField] private Transform grappleTip; // Where will the tip of the grappling gun be at?
    [SerializeField] private Transform cam; // Connect to player camera
    [SerializeField] private Renderer grapplePanel; // Indication panel for grappling
    private Color grapplePanelColor;
    [SerializeField] private Renderer swingPanel; // Indication panel for swinging
    private Color swingPanelColor;
    
    [Header("Grapple Settings")]
    [SerializeField] private float maxGrappleDistance = 100f; // Maximum distance of grappling
    public float grappleDuration = 1f; // How long grappling should go until you can control again?
    [SerializeField] private float grappleDelayTime; // How long grappling will reach?
    [SerializeField] private float overshootYAxis; // How high you should jump higher than the point?
    [SerializeField] private float overshootXZScale = 1f; // How far you should jump farther than the point? (Scaled)
    [SerializeField] private float grappleCooldown; // How long to wait until you can grapple again?
    private bool isGrappling;
    private float grappleCooldownTimer;
    private Vector3 grapplePoint;

    [Header("Swing Settings")]
    [SerializeField] private float maxSwingDistance = 50f; // Maximum distance of swinging
    [SerializeField] private float swingDelayTime; // How long swinging will reach?
    [SerializeField] private float minJointDistanceScale = 0.25f; // Minimum distance of joint (Scaled by point to distance)
    [SerializeField] private float maxJointDistanceScale = 0.8f; // Maximum distance of joint (Scaled by point to distance)
    [SerializeField] private float jointSpring = 4.5f; // Spring of joint
    [SerializeField] private float jointDamper = 7f; // Damper of joint
    [SerializeField] private float jointMassScale = 4.5f; // Mass scale of joint
    [SerializeField] private float swingCooldown; // How long to wait until you can swing again?
    private bool isSwinging;
    private float swingCooldownTimer;
    private SpringJoint joint;

    [Header("Input")]
    [SerializeField] private KeyCode grappleKey = KeyCode.Mouse1; // Input key for grappling
    [SerializeField] private KeyCode swingKey = KeyCode.Mouse0; // Input key for swinging

    [Header("Colours")]
    [SerializeField] private Color grappleColour = Color.red;
    [SerializeField] private Color swingColour = Color.blue;
    
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
        grapplePanel = grapplePanel.gameObject.GetComponent<Renderer>();
        grapplePanelColor = grapplePanel.material.GetColor("_EmissionColor");
        swingPanel = swingPanel.gameObject.GetComponent<Renderer>();
        swingPanelColor = swingPanel.material.GetColor("_EmissionColor");
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(grappleKey))
        {
            StartGrapple();
        }
        if (grappleCooldownTimer > 0f && !isGrappling)
        {
            grappleCooldownTimer -= Time.deltaTime;
            grapplePanel.material.SetColor("_EmissionColor", Color.black);
        }
        else
        {
            grapplePanel.material.SetColor("_EmissionColor", grapplePanelColor);
        }
        if (Input.GetKeyDown(swingKey))
        {
            StartSwing();
        }
        if (Input.GetKeyUp(swingKey))
        {
            StopSwing();
        }
        if (swingCooldownTimer > 0f && !isSwinging)
        {
            swingCooldownTimer -= Time.deltaTime;
            swingPanel.material.SetColor("_EmissionColor", Color.black);
        }
        else
        {
            swingPanel.material.SetColor("_EmissionColor", swingPanelColor);
        }
    }

    // Called after Update
    void LateUpdate()
    {
        if (isGrappling || isSwinging)
            DrawRope();
    }

    private void StartGrapple()
    {
        // Prevent if cooldown or swinging is still in play
        if (grappleCooldownTimer > 0f || isSwinging) return;

        isGrappling = true;
        player.isFrozen = true; // Freeze the player
        grappleCooldownTimer = grappleCooldown; // Reset cooldown and stick it there until grappling has stopped

        // Raycast to shoot grapple. If miss, set point at the maximum distance of ray
        RaycastHit hit;
        if (Physics.Raycast(cam.position, cam.forward, out hit, maxGrappleDistance, canGrapple))
        {
            grapplePoint = hit.point;
            
            Invoke(nameof(ExecuteGrapple), grappleDelayTime); // execute grappling
        }
        else
        {
            grapplePoint = cam.position + cam.forward * maxGrappleDistance;

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

    private void StartSwing()
    {
        // Prevent if cooldown or grappling is still in play
        if (swingCooldownTimer > 0f || isGrappling) return;
        
        isSwinging = true;
        swingCooldownTimer = swingCooldown; // Reset cooldown and stick it there until swinging has stopped
        
        // Raycast to shoot grapple. If miss, set point at the maximum distance of ray
        RaycastHit hit;
        if (Physics.Raycast(cam.position, cam.forward, out hit, maxSwingDistance, canGrapple))
        {
            grapplePoint = hit.point;
            
            Invoke(nameof(ExecuteSwing), swingDelayTime); // execute swinging
        }
        else
        {
            grapplePoint = cam.position + cam.forward * maxSwingDistance;

            Invoke(nameof(StopSwing), swingDelayTime); // stop swinging
        }

        lr.enabled = true;
        currentGrapplePosition = grappleTip.position;
    }

    private void ExecuteSwing()
    {
        if (isSwinging) // In case the input cancels swinging early before it reaches the point, prevent execution when it happens
        {
            swingCooldownTimer = swingCooldown; // Reset cooldown and stick it there until swinging has stopped
            
            // Set up joint at the grapple point
            joint = player.gameObject.AddComponent<SpringJoint>();
            joint.autoConfigureConnectedAnchor = false;
            joint.connectedAnchor = grapplePoint;

            float distanceFromPoint = Vector3.Distance(player.transform.position, grapplePoint);

            // Keeping the distance of the swing away and towards the grapple point
            joint.maxDistance = distanceFromPoint * maxJointDistanceScale;
            joint.minDistance = distanceFromPoint * minJointDistanceScale;

            // Settings used to set the joint's behaviour
            joint.spring = jointSpring;
            joint.damper = jointDamper;
            joint.massScale = jointMassScale;

            player.activeSwing = true;   
        }
    }

    private void StopSwing()
    {
        isSwinging = false;
        lr.enabled = false;
        if (joint) Destroy(joint); // Destroy the joint if it exists
        player.activeSwing = false;
    }

    void DrawRope()
    {
        if (isGrappling)
        {
            lr.startColor = grappleColour;
            lr.endColor = grappleColour;
        }
        else if (isSwinging)
        {
            lr.startColor = swingColour;
            lr.endColor = swingColour;
        }

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

    // Capture whether you're swinging or not
    public bool IsSwinging()
    {
        return isSwinging;
    }

    // Get the grappling ending point
    public Vector3 GetGrapplePoint()
    {
        return grapplePoint;
    }
}
