using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
    [CreateAssetMenu(menuName = "Conditions/Monitor If Vaulting False")]
    public class MonitorIfVaultingFalse : Condition
    {
        public override bool CheckCondition(StateManager state)
        {
            return !state.isVaulting;
        }

    }
}
