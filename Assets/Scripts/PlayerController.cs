using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerController : MonoBehaviour {
    [SerializeField] Rigidbody2D _rb;
    [SerializeField]  Camera camera;

    private bool shiftDown;
    private Vector2 InputDirection;
    [SerializeField] private Collider2D thisCollider;
    [SerializeField] private List<DistanceJoint2D> joints;

    private void Update()
    {
        InputDirection = new(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        if(Input.GetKeyDown(KeyCode.LeftShift))
        {
            shiftDown = true;
        }
    }

    void FixedUpdate() {
        //Vector2 cameraWorld = camera.ScreenToWorldPoint(Input.mousePosition);
        List<RaycastHit2D> results = new();
        results.Capacity = 2;
        ContactFilter2D filter = new();
        Physics2D.Raycast(_rb.position, Vector2.down * 0.5f,filter, results, 0.5f);

        //bool hasAnyHit = results.Any(item => item.transform != _rb.transform);
        float move = InputDirection.x;
        Vector2 playerMovement = new Vector2(move * 5, 0);

        float velocityMagnitude = _rb.velocity.magnitude;
        //float deltaMagnitude = playerMovement.magnitude - velocityMagnitude;
            
        // if (deltaMagnitude < 0) {
        //     deltaMagnitude = 10 / (Mathf.Abs(deltaMagnitude) + 1);
        // } else {
        //     //deltaMagnitude = Mathf.Max(deltaMagnitude, 0.1f);
        // }
            
        //playerMovement = playerMovement.normalized* deltaMagnitude;
        _rb.AddForce(playerMovement, ForceMode2D.Force);
            
        if (shiftDown)
        {
            shiftDown = false;
            Vector2 force = InputDirection;
            force = force.normalized * 10.0f;
            _rb.AddForce(force,ForceMode2D.Impulse);
        }

        bool isGrappling = joints.Any(joint => joint.enabled);

        thisCollider.isTrigger = isGrappling;
    }
}
