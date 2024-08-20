using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.LowLevel;


namespace Titan
{
    public class Titan : MonoBehaviour
    {
        public Animator Animator;
        public GameObject Hand;
        public TitanAgroRange AgroRange;
        public TitanMouth Mouth;
        public GameObject Head;
        public GameObject SeverdHead;
        public Transform HeadPos;
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
            Animator.SetFloat("speed_multiplier", 1 / Mathf.Sqrt(transform.localScale.y - 4));
            Hand.GetComponent<Animator>().SetFloat("speed_multiplier", 1 / (Mathf.Sqrt((transform.localScale.y - 4) * 2)));
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
            // Debug.Log($"Titan {name} agro: {player?.name}");
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

        private void OnWeakSpotDie(float force)
        {
            // NeckSpriteRenderer.DOColor(Color.red, 0.5f)
            //     .SetLoops(2, LoopType.Yoyo)
            //     .OnComplete(() => NeckSpriteRenderer.color = Color.white)
            //     .Play();

            if (NormalizedHealth <= 0)
            {
                SoundManager.Instance.playSound(GetDeathSoundByScale());

                bool decapitate = PlayerTitanTransformation.instance.IsTitan;
                Kill(decapitate);
            }
        }

        private SoundType GetDeathSoundByScale()
        {
            return transform.localScale.y switch
            {
                <= 10 => SoundType.TITAN_DEATH_SMALL,
                <= 15 => SoundType.TITAN_DEATH_MEDIUM,
                _ => SoundType.TITAN_DEATH_BIG
            };
        }

        public void SuperKill()
        {
            if (Head) Head.SetActive(false);

            GameObject LooseHead = Instantiate(SeverdHead, HeadPos.position, HeadPos.rotation);
            LooseHead.transform.localScale = transform.localScale;
            Vector2 force;
            force.x = 0;
            force.y = 600;
            LooseHead.GetComponent<Rigidbody2D>().AddForce(force);

        }
        [Button]
        public void Kill(bool decapitate)
        {

            // Debug.Log($"Titan {name} died");

            SetRagdoll(true);

            if (decapitate) SuperKill();
        }

        [Button]
        public void Revive() => SetRagdoll(false);
    }
}