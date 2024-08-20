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
    [SerializeField] Transform _origin;

    [SerializeField] List<GameObject> _ignoredObjects;
    [SerializeField] float _nearClippingPlane = 1f;
    [SerializeField] AnimationCurve _flyingSpeed;
    [SerializeField] float _flyinghSpeedMultiplier = 3f;
    [SerializeField] AnimationCurve _retractSpeed;
    [SerializeField] float _retractSpeedMultiplier = 1f;
    [SerializeField] LayerMask _grappableLayers;
    [SerializeField] ParticleSystem _fireEffect;

    HookState _state;
    Vector2 _flyingDirection;
    RaycastHit2D _grappleHit;
    Vector3 _grappleOffset;
    float _retractDistanceLeft;

    RaycastHit2D[] _hitsCache = new RaycastHit2D[16];

    public float DistancePercentage => CurrentDistance / MaxDistance;
    public float CurrentDistance => Vector2.Distance(_origin.position, transform.position);
    public float MaxDistance => _grapplingRope.MaxLength;
    public HookState State => _state;

    void Awake()
    {
        transform.parent = null;
    }

    void Start()
    {
        _grapplingRope.Clear();
    }

    void Update()
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

        _grapplingRope.SetSinWave();

        SoundManager.Instance.playSoundRandom(SoundType.GRAPLING_SHOT_1, SoundType.GRAPLING_SHOT_2);
    }

    public void Retract()
    {
        if (_state != HookState.Flying && _state != HookState.Grappled) return;
        // Debug.Log("Retract");

        _state = HookState.Retracting;

        _grappleHit = default;

        _grapplingRope.SetCurve();

        _retractDistanceLeft = CurrentDistance;
    }

    void Grapple(RaycastHit2D hit)
    {
        // Debug.Log("Grapple " + hit.transform.gameObject.name);

        Debug.Assert(_state == HookState.Flying, _state);
        _state = HookState.Grappled;

        _grappleHit = hit;
        transform.position = hit.point;

        Transform target = hit.collider.transform;
        _grappleOffset = target.InverseTransformPoint(transform.position);

        // Debug.DrawLine(target.position, transform.position, Color.green, 5f);

        _grapplingRope.Straighten();

        if (_fireEffect) _fireEffect.Play();
        SoundManager.Instance.playSoundRandom(SoundType.GRAPLING_HIT_1);
    }

    void Reset()
    {
        // Debug.Log("Reset");
        Debug.Assert(_state == HookState.Retracting, _state);

        _state = HookState.Idle;

        transform.position = _origin.position;

        _grapplingRope.Clear();

        _retractDistanceLeft = 0f;
    }

    void UpdateFlying()
    {

        Debug.Assert(_state == HookState.Flying, _state);

        Vector2 prevPosition = transform.position;
        float speed = _flyingSpeed.Evaluate(CurrentDistance / MaxDistance) * _flyinghSpeedMultiplier;
        Vector2 raycastVector = speed * Time.deltaTime * _flyingDirection.normalized;

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
            if (CurrentDistance >= _grapplingRope.MaxLength) Retract();
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
                if (_nearClippingPlane > distance)
                {
                    validHit = false;
                    // Debug.DrawLine(_origin.position, hit.point, Color.red, 1f);
                }

                bool layerCheck = ((1 << hit.collider.gameObject.layer) & (int)_grappableLayers) != 0;
                if (!layerCheck) validHit = false;
            }

            if (validHit) break;
            else currentIndex++;
        }

        if (currentIndex < hitCount)
        {
            RaycastHit2D hit = _hitsCache[currentIndex];
            return hit;
        }

        return null;
    }

    void UpdateGrappled()
    {
        Debug.Assert(_state == HookState.Grappled, _state);

        Transform target = _grappleHit.collider.transform;
        Vector3 targetPosition = target.TransformPoint(_grappleOffset);
        Quaternion targetRotation = target.rotation * Quaternion.Euler(_grappleOffset);
        transform.SetPositionAndRotation(targetPosition, targetRotation);

        // Debug.DrawLine(target.position, transform.position, Color.red, 5f);
    }

    void UpdateRetracting()
    {
        Debug.Assert(_state == HookState.Retracting, _state);

        float speed = _retractSpeed.Evaluate(_retractDistanceLeft / MaxDistance) * _retractSpeedMultiplier;
        float retractDistance = Time.deltaTime * speed;
        _retractDistanceLeft -= retractDistance;

        if (_retractDistanceLeft <= 1f) Reset();
        else
        {
            Vector2 currentDirection = (transform.position - _origin.position).normalized;
            transform.position = _origin.position + (Vector3)currentDirection * _retractDistanceLeft;
        }
    }
}