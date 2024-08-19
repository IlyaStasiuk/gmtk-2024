using System;
using NaughtyAttributes;
using UnityEngine;


namespace Titan
{
    public class TitanMouth : DamageEffectArea
    {
        public Animator Animator;
        public float OpenEverySeconds = 5f;
        public float HoldOpenedSeconds = 3f;

        [ShowNativeProperty] public float TimerLeft => _timerLeft;

        private float _timerLeft;
        private bool _canApplyDamage = false;

        private void Awake()
        {
            _timerLeft = OpenEverySeconds;
        }

        public void SetDisabled(bool isRagdoll)
        {
            Animator.enabled = !isRagdoll;
            enabled = !isRagdoll;

            if (_canApplyDamage)
                CloseMouth();
        }

        private void Update()
        {
            if (_timerLeft > 0)
            {
                _timerLeft -= Time.deltaTime;
                return;
            }

            if (_canApplyDamage)
            {
                CloseMouth();
                _timerLeft = OpenEverySeconds;
            }
            else
            {
                OpenMouth();
                _timerLeft = HoldOpenedSeconds;
            }
        }

        protected override void TryApplyDamage(PlayerTitanAttacker player)
        {
            if (_canApplyDamage)
            {
                Animator.SetTrigger("Bite");
                SoundManager.Instance.playSoundRandom(SoundType.PLAYER_DEATH_EATEN);
                player.TakeDamage(Damage, transform, PlayerDeathPositionShift);
                _canApplyDamage = false;
                _timerLeft = OpenEverySeconds;
            }
        }
        
        private void OpenMouth()
        {
            //play sound with 20% chance
            if (UnityEngine.Random.Range(0, 5) == 0)
                SoundManager.Instance.playSoundRandom(SoundType.TITAN_SCREAM_1, SoundType.TITAN_SCREAM_2, SoundType.TITAN_SCREAM_3);

            _canApplyDamage = true;
            Animator.SetTrigger("Open");
        }

        private void CloseMouth()
        {
            _canApplyDamage = false;
            Animator.SetTrigger("Close");
        }
    }
}