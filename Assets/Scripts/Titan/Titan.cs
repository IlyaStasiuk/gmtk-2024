using System.Collections.Generic;
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
            {
                hand.SetIsFollowTarget(follow);
            }
        }

        private void OnAgro(bool agro)
        {
            SetHandsFollowTarget(agro);
        }
    }
}