using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using Menu;
using NaughtyAttributes;
using UnityEngine;


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
            Animator.SetFloat("speed_multiplier",1/Mathf.Sqrt(transform.localScale.y-4));
            Hand.GetComponent<Animator>().SetFloat("speed_multiplier", 1 /( Mathf.Sqrt((transform.localScale.y-4)*2)));
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
        
        private void OnWeakSpotDie(float force)
        {
            if (NormalizedHealth <= 0)
            {
                MenuScreen.Instance.GameUI.AddScore(GetScoreByScale(transform.localScale.y));
                SoundManager.Instance.playSound(GetDeathSoundByScale());
                CameraShaker.Instance.Shake(1);

                Kill();
                SuperKill();
            }
        }

        public int GetCameraShakePowerByScaleOnDeath()
        {
            return transform.localScale.y switch
            {
                <= 10 => 1,
                <= 15 => 2,
                > 20 => 3,
                _ => 1
            };
        }

        public static int GetScoreByScale(float scaleY)
        {
            //if want to change, change the values in GameUI.AddScore also
            return scaleY switch
            {
                <= 10 => 100,
                <= 15 => 500,
                _ => 1000
            };
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
            Head.SetActive(false);

            Instantiate(SeverdHead, HeadPos.position, HeadPos.rotation);

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