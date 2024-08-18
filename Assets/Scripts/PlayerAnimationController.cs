using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationController : MonoBehaviour
{
    [SerializeField] private  Animator animator;
    Vector2 pos;
    Camera cam;

    private void Start()
    {
        cam = Camera.main;
    }
    void FixedUpdate()
    {
        Vector2 velocity = new Vector2(transform.position.x, transform.position.y) - pos;
        pos = new Vector2( transform.position.x, transform.position.y);
        Vector2 mousePos = cam.ScreenToWorldPoint(Input.mousePosition);
        animator.SetFloat("LeftRight", ( mousePos.x - transform.position.x) * 0.1f);
        animator.SetFloat("UpDownHead", (mousePos.y - transform.position.y) * 0.1f);
        animator.SetFloat("UpDown", velocity.y * 12f);
    }
}
