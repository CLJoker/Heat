using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
    [CreateAssetMenu(menuName ="Actions/Mono Actions/On Aiming")]
    public class OnAiming : Condition
    {
        public bool reverse;

        public override bool CheckCondition(StateManager state)
        {
            if(!reverse)
            {
                return state.isAiming;
            }
            else
            {
                return !state.isAiming;
            }
        }

    }
}
