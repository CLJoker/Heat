using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SO;

namespace SA
{
    [CreateAssetMenu(menuName = "Actions/State Actions/Assign State Manager")]
    public class AssignStateManagerStateAction : StateActions
    {
        public StatesVariable variable;

        public override void Execute(StateManager states)
        {
            variable.value = states;
        }
    }
}
