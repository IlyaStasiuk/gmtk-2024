using DG.Tweening;
using UnityEngine;


namespace Titan
{
    public static class AnimationsHelper
    {
        public static Tween DOFlashAnimation(this SpriteRenderer spriteRenderer, float duration = 0.1f)
        {
            const string flashValueKey = "_FlashAmount";
            var material = spriteRenderer.material;
            material.SetFloat(flashValueKey, 1f);
            return DOTween.To(() => material.GetFloat(flashValueKey), x => material.SetFloat(flashValueKey, x), 0f, duration)
                .SetEase(Ease.InOutExpo)
                .SetLoops(2, LoopType.Yoyo);
        }
    }
}