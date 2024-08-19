using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GrapplingDash : MonoBehaviour
{
    [SerializeField] Rigidbody2D _targetRigidbody;
    [SerializeField] GrapplingGun _grapplingGunLeft;
    [SerializeField] GrapplingGun _grapplingGunRight;

    [SerializeField] float _maxAngleToDash = 10f;
    [SerializeField] float _dashForce = 1f;
    [SerializeField] float _velocityChangeSpeed = 1f;

    void FixedUpdate()
    {
        bool leftGrappled = _grapplingGunLeft.GrapplingHook.State == GrapplingHook.HookState.Grappled;
        bool rightGrappled = _grapplingGunRight.GrapplingHook.State == GrapplingHook.HookState.Grappled;

        if (leftGrappled && rightGrappled) ApplyDash();
    }

    void ApplyDash()
    {
        Vector2 gunPosition = _grapplingGunLeft.transform.position;
        Vector2 leftVector = (Vector2)_grapplingGunLeft.GrapplingHook.transform.position - gunPosition;
        Vector2 rightVector = (Vector2)_grapplingGunRight.GrapplingHook.transform.position - gunPosition;

        float dot = Vector2.Dot(leftVector, rightVector);
        float cosAngle = Mathf.Cos(_maxAngleToDash * Mathf.Deg2Rad);

        if (dot > cosAngle)
        {
            Vector2 middleDirection = (leftVector + rightVector) / 2;
            middleDirection = middleDirection.normalized;

            float speed = _targetRigidbody.velocity.magnitude;
            Vector2 targetVelocity = middleDirection * speed;

            _targetRigidbody.velocity = Vector2.Lerp(_targetRigidbody.velocity, targetVelocity, Time.deltaTime * _velocityChangeSpeed);
            _targetRigidbody.AddForce(middleDirection * _dashForce, ForceMode2D.Force);

            Debug.DrawRay(gunPosition, middleDirection * speed, Color.red);
        }
    }
}