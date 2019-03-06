using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SO;

namespace SA
{
    [CreateAssetMenu(menuName ="Actions/State Actions/MonoAction From StateAction")]
    public class MonoActionFromStateAction : StateActions
    {
        public Action monoAction;

        public override void Execute(StateManager states)
        {
            monoAction.Execute();
        }

    }
}
