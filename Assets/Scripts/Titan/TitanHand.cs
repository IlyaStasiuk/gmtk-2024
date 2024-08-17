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

        public void SetIsFollowTarget(bool follow)
        {
            IKManager.enabled = follow;
            Animator.enabled = !follow;
        }
    }
}