using System;
using UnityEngine;

public class Follower2D : MonoBehaviour {
    [SerializeField] private Transform target;
    [SerializeField] private AnimationCurve SpeedCurve;


    private void LateUpdate() {
        Vector3 thisPosition = transform.position;
        thisPosition.z = target.position.z;
        
        Vector3 direction = (target.position - thisPosition);
        float distance = direction.magnitude;
        float speed = SpeedCurve.Evaluate(distance);

        direction = direction.normalized * (speed * Time.deltaTime);
        transform.position += direction;
    }
}