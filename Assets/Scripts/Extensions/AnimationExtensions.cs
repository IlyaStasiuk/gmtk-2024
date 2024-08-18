using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;


namespace Titan
{
    public static class AnimationExtensions
    {
        private const float DefaultFlashDuration = 0.25f;

        public static Tween DOFlashAnimation(this SpriteRenderer spriteRenderer, float duration = DefaultFlashDuration)
        {
            const string flashValueKey = "_BlinkValue";
            var material = spriteRenderer.material;
            material.SetFloat(flashValueKey, 1f);
            return DOTween.To(() => material.GetFloat(flashValueKey), x => material.SetFloat(flashValueKey, x), 0f, duration)
                .SetEase(Ease.InOutExpo)
                .OnComplete(() => material.SetFloat(flashValueKey, 0f));
        }

        public static Tween DOFlashAnimation(this IEnumerable<SpriteRenderer> spriteRenderers, float duration = DefaultFlashDuration)
        {
            var sequence = DOTween.Sequence();
            foreach (var renderer in spriteRenderers)
                sequence.Join(renderer.DOFlashAnimation(duration));

            return sequence;
        }
    }
}