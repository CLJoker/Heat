using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
    [CreateAssetMenu(menuName = "Actions/State Actions/Init Player Controller")]
    public class InitPlayerController : StateActions
    {
        public StateActions initActionsBatch;

        public override void Execute(StateManager states)
        {
            states.mTransform = states.transform;
            states.anim = states.mTransform.GetComponentInChildren<Animator>();
            states.rigid = states.mTransform.GetComponent<Rigidbody>();

            states.rigid.drag = 4;
            states.rigid.angularDrag = 999;
            states.rigid.constraints = RigidbodyConstraints.FreezeRotation;
            states.ignoreLayers = ~(1 << 9 | 1 << 3);
            if (initActionsBatch != null)
            {
                initActionsBatch.Execute(states);
            }
        }
    }
}
