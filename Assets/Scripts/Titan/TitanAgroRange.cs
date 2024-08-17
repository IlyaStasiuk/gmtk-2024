using System;
using UnityEngine;


namespace Titan
{
    public class TitanAgroRange : MonoBehaviour
    {
        public event Action<bool> OnAgro;

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
            {
                OnAgro?.Invoke(true);
            }
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
            {
                OnAgro?.Invoke(false);
            }
        }
    }
}