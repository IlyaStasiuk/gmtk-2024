using System;
using DG.Tweening;
using NaughtyAttributes;
using UnityEngine;


namespace Titan
{
    public class TitanWeakSpot : MonoBehaviour, IDestroyableTitanPart
    {
        public event Action OnPartDestroy;

        [SerializeField] private Animator _animator;
        [SerializeField] private SpriteRenderer _spriteRenderer;
        [SerializeField] private float _maxHealth = 100f;
        [SerializeField] private float _health = 100f;

        public float MaxHealth => _maxHealth;
        public float Health => _health;
        public float NormalizedHealth => Mathf.Clamp01(Health / MaxHealth);

        private void Awake()
        {
            _health = _maxHealth;
        }

        public void TakeDamage(float damage)
        {
            if (_health <= 0)
                return;

            _health -= damage;
            PlayHitAnimation();
            if (Health <= 0)
            {
                Die();
            }
        }

        private void Die()
        {
            PlayDieAnimation();
            OnPartDestroy?.Invoke();
        }

        [Button]
        private void PlayHitAnimation()
        {
            _spriteRenderer.DOFlashAnimation().Play();
        }
        
        private void PlayDieAnimation()
        {
            _animator.SetTrigger("Destroy");
            _spriteRenderer.transform.DOScale(Vector3.zero, 0.5f).SetEase(Ease.InOutBack).Play();
        }
    }
}