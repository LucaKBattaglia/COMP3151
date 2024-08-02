using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrapplingGun : MonoBehaviour
{
    // SETTINGS
    [SerializeField] private float maxGrappleDistance = 100f;
    [SerializeField] private float maxGrappleSpeed = 8f;
    [SerializeField] private float maxJointDistanceScale = 0.8f;
    [SerializeField] private float minJointDistanceScale = 0.25f;
    [SerializeField] private float jointSpring = 4.5f;
    [SerializeField] private float jointDamper = 7f;
    [SerializeField] private float jointMassScale = 4.5f;
    
    [SerializeField] private LayerMask isGrappeable;
    [SerializeField] private Transform grappleTip;
    [SerializeField] private Transform camera;
    [SerializeField] private Transform player;
    
    private LineRenderer lr;
    private Vector3 grapplePoint;
    private SpringJoint joint;
    private Vector3 currentGrapplePosition;
    
    void Awake() {
        lr = GetComponent<LineRenderer>();
    }
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0)) {
            StartGrapple();
        }
        else if (Input.GetMouseButtonUp(0)) {
            StopGrapple();
        }
    }

    //Called after Update
    void LateUpdate() {
        DrawRope();
    }

    void StartGrapple() {
        RaycastHit hit;
        if (Physics.Raycast(camera.position, camera.forward, out hit, maxGrappleDistance, isGrappeable)) {
            grapplePoint = hit.point;
            joint = player.gameObject.AddComponent<SpringJoint>();
            joint.autoConfigureConnectedAnchor = false;
            joint.connectedAnchor = grapplePoint;

            float distanceFromPoint = Vector3.Distance(player.position, grapplePoint);

            joint.maxDistance = distanceFromPoint * maxJointDistanceScale;
            joint.minDistance = distanceFromPoint * minJointDistanceScale;

            joint.spring = jointSpring;
            joint.damper = jointDamper;
            joint.massScale = jointMassScale;

            lr.positionCount = 2;
            currentGrapplePosition = grappleTip.position;
        }
    }

    void StopGrapple() {
        lr.positionCount = 0;
        Destroy(joint);
    }

    void DrawRope() {
        //If not grappling, don't draw rope
        if (!joint) return;

        currentGrapplePosition = Vector3.Lerp(currentGrapplePosition, grapplePoint, Time.deltaTime * maxGrappleSpeed);
        
        lr.SetPosition(0, grappleTip.position);
        lr.SetPosition(1, currentGrapplePosition);
    }

    public bool IsGrappling() {
        return joint != null;
    }

    public Vector3 GetGrapplePoint() {
        return grapplePoint;
    }
}
