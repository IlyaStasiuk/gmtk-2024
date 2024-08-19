using System;
using UnityEngine;


namespace Titan
{
    public class DamageEffectArea : MonoBehaviour
    {
        public float Damage = 10f;

        private PlayerTitanAttacker _player;

        protected virtual void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
            {
                _player = other.GetComponent<PlayerTitanAttacker>();
            }
        }

        protected virtual void OnTriggerStay2D(Collider2D other)
        {
            if (_player == null)
                return;

            TryApplyDamage(_player);
        }

        protected virtual void OnTriggerExit2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
            {
                _player = null;
            }
        }

        protected virtual void TryApplyDamage(PlayerTitanAttacker player)
        {
            // Debug.Log("Player is in damage area");
            player.TakeDamage(Damage);
        }
    }
}