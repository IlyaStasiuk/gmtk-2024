using System.Collections.Generic;
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

        public List<Collider2D> RagdollColliders;
        public List<Joint2D> RagdollJoints;

        public void SetIsFollowTarget(PlayerTitanAttacker player)
        {
            if (!enabled)
                return;

            bool playerAsTargetExists = player != null;
            Target.SetParent(playerAsTargetExists ? player.transform : transform);
            Target.localPosition = Vector3.zero;

            IKManager.enabled = playerAsTargetExists;
            Animator.enabled = !playerAsTargetExists;
        }

        public void SetRagdoll(bool isRagdoll)
        {
            Animator.enabled = !isRagdoll;
            IKManager.enabled = !isRagdoll;
            enabled = !isRagdoll;

            foreach (var ragdollCollider in RagdollColliders)
                ragdollCollider.enabled = isRagdoll;

            foreach (var ragdollJoint in RagdollJoints)
                ragdollJoint.enabled = isRagdoll;
        }
    }
}