using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundAnimController : MonoBehaviour
{
    [SerializeField] private Animator[] animator;
    Camera cam;

    private void Awake()
    {
        for (int i = 0; i < animator.Length; i++)
        {
            AnimatorClipInfo state = animator[i].GetCurrentAnimatorClipInfo(0)[0];
            animator[i].Play("idle", 0, 1f /( i+1)); ;
        }
        cam = Camera.main;
    }

    private void Update()
    {
        Shader.SetGlobalFloat("_CamPosX", cam.transform.position.x);
    }
}
