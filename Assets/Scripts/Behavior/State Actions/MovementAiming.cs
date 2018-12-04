using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
    [CreateAssetMenu(menuName = "Actions/State Actions/Movement Aiming")]
    public class MovementAiming : StateActions
    {
        public float movementSpeed = 2;

        public override void Execute(StateManager states)
        {
            if (states.movementValues.moveAmount > 0.1f)
                states.rigid.drag = 0;
            else
                states.rigid.drag = 4;

            Vector3 velocity = states.movementValues.moveDirection * (states.movementValues.moveAmount * movementSpeed);
            states.rigid.velocity = velocity;
        }

    }
}
