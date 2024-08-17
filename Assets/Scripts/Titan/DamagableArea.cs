using System;
using UnityEngine;


namespace Titan
{
    public class DamagableArea : MonoBehaviour
    {
        public float Damage = 10f;

        protected virtual void OnTriggerStay2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
            {
                TryApplyDamage(other.GetComponent<Player>());
            }
        }

        protected virtual void TryApplyDamage(Player player)
        {
            Debug.Log("Player is in damage area");
            player?.TakeDamage(Damage);
        }

        //temp mock class, TODO replace by real
        public class Player : MonoBehaviour
        {
            public void TakeDamage(float damage)
            {
                
            }
        }
    }
}