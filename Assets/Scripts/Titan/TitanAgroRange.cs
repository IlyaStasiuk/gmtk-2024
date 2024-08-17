using System;
using UnityEngine;


namespace Titan
{
    public class TitanAgroRange : MonoBehaviour
    {
        public event Action<PlayerTitanAttacker> OnAgro;

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
            {
                if (other.TryGetComponent<PlayerTitanAttacker>(out var player))
                    OnAgro?.Invoke(player);
            }
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
            {
                if (other.TryGetComponent<PlayerTitanAttacker>(out _))
                    OnAgro?.Invoke(null);
            }
        }
    }
}