using System.Collections.Generic;
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

        public void SetIsFollowTarget(bool follow)
        {
            IKManager.enabled = follow;
            Animator.enabled = !follow;
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