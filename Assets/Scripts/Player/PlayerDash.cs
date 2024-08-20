using System.Collections;
using UnityEngine;

public class PlayerDash : MonoBehaviour
{
    [SerializeField] Rigidbody2D _rb;
    [SerializeField] GrapplingGun _grapplingGunLeft;
    [SerializeField] GrapplingGun _grapplingGunRight;
    // [SerializeField] AnimationCurve _dashForce;
    [SerializeField] float _dashForceMultiplier = 10f;
    // [SerializeField] float _dashDuration = 0.5f;
    [SerializeField] float _maxSpeedToDash = 30f;

    bool _dashIsUsed;
    bool _isDashing;
    float _dashStartTime;
    Vector2 _dashDirection;

    void Update()
    {
        _dashDirection = Vector2.up;
        // _dashDirection = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
        _dashDirection.Normalize();
    }

    void FixedUpdate()
    {
        bool grappled = _grapplingGunLeft.GrapplingHook.State == GrapplingHook.HookState.Grappled || _grapplingGunRight.GrapplingHook.State == GrapplingHook.HookState.Grappled;
        if (grappled)
        {
            ReloadDash();
        }
        else
        {
            if (!_isDashing && !_dashIsUsed)
            {
                if (Input.GetKey(KeyCode.Space))
                {
                    float speedInDirectionOfDash = Vector2.Dot(_rb.velocity, _dashDirection);
                    // Debug.Log("Rb velocity: " + _rb.velocity + "Dash direction: " + _dashDirection + "Speed in direction of dash: " + speedInDirectionOfDash);
                    if (speedInDirectionOfDash < _maxSpeedToDash)
                    {
                        StartDash();
                    }
                }
            }
        }

        if (_isDashing)
        {
            if (!Input.GetKey(KeyCode.Space))
            {
                EndDash();
            }
            // else
        }
    }

    void ReloadDash()
    {
        _dashIsUsed = false;
    }

    void StartDash()
    {
        Debug.Assert(!_isDashing, "Trying to start a dash while already dashing");
        Debug.Assert(!_dashIsUsed, "Trying to start a dash while it is already used");

        _isDashing = true;
        _dashStartTime = Time.time;
        _dashIsUsed = true;

        float force = _dashForceMultiplier;
        // _rb.AddForce(_dashDirection * force, ForceMode2D.Impulse);
        _rb.AddForce(_dashDirection * force, ForceMode2D.Impulse);

        // Debug.DrawLine(transform.position, transform.position + (Vector3)_dashDirection * force, Color.red, 1f);
    }

    void UpdateDash(float progress)
    {
        // float force = _dashForce.Evaluate(progress) * _dashForceMultiplier;
        // _rb.AddForce(_dashDirection * force, ForceMode2D.Impulse);
        // Debug.Log("UpdateDash");
    }

    void EndDash()
    {
        _isDashing = false;

        // Debug.Log("EndDash");
    }
}
