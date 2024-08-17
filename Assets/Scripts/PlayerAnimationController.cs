using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationController : MonoBehaviour
{
    [SerializeField] private  Animator animator;
    Vector2 pos;
    void FixedUpdate()
    {
        Vector2 velocity = new Vector2(transform.position.x, transform.position.y) - pos;
        pos = new Vector2( transform.position.x, transform.position.y);
        animator.SetBool("MovingRight", velocity.x <= 0f);
        animator.SetFloat("UpDown", velocity.y * 1.5f);
    }
}
