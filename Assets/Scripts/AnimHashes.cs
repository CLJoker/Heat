using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
    public class AnimHashes
    {
        public int vertical = Animator.StringToHash("vertical");
        public int horizontal = Animator.StringToHash("horizontal");
        public int leftFootForward = Animator.StringToHash("leftFootForward");
        public int JumpForward = Animator.StringToHash("Jump Forward");
        public int JumpIdle = Animator.StringToHash("Jump Idle");
        public int isGrounded = Animator.StringToHash("isGrounded");
        public int LandFast = Animator.StringToHash("Land Fast");
        public int LandHard = Animator.StringToHash("Land Hard");
        public int LandRolling = Animator.StringToHash("Land Rolling");
        public int isInteracting = Animator.StringToHash("isInteracting");
        public int VaultWalk = Animator.StringToHash("Vault Walk");
        public int isAiming = Animator.StringToHash("aiming");
    }
}
