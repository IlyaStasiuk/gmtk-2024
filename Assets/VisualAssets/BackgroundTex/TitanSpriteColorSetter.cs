using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitanSpriteColorSetter : MonoBehaviour
{
    [SerializeField] private Color col = Color.white;

    private void Awake()
    {
        SpriteRenderer[] spriteRenderers = GetComponentsInChildren<SpriteRenderer>();
        foreach (var sprite in spriteRenderers)
        {
            Color color = sprite.color;
            color *= col;
            sprite.color = color;
        }
    }
}
