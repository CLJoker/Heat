using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
    [CreateAssetMenu(menuName ="Actions/State Actions/Init Actions/Init Animator Hook")]
    public class InitAnimatorHook : StateActions
    {
        public override void Execute(StateManager states)
        {
            GameObject model = states.anim.gameObject;

            states.animHook = model.AddComponent<AnimatorHook>();
            states.animHook.Init(states);
        }

    }
}
