using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
    [CreateAssetMenu(menuName = "Actions/State Actions/Movement Animations All")]
    public class UpdateMovementAnimationsAll : StateActions
    {
        public override void Execute(StateManager states)
        {
            if (states.isAiming)
            {
                states.anim.SetFloat(states.hashes.vertical, states.movementValues.vertical, 0.2f, states.delta);
                states.anim.SetFloat(states.hashes.horizontal, states.movementValues.horizontal, 0.2f, states.delta);
            }
            else
            {
                states.anim.SetFloat(states.hashes.vertical, states.movementValues.moveAmount, 0.2f, states.delta);
            }
        }
    }
}
