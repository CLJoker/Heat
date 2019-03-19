using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SO;

namespace SA
{
    [CreateAssetMenu(menuName = "Actions/Delta Time Manager")]
    public class DeltaTimeManager : Action
    {
        public FloatVariable variable;
        public bool isFixed;

        public override void Execute()
        {
            variable.value = (isFixed)? Time.fixedDeltaTime : Time.deltaTime;
        }
    }
}
