using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLimits : MonoBehaviour
{
    [SerializeField] Rigidbody2D _rigidbody;
    [SerializeField] float _maxSpeed = 80f;
    [SerializeField] float _slowDownHeight = 80f;
    [SerializeField] float _maxHeight = 100f;
    [SerializeField] AnimationCurve _heightSlowDownCurve;

    void FixedUpdate()
    {
        float speed = _rigidbody.velocity.magnitude;
        if (speed > _maxSpeed)
        {
            _rigidbody.velocity = _rigidbody.velocity.normalized * _maxSpeed;
        }

        Debug.Log(speed);

        if (_rigidbody.velocity.y > 0)
        {
            float height = _rigidbody.position.y;
            float slowDownProgress = FloatUtils.MapRange01(height, _slowDownHeight, _maxHeight);
            if (slowDownProgress > 0f)
            {
                float slowDownForce = _heightSlowDownCurve.Evaluate(slowDownProgress);
                _rigidbody.velocity = Vector2.Lerp(_rigidbody.velocity, new Vector2(_rigidbody.velocity.x, 0), slowDownForce);
            }
        }

    }
}
