using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
    [CreateAssetMenu(menuName ="Actions/State Actions/Switcher")]
    public class StateActionSwitcher : StateActions
    {
        public SO.BoolVariable targetBool;
        public StateActions onFalseAction;
        public StateActions onTrueAction;


        public override void Execute(StateManager states)
        {
            if(targetBool.value)
            {
                if (onTrueAction != null)
                    onTrueAction.Execute(states);
            }
            else
            {
                if (onFalseAction != null)
                    onFalseAction.Execute(states);
            }

        }
    }
}
