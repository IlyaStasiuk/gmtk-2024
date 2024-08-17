using System;
using UnityEngine;


namespace Titan
{
    public class PlayerTitanAttacker : MonoBehaviour
    {
        public float Health = 100f;
        public float Damage = 10f;

        public void TakeDamage(float damage)
        {
            if (Health <= 0)
                return;

            Health -= damage;
            if (Health <= 0)
            {
                Die();
            }
        }

        private void Die()
        {
            Debug.Log("Player died");
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Titan"))
            {
                var weakSpot = other.GetComponent<TitanWeakSpot>();
                if (weakSpot != null)
                {
                    Debug.Log($"Player hit weak spot ({weakSpot.name}) of titan {other.name}");
                    weakSpot.TakeDamage(Damage);
                }
            }
        }
    }
}