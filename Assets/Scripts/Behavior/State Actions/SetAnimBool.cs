using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
    [CreateAssetMenu(menuName ="Actions/State Actions/Set Anim Bool")]
    public class SetAnimBool : StateActions
    {
        public string targetBool;
        public bool status;

        public override void Execute(StateManager states)
        {
            states.anim.SetBool(targetBool, status);
        }

    }
}
