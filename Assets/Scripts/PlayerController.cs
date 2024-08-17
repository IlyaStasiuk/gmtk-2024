using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {
    [SerializeField] Rigidbody2D _rb;

    void FixedUpdate() {
        float move = Input.GetAxis("Horizontal");
        _rb.velocity = new Vector2(move * 5, _rb.velocity.y);
    }
}
