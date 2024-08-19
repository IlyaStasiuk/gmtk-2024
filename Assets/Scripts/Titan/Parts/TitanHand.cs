using System.Collections.Generic;
using System.Linq;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.U2D.IK;


namespace Titan
{
    public class TitanHand : MonoBehaviour
    {
        public Animator Animator;
        public IKManager2D IKManager;
        public Transform Target;

        public void SetIsFollowTarget(PlayerTitanAttacker player)
        {
            if (!enabled)
                return;

            bool playerAsTargetExists = player != null;
            Target.SetParent(playerAsTargetExists ? player.transform : transform);
            Target.localPosition = Vector3.zero;

            // IKManager.enabled = playerAsTargetExists;
            Animator.enabled = !playerAsTargetExists;
        }

        public void SetRagdoll(bool isRagdoll)
        {
            IKManager.enabled = !isRagdoll;
            Animator.enabled = !isRagdoll;
            enabled = !isRagdoll;
        }
    }
}