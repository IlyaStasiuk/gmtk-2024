using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace New
{
    public class GrapplingGun : MonoBehaviour
    {
        [SerializeField] Transform _gunPivot;
        [SerializeField] GrapplingHook _grapplingHook;
        [SerializeField] GrapplingSpring _grapplingSping;

        [SerializeField] private float _launchDistance = 30;
        [SerializeField] private KeyCode Button = KeyCode.Mouse0;

        [SerializeField] private bool rotateOverTime = true;
        [Range(0, 80)][SerializeField] private float rotationSpeed = 4;

        Vector2? _target = null;

<<<<<<< HEAD
        void Update()
        {
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            if (Input.GetKeyDown(Button))
            {
                _target = mousePosition;
=======
        [Header("Launching")]
        [SerializeField] private bool launchToPoint = true;
        [SerializeField] private LaunchType Launch_Type = LaunchType.Transform_Launch;
        [Range(0, 5)][SerializeField] private float launchSpeed = 5;

        [Header("No Launch To Point")]
        [SerializeField] private bool autoCongifureDistance = false;
        [SerializeField] private float targetDistance = 3;
        [SerializeField] private float targetFrequency = 3;

        [SerializeField] private ParticleSystem fireEffect;
        private Rigidbody2D attachToBody;
        private Vector3 attachToBodyLastPosition;


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
            if(m_camera == null) { m_camera = Camera.main; }
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

                if (attachToBody)
                {
                    Vector3 delta = attachToBody.transform.position - attachToBodyLastPosition;

                    m_springJoint2D.connectedAnchor += (Vector2)delta;
                    grapplePoint += (Vector2)delta;
                    
                    attachToBodyLastPosition = attachToBody.transform.position;
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
>>>>>>> c40f28cc28e6952234b57ca2c4354d634baa750a
            }

            if (_grapplingHook.State == GrapplingHook.HookState.Idle) RotateGun(mousePosition, true);
            else RotateGun(_grapplingHook.transform.position, true);

            if (_target.HasValue && _grapplingHook.State == GrapplingHook.HookState.Idle)
            {
                Vector2 launchVector = _target.Value - (Vector2)_gunPivot.transform.position;
                _grapplingHook.Launch(launchVector.normalized * _launchDistance);
                _target = null;
            }

            if (_grapplingHook.State == GrapplingHook.HookState.Grappled)
            {
                if (Input.GetKeyDown(Button) || Input.GetKeyUp(Button)) _grapplingHook.Retract();
            }

            if (_grapplingHook.State == GrapplingHook.HookState.Grappled) _grapplingSping.Grapple();
            else _grapplingSping.EndGrapple();
        }

        void RotateGun(Vector3 lookPoint, bool allowRotationOverTime)
        {
            Vector3 distanceVector = lookPoint - _gunPivot.transform.position;

            float angle = Mathf.Atan2(distanceVector.y, distanceVector.x) * Mathf.Rad2Deg;
            if (rotateOverTime && allowRotationOverTime)
            {
                Quaternion startRotation = _gunPivot.transform.rotation;
                _gunPivot.transform.rotation = Quaternion.Lerp(startRotation, Quaternion.AngleAxis(angle, Vector3.forward), Time.deltaTime * rotationSpeed);
            }
            else
            {
<<<<<<< HEAD
                _gunPivot.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
=======
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
                    fireEffect.Play();
                    attachToBody = hit.rigidbody;
                    attachToBodyLastPosition = hit.rigidbody.transform.position;
                    break;
                }
>>>>>>> c40f28cc28e6952234b57ca2c4354d634baa750a
            }
        }
    }
}