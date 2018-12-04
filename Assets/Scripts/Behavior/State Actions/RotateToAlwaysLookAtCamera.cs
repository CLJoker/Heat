using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
    [CreateAssetMenu(menuName = "Actions/State Actions/Rotate To Always Look On Camera")]
    public class RotateToAlwaysLookAtCamera : StateActions
    {
        public float speed = 8;

        public override void Execute(StateManager states)
        {


            Vector3 targetDir = states.movementValues.lookDirection;

            targetDir.y = 0;
            if (targetDir == Vector3.zero)
                targetDir = states.mTransform.forward;

            Quaternion tr = Quaternion.LookRotation(targetDir);
            Quaternion targetRotation = Quaternion.Slerp(
                states.mTransform.rotation,
                tr,
                states.delta * speed);

            states.mTransform.rotation = targetRotation;
        }

    }
}
