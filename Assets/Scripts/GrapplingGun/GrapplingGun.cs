using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace New
{
    public class GrapplingGun : MonoBehaviour
    {
        [Header("Scripts:")]
        public GrappleRope grappleRope;

        [Header("Layer Settings:")]
        [SerializeField] private LayerMask grappableLayers;

        [Header("Main Camera")]
        public Camera m_camera;

        [Header("Transform Refrences:")]
        public Transform gunHolder;
        public Transform gunPivot;
        public Transform firePoint;

        [Header("Rotation:")]
        [SerializeField] private bool rotateOverTime = true;
        [Range(0, 80)][SerializeField] private float rotationSpeed = 4;

        [Header("Distance:")]
        [SerializeField] private bool hasMaxDistance = true;
        [SerializeField] private float maxDistance = 4;
        [SerializeField] private float minDistance = 1f;
        [SerializeField] private float disableSpringDistance = 1f;

        [Header("Launching")]
        [SerializeField] private bool launchToPoint = true;
        [SerializeField] private LaunchType Launch_Type = LaunchType.Transform_Launch;
        [Range(0, 5)][SerializeField] private float launchSpeed = 5;

        [Header("No Launch To Point")]
        [SerializeField] private bool autoCongifureDistance = false;
        [SerializeField] private float targetDistance = 3;
        [SerializeField] private float targetFrequency = 3;


        private enum LaunchType
        {
            Transform_Launch,
            Physics_Launch,
        }

        [Header("Component Refrences:")]
        public SpringJoint2D m_springJoint2D;
        public Rigidbody2D ballRigidbody;

        [Header("Custom")]
        [SerializeField] private KeyCode Button = KeyCode.Mouse0;


        [HideInInspector] public Vector2 grapplePoint;
        Vector2 Mouse_FirePoint_DistanceVector;

        public Vector2 DistanceVector => grapplePoint - (Vector2)gunPivot.position;

        private void Start()
        {
            grappleRope.enabled = false;
            m_springJoint2D.enabled = false;
            ballRigidbody.gravityScale = 1;
        }

        private void Update()
        {
            Mouse_FirePoint_DistanceVector = m_camera.ScreenToWorldPoint(Input.mousePosition) - gunPivot.position;

            if (Input.GetKeyDown(Button))
            {
                SetGrapplePoint();
            }
            else if (Input.GetKey(Button))
            {
                RotateGun(grapplePoint, false);
                // if (grappleRope.enabled)
                // {
                // }
                // else
                // {
                //     RotateGun(m_camera.ScreenToWorldPoint(Input.mousePosition), false);
                // }

                if (launchToPoint && grappleRope.isGrappling)
                {
                    if (Launch_Type == LaunchType.Transform_Launch)
                    {
                        gunHolder.position = Vector3.Lerp(gunHolder.position, grapplePoint, Time.deltaTime * launchSpeed);
                    }
                }

            }
            else if (Input.GetKeyUp(Button))
            {
                grappleRope.enabled = false;
                m_springJoint2D.enabled = false;
                ballRigidbody.gravityScale = 1;
            }
            else
            {
                RotateGun(m_camera.ScreenToWorldPoint(Input.mousePosition), true);
            }


            if (launchToPoint && m_springJoint2D.enabled)
            {
                float springPercent = FloatUtils.MapRange01(DistanceVector.magnitude, 0, disableSpringDistance);
                m_springJoint2D.frequency = launchSpeed * springPercent;
            }
        }

        void RotateGun(Vector3 lookPoint, bool allowRotationOverTime)
        {
            Vector3 distanceVector = lookPoint - gunPivot.position;

            float angle = Mathf.Atan2(distanceVector.y, distanceVector.x) * Mathf.Rad2Deg;
            if (rotateOverTime && allowRotationOverTime)
            {
                Quaternion startRotation = gunPivot.rotation;
                gunPivot.rotation = Quaternion.Lerp(startRotation, Quaternion.AngleAxis(angle, Vector3.forward), Time.deltaTime * rotationSpeed);
            }
            else
                gunPivot.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

        }

        void SetGrapplePoint()
        {
            List<RaycastHit2D> hits = Physics2D.RaycastAll(firePoint.position, Mouse_FirePoint_DistanceVector.normalized).ToList();

            if (hits.Count == 0) return;

            while (hits.Count > 0)
            {
                if (hits[0].transform == gunHolder)
                {
                    hits.RemoveAt(0);
                }
                else if (minDistance > hits[0].distance)
                {
                    hits.RemoveAt(0);
                }
                else
                {
                    break;
                }
            }

            if (hits.Count == 0) return;

            foreach (var hit in hits)
            {
                bool layerCheck = ((1 << hit.transform.gameObject.layer) & (int)grappableLayers) != 0;
                bool distanceCheck = (Vector2.Distance(hit.point, firePoint.position) <= maxDistance) || !hasMaxDistance;

                if (layerCheck && distanceCheck)
                {
                    grapplePoint = hit.point;
                    grappleRope.enabled = true;
                    break;
                }
            }
        }

        public void Grapple()
        {

            if (!launchToPoint && !autoCongifureDistance)
            {
                m_springJoint2D.distance = targetDistance;
                m_springJoint2D.frequency = targetFrequency;
            }

            if (!launchToPoint)
            {
                if (autoCongifureDistance)
                {
                    m_springJoint2D.autoConfigureDistance = true;
                    m_springJoint2D.frequency = 0;
                }
                m_springJoint2D.connectedAnchor = grapplePoint;
                m_springJoint2D.enabled = true;
            }

            else
            {
                if (Launch_Type == LaunchType.Transform_Launch)
                {
                    ballRigidbody.gravityScale = 0;
                    ballRigidbody.velocity = Vector2.zero;
                }
                if (Launch_Type == LaunchType.Physics_Launch)
                {
                    m_springJoint2D.connectedAnchor = grapplePoint;
                    m_springJoint2D.distance = 0;
                    m_springJoint2D.frequency = launchSpeed;
                    m_springJoint2D.enabled = true;
                }
            }
        }

        private void OnDrawGizmos()
        {
            // if (hasMaxDistance)
            // {
            //     Gizmos.color = Color.green;
            //     Gizmos.DrawWireSphere(firePoint.position, maxDistance);
            // }
        }

    }
}