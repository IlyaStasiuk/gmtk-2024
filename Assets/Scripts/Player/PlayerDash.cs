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
        _dashDirection = (Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position).normalized;
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
            if (Input.GetKeyDown(KeyCode.Space))
            {
                float speed = _rb.velocity.magnitude;
                if (!_dashIsUsed && speed < _maxSpeedToDash)
                {

                    StartDash();
                }
            }
        }

        if (_isDashing)
        {
            if (!Input.GetKey(KeyCode.Space))
            {
                // float progress = Mathf.Clamp01((Time.time - _dashStartTime) / _dashDuration);
                // UpdateDash(progress);

                // if (progress >= 1f) EndDash();
                EndDash();
            }
            // else
            // {
            //     EndDash();
            // }
        }
    }

    void ReloadDash()
    {
        _dashIsUsed = false;
    }

    void StartDash()
    {
        if (_isDashing) return;

        _isDashing = true;
        _dashStartTime = Time.time;
        _dashIsUsed = true;

        float force = _dashForceMultiplier;
        _rb.AddForce(_dashDirection * force, ForceMode2D.Impulse);

        // Debug.Log("StartDash");
    }

    void UpdateDash(float progress)
    {
        if (!_isDashing) return;

        // float force = _dashForce.Evaluate(progress) * _dashForceMultiplier;
        // _rb.AddForce(_dashDirection * force, ForceMode2D.Impulse);
        // Debug.Log("UpdateDash");
    }

    void EndDash()
    {
        if (!_isDashing) return;

        _isDashing = false;

        // Debug.Log("EndDash");
    }
}
