using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationController : MonoBehaviour
{
    [SerializeField] private  Animator animator;
    public bool isTitan;
    bool wasTitan;
    Vector2 pos;
    Camera cam;

    private void Start()
    {
        isTitan = false;
        wasTitan = isTitan;
        cam = Camera.main;
    }
    void FixedUpdate()
    {
        Vector2 velocity = new Vector2(transform.position.x, transform.position.y) - pos;
        pos = new Vector2( transform.position.x, transform.position.y);
        Vector2 mousePos = cam.ScreenToWorldPoint(Input.mousePosition);
        if (isTitan)
        {
            animator.SetFloat("LeftRight", (velocity.x * 10f) * 0.1f);
        }
        else
        {
            animator.SetFloat("LeftRight", (mousePos.x - transform.position.x) * 0.1f);
        }
        animator.SetFloat("UpDownHead", (mousePos.y - transform.position.y) * 0.1f);
        animator.SetFloat("UpDown", velocity.y * 12f);
        
        if(isTitan != wasTitan) {
            if(isTitan)
            {
                animator.SetTrigger("toTitan");
            }
            else
            {
                animator.SetTrigger("toHuman");
            }
        }
            wasTitan = isTitan;
    }
}
