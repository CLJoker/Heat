using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using SO;

namespace SA
{
    [CreateAssetMenu(menuName = "Actions/State Actions/Rotate Based On Camera")]
    public class RotateBasedOnCamera : StateActions
    {
        public float speed = 8;

        public override void Execute(StateManager states)
        {


            Vector3 targetDir = states.movementValues.moveDirection;

            targetDir.y = 0;
            if (targetDir == Vector3.zero)
                targetDir = states.mTransform.forward;

            Quaternion tr = Quaternion.LookRotation(targetDir);
            Quaternion targetRotation = Quaternion.Slerp(
                states.mTransform.rotation,
                tr,
                states.delta * states.movementValues.moveAmount * speed);

            states.mTransform.rotation = targetRotation;
        }

    }
}
