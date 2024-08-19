using System;
using UnityEngine;


namespace Titan
{
    public class DamageEffectArea : MonoBehaviour
    {
        public float Damage = 10f;

        protected virtual void OnTriggerStay2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
            {
                var player = other.GetComponent<PlayerTitanAttacker>();
                TryApplyDamage(player);
            }
        }

        protected virtual void TryApplyDamage(PlayerTitanAttacker player)
        {
            // Debug.Log("Player is in damage area");
            player.TakeDamage(Damage);
        }
    }
}