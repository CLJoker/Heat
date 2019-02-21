﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
    [CreateAssetMenu(menuName = "Actions/State Actions/Init Client Controller")]
    public class InitClientController : StateActions
    {
        public override void Execute(StateManager states)
        {
            states.mTransform = states.transform;
            states.anim = states.mTransform.GetComponentInChildren<Animator>();
            states.rigidbody = states.mTransform.GetComponent<Rigidbody>();

            states.rigidbody.isKinematic = true;
        }
    }
}