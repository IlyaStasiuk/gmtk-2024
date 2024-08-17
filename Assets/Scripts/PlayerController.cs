using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerController : MonoBehaviour {
    [SerializeField] Rigidbody2D _rb;
    [SerializeField]  Camera camera;


    void FixedUpdate() {
        //Vector2 cameraWorld = camera.ScreenToWorldPoint(Input.mousePosition);
        List<RaycastHit2D> results = new();
        results.Capacity = 2;
        ContactFilter2D filter = new();
        Physics2D.Raycast(_rb.position, Vector2.down * 0.5f,filter, results, 0.5f);

        //bool hasAnyHit = results.Any(item => item.transform != _rb.transform);
        
        float move = Input.GetAxis("Horizontal");

        float velocityMagnitude = _rb.velocity.magnitude;
        Vector2 playerMovement = new Vector2(move * 5, 0);
        float deltaMagnitude = playerMovement.magnitude - velocityMagnitude;
            
        if (deltaMagnitude < 0) {
            deltaMagnitude = 0.2f / (Mathf.Abs(deltaMagnitude) + 1);
        } else {
            deltaMagnitude = Mathf.Max(deltaMagnitude, 0.1f);
        }

        playerMovement = playerMovement.normalized* deltaMagnitude;
        Debug.Log(playerMovement.magnitude);

        _rb.velocity += playerMovement;
    }
}
