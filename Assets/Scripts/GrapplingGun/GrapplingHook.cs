using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GrapplingHook : MonoBehaviour
{
    public enum HookState
    {
        Idle,
        Flying,
        Grappled,
        Retracting
    }

    [SerializeField] GrapplingRope _grapplingRope;
    [SerializeField] Rigidbody2D _rigidbody;
    [SerializeField] FixedJoint2D _joint;
    [SerializeField] Transform _origin;

    [SerializeField] List<GameObject> _ignoredObjects;
    // [SerializeField] private float maxDistance = 4;
    [SerializeField] private float _nearClippingPlane = 1f;
    [SerializeField] float _launchSpeed = 3f;
    [SerializeField] float _retractSpeed = 1f;
    [SerializeField] private LayerMask _grappableLayers;

    HookState _state;
    Vector2 _flyingDirection;
    RaycastHit2D _grappleHit;

    RaycastHit2D[] _hitsCache = new RaycastHit2D[16];

    float CurrentDistance => Vector2.Distance(_origin.position, transform.position);

    public HookState State => _state;

    void Start()
    {
        _grapplingRope.Clear();
    }

    void FixedUpdate()
    {
        switch (_state)
        {
            case HookState.Flying:
                UpdateFlying();
                break;
            case HookState.Grappled:
                UpdateGrappled();
                break;
            case HookState.Retracting:
                UpdateRetracting();
                break;
        }
    }

    public void Launch(Vector2 direction)
    {
        if (_state != HookState.Idle) return;
        // Debug.Log("Launch " + direction);

        _state = HookState.Flying;

        _flyingDirection = direction;
        transform.position = _origin.position;

        // _grapplingRope.disableDrag = true;
        _grapplingRope.SetSinWave();
    }

    public void Retract()
    {
        if (_state != HookState.Flying && _state != HookState.Grappled) return;
        // Debug.Log("Retract");

        _state = HookState.Retracting;

        _grappleHit = default;
        _rigidbody.simulated = false;
        _joint.connectedBody = null;
        _joint.enabled = false;

        // _grapplingRope.disableDrag = false;
        _grapplingRope.SetCurve();

        // _state = false;
        // _rb.velocity = Vector2.zero;
    }

    void Grapple(RaycastHit2D hit)
    {
        // Debug.Log("Grapple " + hit.transform.gameObject.name);

        Debug.Assert(_state == HookState.Flying, _state);
        _state = HookState.Grappled;

        _grappleHit = hit;
        _rigidbody.simulated = true;
        _joint.connectedBody = hit.rigidbody;
        _joint.enabled = true;

        // _grapplingRope.disableDrag = true;
        _grapplingRope.Straighten();

        transform.position = hit.point;
    }

    void Reset()
    {
        // Debug.Log("Reset");
        Debug.Assert(_state == HookState.Retracting, _state);

        _state = HookState.Idle;

        transform.position = _origin.position;

        _grapplingRope.Clear();
    }

    void UpdateFlying()
    {

        Debug.Assert(_state == HookState.Flying, _state);

        Vector2 prevPosition = transform.position;
        Vector2 raycastVector = _launchSpeed * _flyingDirection.normalized;

        // Debug.Log("UpdateFlying");

        RaycastHit2D? hit = RaycastFlying(prevPosition, raycastVector);

        if (hit.HasValue)
        {
            Grapple(hit.Value);
        }
        else
        {
            // Debug.Log("UpdateFlying transform");

            transform.position += (Vector3)raycastVector;
            if (CurrentDistance >= _flyingDirection.magnitude) Retract();
        }
    }

    RaycastHit2D? RaycastFlying(Vector2 start, Vector2 direction)
    {
        // Debug.Log("RaycastFlying " + start + " " + direction);

        // Debug.DrawLine(start, start + direction, Color.red, 0.1f);
        int hitCount = Physics2D.RaycastNonAlloc(start, direction.normalized, _hitsCache, direction.magnitude);

        // Debug.Log("RaycastFlying hits " + hits.Count);

        int currentIndex = 0;
        while (currentIndex < hitCount)
        {
            RaycastHit2D hit = _hitsCache[currentIndex];
            bool validHit = true;

            if (_ignoredObjects.Contains(hit.transform.gameObject)) validHit = false;
            else
            {
                float distance = Vector2.Distance(hit.point, _origin.position);
                if (_nearClippingPlane > distance) validHit = false;
            }

            if (validHit) break;
            else currentIndex++;
        }

        if (currentIndex < hitCount)
        {
            RaycastHit2D hit = _hitsCache[currentIndex];
            bool layerCheck = ((1 << hit.transform.gameObject.layer) & (int)_grappableLayers) != 0;
            if (layerCheck) return hit;
        }

        return null;
    }

    void UpdateGrappled()
    {
        Debug.Assert(_state == HookState.Grappled, _state);

    }

    void UpdateRetracting()
    {
        Debug.Assert(_state == HookState.Retracting, _state);

        Vector2 retractDirection = _origin.position - transform.position;
        Vector2 retractVector = retractDirection.normalized * _retractSpeed;
        float distanceLeft = Vector2.Distance(_origin.position, transform.position);

        if (distanceLeft <= 1f || distanceLeft <= retractVector.magnitude) Reset();
        else transform.position += (Vector3)retractVector;
    }
}