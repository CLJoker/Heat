using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SO;

namespace SA
{
    [CreateAssetMenu(menuName ="Actions/State Actions/Assign Transform")]
    public class AssignTransformStateAction : StateActions
    {
        public TransformVariable transformVariable;

        public override void Execute(StateManager states)
        {
            transformVariable.value = states.transform;
        }
    }
}
