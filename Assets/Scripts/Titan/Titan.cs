using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using NaughtyAttributes;
using UnityEngine;


namespace Titan
{
    public class Titan : MonoBehaviour
    {
        public Animator Animator;
        public TitanAgroRange AgroRange;
        public TitanMouth Mouth;
        public List<TitanWeakSpot> WeakSpots;
        public List<TitanHand> Hands;
        
        public SpriteRenderer NeckSpriteRenderer;

        [ShowNativeProperty]
        public float NormalizedHealth
        {
            get
            {
                if (DestroyableTitanParts == null)
                    return 0;

                var parts = DestroyableTitanParts.Where(it => it != null).ToList();
                if (parts.Any())
                    return parts.Average(it => it.NormalizedHealth);

                return 0;
            }
        }

        protected IEnumerable<IDestroyableTitanPart> DestroyableTitanParts => WeakSpots/*.Concat(Hands) TODO?*/;

        private void Awake()
        {
            AgroRange.OnAgro += OnAgro;
            
            foreach (var weakSpot in WeakSpots)
                weakSpot.OnPartDestroy += OnWeakSpotDie;
        }

        public void SetHandsFollowTarget(PlayerTitanAttacker player)
        {
            foreach (var hand in Hands)
                hand.SetIsFollowTarget(player);
        }

        private void OnAgro(PlayerTitanAttacker player)
        {
            Debug.Log($"Titan {name} agro: {player?.name}");
            SetHandsFollowTarget(player);
        }

        private void SetRagdoll(bool isRagdoll)
        {
            var rigidbodyArray = GetComponentsInChildren<Rigidbody2D>();
            foreach (var col in rigidbodyArray)
                col.bodyType = isRagdoll ? RigidbodyType2D.Dynamic : RigidbodyType2D.Kinematic;

            foreach (var hand in Hands)
                hand.SetRagdoll(isRagdoll);

            Mouth.SetDisabled(isRagdoll);
            Animator.enabled = !isRagdoll;
            AgroRange.enabled = !isRagdoll;
            enabled = !isRagdoll;
        }

        private void OnWeakSpotDie()
        {
            // NeckSpriteRenderer.DOColor(Color.red, 0.5f)
            //     .SetLoops(2, LoopType.Yoyo)
            //     .OnComplete(() => NeckSpriteRenderer.color = Color.white)
            //     .Play();

            if (NormalizedHealth <= 0)
                Kill();
        }

        [Button]
        public void Kill()
        {
            Debug.Log($"Titan {name} died");
            SetRagdoll(true);
        }

        [Button]
        public void Revive() => SetRagdoll(false);
    }
}