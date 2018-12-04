using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
    [CreateAssetMenu(menuName ="Actions/State Actions/Anim Bool From Bool Var")]
    public class ChangeAnimBoolFromBoolVariable : StateActions
    {
        public SO.BoolVariable targetBoolVar;
        public string targetAnimVar;
        public bool reverse;


        public override void Execute(StateManager states)
        {
            if(reverse)
            {
                states.anim.SetBool(targetAnimVar, !targetBoolVar.value);
            }
            states.anim.SetBool(targetAnimVar, targetBoolVar.value);
        }
    }
}
