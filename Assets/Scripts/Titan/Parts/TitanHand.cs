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
        [Space]
        public Rigidbody2D RBOfJoinTOBody;
        public List<Collider2D> RagdollColliders;
        public List<Rigidbody2D> RagdollRigidbodys;
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
            return;
            Animator.enabled = !isRagdoll;
            IKManager.enabled = !isRagdoll;
            enabled = !isRagdoll;
            RBOfJoinTOBody.simulated = isRagdoll;

            GetComponents<DamageEffectArea>().ToList().ForEach(it => it.enabled = !isRagdoll);
            RagdollRigidbodys.ForEach(it => it.simulated = isRagdoll);
            RagdollColliders.ForEach(it => it.isTrigger = !isRagdoll);
            RagdollJoints.ForEach(it => it.enabled = isRagdoll);
        }
    }
}