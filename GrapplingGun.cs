using UnityEngine;

public class GrapplingGun : MonoBehaviour
{
    public LayerMask whatIsGrappleable;
    public Transform gunTip, player;
    public bool isBeingUsed;
    public float maxDistance = 100f;
    private Vector3 currentGrapplePosition;
    private Vector3 grapplePoint;
    private SpringJoint joint;
    private LineRenderer lr;
    
    private void Awake()
    {
        lr = GetComponent<LineRenderer>();
    }

    private void Update()
    {
        if (isBeingUsed)
        {
            StartGrapple();
            gameObject.GetComponent<BoxCollider>().enabled = false;
            gameObject.GetComponent<Rigidbody>().isKinematic = true;
            player.GetComponent<Rigidbody>().freezeRotation = true;
            player.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
            
        }
        else if (!isBeingUsed)
        {
            StopGrapple();
            gameObject.GetComponent<BoxCollider>().enabled = true;
            gameObject.GetComponent<Rigidbody>().isKinematic = false;
            player.GetComponent<Rigidbody>().freezeRotation = false;
            player.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;

        }
    }

    private void LateUpdate()
    {
        DrawRope();
    }

    private void StartGrapple()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, maxDistance, whatIsGrappleable))
        {
            grapplePoint = hit.point;
            joint = player.gameObject.AddComponent<SpringJoint>();
            joint.autoConfigureConnectedAnchor = false;
            joint.connectedAnchor = grapplePoint;

            var distanceFromPoint = Vector3.Distance(player.position, grapplePoint);

            // The distance grapple will try keep from grapple point.
            joint.maxDistance = distanceFromPoint * 0.2f;
            joint.minDistance = distanceFromPoint * 0.1f;

            joint.spring = 10f;
            joint.damper = 7f;
            joint.massScale = 4.5f;

            lr.positionCount = 2;
            currentGrapplePosition = gunTip.position;
        }
    }

    private void DrawRope()
    {
        // If not grappling, Don't draw rope
        if (!joint) return;
        currentGrapplePosition = Vector3.Lerp(currentGrapplePosition, grapplePoint, Time.deltaTime * 8f);
        lr.SetPosition(0, gunTip.position);
        lr.SetPosition(1, grapplePoint);
    }

    private void StopGrapple()
    {
        lr.positionCount = 0;
        Destroy(joint);
        joint = null;
        currentGrapplePosition = Vector3.zero;
        grapplePoint = Vector3.zero;
    }

    public bool IsGrappling()
    {
        return joint != null;
    }

    public Vector3 GetGrapplePoint()
    {
        return grapplePoint;
    }
}