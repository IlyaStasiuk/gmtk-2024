using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisualLayerController : MonoBehaviour
{
    [SerializeField] Renderer[] rends;
    [SerializeField] float parallaxSpeed = 0.2f;
    [SerializeField] float movementSpeed = 0f;
    [SerializeField] Color tintColor = Color.white;
    void Start()
    {
        rends = GetComponentsInChildren<Renderer>();
        foreach (Renderer r in rends) {

//            r.material.SetFloat("_Scale", -0.03f);
            r.material.SetFloat("_Slide", parallaxSpeed * 0.1f);
            r.material.SetFloat("_Speed", -movementSpeed * 0.1f);
            r.material.SetFloat("_Offset", Random.Range(0f,33f));
            if(r is SpriteRenderer)
            {
                SpriteRenderer sprite = (SpriteRenderer)r;
                Color col = tintColor * sprite.color;
                sprite.color = col;
            }
                }
    }

}
