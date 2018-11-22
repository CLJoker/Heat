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
        public TransformVariable cameraTransform;
        public float speed = 8;

        public override void Execute(StateManager states)
        {
            if (cameraTransform.value == null)
                return;

            float h = states.movementValues.horizontal;
            float v = states.movementValues.vertical;

            Vector3 targetDir = cameraTransform.value.forward * v;
            targetDir += cameraTransform.value.right * h;
            targetDir.Normalize();

            targetDir.y = 0;
            if (targetDir == Vector3.zero)
                targetDir = states.transform.forward;

            Quaternion tr = Quaternion.LookRotation(targetDir);
            Quaternion targetRotation = Quaternion.Slerp(
                states.transform.rotation,
                tr,
                states.delta * states.movementValues.moveAmount * speed);

            states.transform.rotation = targetRotation;
        }

    }
}
