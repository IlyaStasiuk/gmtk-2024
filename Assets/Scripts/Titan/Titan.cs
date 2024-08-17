using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;


namespace Titan
{
    public class Titan : MonoBehaviour
    {
        public TitanAgroRange AgroRange;
        public TitanMouth Mouth;
        public List<TitanHand> Hands;

        private void Awake()
        {
            AgroRange.OnAgro += OnAgro;
        }

        public void SetHandsFollowTarget(bool follow)
        {
            foreach (var hand in Hands)
                hand.SetIsFollowTarget(follow);
        }

        private void OnAgro(bool agro)
        {
            SetHandsFollowTarget(agro);
        }

        private void SetRagdoll(bool isRagdoll)
        {
            Mouth.SetRagdoll(isRagdoll);
            foreach (var hand in Hands)
                hand.SetRagdoll(isRagdoll);

            AgroRange.enabled = !isRagdoll;
            enabled = !isRagdoll;
        }

        [Button]
        public void Kill() => SetRagdoll(true);
        
        [Button]
        public void Revive() => SetRagdoll(false);
    }
}