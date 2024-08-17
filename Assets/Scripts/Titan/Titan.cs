using System.Collections.Generic;
using System.Linq;
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

        [ShowNativeProperty]
        public float NormalizedHealth
        {
            get
            {
                var parts = DestroyableTitanParts.Where(it => it != null).ToList();
                if (parts.Any())
                    parts.Average(it => it.NormalizedHealth);
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
            GetComponent<Rigidbody2D>().simulated = isRagdoll;
            foreach (var col in GetComponents<Collider2D>())
                col.enabled = isRagdoll;

            Mouth.SetRagdoll(isRagdoll);
            foreach (var hand in Hands)
                hand.SetRagdoll(isRagdoll);

            Animator.enabled = !isRagdoll;
            AgroRange.enabled = !isRagdoll;
            enabled = !isRagdoll;
        }

        private void OnWeakSpotDie()
        {
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