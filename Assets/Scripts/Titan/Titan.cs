using System.Collections.Generic;
using System.Linq;
using NaughtyAttributes;
using UnityEngine;


namespace Titan
{
    public class Titan : MonoBehaviour
    {
        public TitanAgroRange AgroRange;
        public TitanMouth Mouth;
        public List<TitanWeakSpot> WeakSpots;
        public List<TitanHand> Hands;

        public float NormalizedHealth => DestroyableTitanParts.Where(it => it != null).Average(it => it.NormalizedHealth);

        protected IEnumerable<IDestroyableTitanPart> DestroyableTitanParts => WeakSpots/*.Concat(Hands) TODO?*/;

        private void Awake()
        {
            AgroRange.OnAgro += OnAgro;
            
            foreach (var weakSpot in WeakSpots)
                weakSpot.OnPartDestroy += OnWeakSpotDie;
        }

        public void SetHandsFollowTarget(bool follow)
        {
            foreach (var hand in Hands)
                hand.SetIsFollowTarget(follow);
        }

        private void OnAgro(bool agro)
        {
            Debug.Log($"Titan {name} agro: {agro}");
            SetHandsFollowTarget(agro);
        }

        private void SetRagdoll(bool isRagdoll)
        {
            GetComponent<Rigidbody2D>().simulated = isRagdoll;
            foreach (var col in GetComponents<Collider2D>())
                col.enabled = isRagdoll;

            Mouth.SetRagdoll(isRagdoll);
            foreach (var hand in Hands)
                hand.SetRagdoll(isRagdoll);

            AgroRange.enabled = !isRagdoll;
            enabled = !isRagdoll;
        }

        private void OnWeakSpotDie()
        {
            if (NormalizedHealth <= 0)
                Kill();
        }

        [Button]
        public void Kill() => SetRagdoll(true);
        
        [Button]
        public void Revive() => SetRagdoll(false);
    }
}