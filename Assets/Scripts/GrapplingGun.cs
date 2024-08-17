using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;

// https://www.youtube.com/watch?v=dnNCVcVS6uw
public class GrapplingGun : MonoBehaviour {
    [Header("Scripts Ref:")]
    public GrapplingRope grappleRope;

    [Header("Layers Settings:")]
    [SerializeField] private bool grappleToAll = false;
    [SerializeField] private int grappableLayerNumber = 9;

    [Header("Main Camera:")]
    public Camera m_camera;

    [Header("Transform Ref:")]
    public Transform gunHolder;
    public Transform gunPivot;
    public Transform firePoint;

    [Header("Physics Ref:")]
    public DistanceJoint2D joint;
    public Rigidbody2D m_rigidbody;

    [Header("Rotation:")]
    [SerializeField] private bool rotateOverTime = true;
    [Range(0, 60)][SerializeField] private float rotationSpeed = 4;

    [Header("Distance:")]
    [SerializeField] private bool hasMaxDistance = false;
    [SerializeField] private float maxDistnace = 20;

    private enum LaunchType {
        Transform_Launch,
        Physics_Launch
    }

    [Header("Launching:")]
    [SerializeField] private bool launchToPoint = true;
    [SerializeField] private LaunchType launchType = LaunchType.Physics_Launch;
    [SerializeField] private float launchSpeed = 1;

    [Header("No Launch To Point")]
    [SerializeField] private bool autoConfigureDistance = false;
    [SerializeField] private float targetDistance = 3;
    [SerializeField] private float targetFrequncy = 1;

    [HideInInspector] public Vector2 grapplePoint;
    [HideInInspector] public Vector2 grappleDistanceVector;

    private void Start() {
        grappleRope.enabled = false;
        joint.enabled = false;

    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.Mouse0)) {
            SetGrapplePoint();
        } else if (Input.GetKey(KeyCode.Mouse0)) {
            if (grappleRope.enabled) {
                RotateGun(grapplePoint, false);
            } else {
                Vector2 mousePos = m_camera.ScreenToWorldPoint(Input.mousePosition);
                RotateGun(mousePos, true);
            }

            if (launchToPoint && grappleRope.isGrappling) {
                if (launchType == LaunchType.Transform_Launch) {
                    Vector2 firePointDistnace = firePoint.position - gunHolder.localPosition;
                    Vector2 targetPos = grapplePoint - firePointDistnace;
                    gunHolder.position = Vector2.Lerp(gunHolder.position, targetPos, Time.deltaTime * launchSpeed);
                }
            }
        } else if (Input.GetKeyUp(KeyCode.Mouse0)) {
            grappleRope.enabled = false;
            joint.enabled = false;
            m_rigidbody.gravityScale = 1;
        } else {
            Vector2 mousePos = m_camera.ScreenToWorldPoint(Input.mousePosition);
            RotateGun(mousePos, true);
        }
    }

    void RotateGun(Vector3 lookPoint, bool allowRotationOverTime) {
        Vector3 distanceVector = lookPoint - gunPivot.position;

        float angle = Mathf.Atan2(distanceVector.y, distanceVector.x) * Mathf.Rad2Deg;
        if (rotateOverTime && allowRotationOverTime) {
            gunPivot.rotation = Quaternion.Lerp(gunPivot.rotation, Quaternion.AngleAxis(angle, Vector3.forward), Time.deltaTime * rotationSpeed);
        } else {
            gunPivot.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        }
    }

    void SetGrapplePoint() {
        Vector2 distanceVector = m_camera.ScreenToWorldPoint(Input.mousePosition) - gunPivot.position;
        
        List<RaycastHit2D> results = new();
        ContactFilter2D filter = new();
        int resultsCount = Physics2D.Raycast(firePoint.position, distanceVector.normalized, filter, results);
        if (resultsCount > 0 && results.First().transform == gunHolder) {
            results.RemoveAt(0);
        }
        bool hasAnyHit = results.Any();
        
        if (hasAnyHit) {
            resultsCount = Physics2D.Raycast(firePoint.position, distanceVector.normalized, filter, results);
            if (resultsCount > 0 && results.First().transform == gunHolder) {
                results.RemoveAt(0);
            }

            RaycastHit2D hit = results.First();
            if (hit.transform.gameObject.layer == grappableLayerNumber || grappleToAll) {
                if (Vector2.Distance(hit.point, firePoint.position) <= maxDistnace || !hasMaxDistance) {
                    grapplePoint = hit.point;
                    grappleDistanceVector = grapplePoint - (Vector2)gunPivot.position;
                    grappleRope.enabled = true;
                }
            }
        }
    }

    public void Grapple() {
        //m_springJoint2D.autoConfigureDistance = false;
        if (!launchToPoint && !autoConfigureDistance)
        {
            Vector2 jointPos = joint.transform.position;
            Vector2 direction = (grapplePoint - jointPos);
            float distance = direction.magnitude;
            joint.distance = distance;
            Vector2 grappleForce = direction.normalized * (distance * 0.85f);
            m_rigidbody.AddForce(grappleForce, ForceMode2D.Impulse);
            //joint.frequency = targetFrequncy;
        }
        if (!launchToPoint) {
            if (autoConfigureDistance) {
                //m_springJoint2D.autoConfigureDistance = true;
                //joint.frequency = 0;
            }

            joint.connectedAnchor = grapplePoint;
            joint.enabled = true;
        } else {
            switch (launchType) {
                case LaunchType.Physics_Launch:
                    joint.connectedAnchor = grapplePoint;

                    Vector2 distanceVector = firePoint.position - gunHolder.position;

                    joint.distance = distanceVector.magnitude;
                    //joint.frequency = launchSpeed;
                    joint.enabled = true;
                    break;
                case LaunchType.Transform_Launch:
                    m_rigidbody.gravityScale = 0;
                    m_rigidbody.velocity = Vector2.zero;
                    break;
            }
        }
    }

    private void OnDrawGizmosSelected() {
        if (firePoint != null && hasMaxDistance) {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(firePoint.position, maxDistnace);
        }
    }

}
