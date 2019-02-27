using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
    [CreateAssetMenu(menuName = "Actions/State Actions/Update Aiming")]
    public class UpdateAiming : StateActions
    {
        public override void Execute(StateManager states)
        {
            states.anim.SetBool(states.hashes.isAiming, states.isAiming);
        }
    }
}
