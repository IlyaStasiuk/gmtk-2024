

using UnityEngine;

public class GrapplingSpring : MonoBehaviour
{
    // private enum LaunchType
    // {
    //     Transform_Launch,
    //     Physics_Launch,
    // }

    public SpringJoint2D _springJoint;
    public Rigidbody2D _targetRigidbody;
    public Transform _grapplePoint;

    [SerializeField][Range(0, 5)] float _launchSpeed = 5;
    [SerializeField] float _disableSpringDistance = 1f;

    // [Header("Launching")]
    // [SerializeField] private bool launchToPoint = true;
    // [SerializeField] private LaunchType Launch_Type = LaunchType.Transform_Launch;

    // [Header("No Launch To Point")]
    // [SerializeField] private bool autoCongifureDistance = false;
    // [SerializeField] private float targetDistance = 3;
    // [SerializeField] private float targetFrequency = 3;

    public bool Grappled => _springJoint.enabled;

    void Start()
    {
        _springJoint.enabled = false;
        _targetRigidbody.gravityScale = 1;
    }

    void FixedUpdate()
    {
        if (Grappled)
        {
            float distance = Vector2.Distance(_springJoint.connectedAnchor, _targetRigidbody.position);
            float springPercent = FloatUtils.MapRange01(distance, 0, _disableSpringDistance);
            _springJoint.frequency = _launchSpeed * springPercent;

            _springJoint.connectedAnchor = _grapplePoint.position;
        }

        // if (launchToPoint && grapplingRope.isGrappling)
        // {
        //     if (Launch_Type == LaunchType.Transform_Launch)
        //     {
        //         gunHolder.position = Vector3.Lerp(gunHolder.position, grapplePoint, Time.deltaTime * launchSpeed);
        //     }
        // }
    }

    public void Grapple()
    {
        if (Grappled) return;

        // if (!launchToPoint && !autoCongifureDistance)
        // {
        //     _springJoint.distance = targetDistance;
        //     _springJoint.frequency = targetFrequency;
        // }

        // if (!launchToPoint)
        // {
        //     if (autoCongifureDistance)
        //     {
        //         _springJoint.autoConfigureDistance = true;
        //         _springJoint.frequency = 0;
        //     }
        //     _springJoint.connectedAnchor = grapplePoint;
        //     _springJoint.enabled = true;
        // }
        // else
        // {
        // if (Launch_Type == LaunchType.Transform_Launch)
        // {
        //     _targetRigidbody.gravityScale = 0;
        //     _targetRigidbody.velocity = Vector2.zero;
        // }

        // if (Launch_Type == LaunchType.Physics_Launch)
        // {
        _springJoint.connectedAnchor = _grapplePoint.position;
        _springJoint.distance = 0;
        _springJoint.frequency = _launchSpeed;
        _springJoint.enabled = true;

        // float speed = _targetRigidbody.velocity.magnitude;
        // Vector2 direction = ((Vector2)_grapplePoint.position - _targetRigidbody.position).normalized;
        // _targetRigidbody.velocity = direction * speed;

        // _targetRigidbody.gravityScale = 0;

        // }

        // }
    }

    public void EndGrapple()
    {
        if (!Grappled) return;

        _springJoint.enabled = false;
        // _targetRigidbody.gravityScale = 1;
    }
}