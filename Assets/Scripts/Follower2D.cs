using UnityEngine;

public class Follower2D : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private AnimationCurve SpeedCurveX;
    [SerializeField] private AnimationCurve SpeedCurveY;


    private void LateUpdate()
    {
        Vector3 thisPosition = transform.position;
        thisPosition.z = target.position.z;

        Vector3 direction = (target.position - thisPosition);
        float distance = direction.magnitude;
        float speedX = SpeedCurveX.Evaluate(distance);
        float speedY = SpeedCurveY.Evaluate(distance);

        Vector3 movementDirection = direction.normalized;
        movementDirection.x *= speedX * Time.deltaTime;
        movementDirection.y *= speedY * Time.deltaTime;

        movementDirection.x = Mathf.Clamp(movementDirection.x, -Mathf.Abs(direction.x), Mathf.Abs(direction.x));
        movementDirection.y = Mathf.Clamp(movementDirection.y, -Mathf.Abs(direction.y), Mathf.Abs(direction.y));

        transform.position += movementDirection;
    }
}