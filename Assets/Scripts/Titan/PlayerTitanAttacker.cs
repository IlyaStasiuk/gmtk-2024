using System;
using UnityEngine;
using UnityEngine.Events;


namespace Titan
{
    public class PlayerTitanAttacker : MonoBehaviour
    {
        [SerializeField] private GameObject deathParticles;
        public Rigidbody2D PlayerRoot;
        public float Health = 100f;
        public float Damage = 10f;

        public UnityEvent<GameObject> OnHit;

        public void TakeDamage(float damage, Transform attackerTransform, Vector3 PlayerDeathPositionShift)
        {
            if (Health <= 0)
                return;

            Health -= damage;
            if (Health <= 0)
            {
                Die(attackerTransform, PlayerDeathPositionShift);
            }
        }

        private void Die(Transform attackerTransform, Vector3 playerDeathPositionShift)
        {
            if (SceneRestarter.instance.IsInDeathZone)
                return;

            Debug.Log("Player died");
            PlayerRoot.simulated = false;
            if (attackerTransform != null)
            {
                PlayerRoot.transform.parent = attackerTransform;
                PlayerRoot.transform.localPosition = Vector3.zero + playerDeathPositionShift;
            }

            GameObject instance = Instantiate(deathParticles);
            instance.transform.position = PlayerRoot.transform.position;

            var animator = PlayerRoot.GetComponentInChildren<Animator>();
            animator.enabled = false;

            SceneRestarter.instance.SetPlayerDied();
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Titan"))
            {
                var weakSpot = other.GetComponent<TitanWeakSpot>();
                if (weakSpot != null)
                {
                    OnHit.Invoke(other.gameObject);

                    Debug.Log($"Player hit weak spot ({weakSpot.name}) of titan {other.name}");
                    weakSpot.TakeDamage(Damage);
                }
            }
        }
    }
}