using System;
using UnityEngine;


namespace Titan
{
    public class DamageEffectArea : MonoBehaviour
    {
        [SerializeField] protected Vector3 PlayerDeathPositionShift;
        [SerializeField] protected float DurationToAttackPlayer;
        public float Damage = 10f;

        private PlayerTitanAttacker _player;
        private float _durationLeft;

        protected virtual void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
            {
                _player = other.GetComponent<PlayerTitanAttacker>();
                _durationLeft = DurationToAttackPlayer;
            }
        }

        protected virtual void OnTriggerStay2D(Collider2D other)
        {
            if (_player == null)
                return;

            bool wasPositive = _durationLeft > 0.0f;
            _durationLeft -= Time.deltaTime;
            bool isPositive = _durationLeft > 0.0f;

            bool hasChanged = wasPositive && !isPositive;

            if (hasChanged || DurationToAttackPlayer == 0.0f)
            {
                TryApplyDamage(_player);
            }
        }

        protected virtual void OnTriggerExit2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
            {
                _player = null;
                _durationLeft = DurationToAttackPlayer;
            }
        }

        protected virtual void TryApplyDamage(PlayerTitanAttacker player)
        {
            SoundManager.Instance.playSoundRandom(SoundType.PLAYER_DEATH_SMASHED);
            // Debug.Log("Player is in damage area");
            player.TakeDamage(Damage, transform, PlayerDeathPositionShift);
        }
    }
}