using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
    [CreateAssetMenu(menuName = "Actions/State Actions/Movement Aiming")]
    public class MovementAiming : StateActions
    {
        public float movementSpeed = 2;
        public float crouchSpeed = 2;

        public override void Execute(StateManager states)
        {
            if (states.movementValues.moveAmount > 0.1f)
                states.rigidbody.drag = 0;
            else
                states.rigidbody.drag = 4;

            float targetSpeed = movementSpeed;
            if (states.isCrouching)
                targetSpeed = crouchSpeed;

            Vector3 velocity = states.movementValues.moveDirection * (states.movementValues.moveAmount * targetSpeed);
            states.rigidbody.velocity = velocity;
        }

    }
}
