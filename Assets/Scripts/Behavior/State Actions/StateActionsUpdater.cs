using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
    [CreateAssetMenu(menuName ="Actions/State Actions/State Actions Holder")]
    public class StateActionsUpdater : StateActions
    {
        public StateActions[] stateActions;

        public override void Execute(StateManager states)
        {
            if (stateActions == null)
                return;

            for(int i = 0; i < stateActions.Length; i++)
            {
                stateActions[i].Execute(states);
            }
        }

    }
}
