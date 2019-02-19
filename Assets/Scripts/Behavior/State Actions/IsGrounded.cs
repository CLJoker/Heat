using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace SA
{
    [CreateAssetMenu(menuName = "Actions/State Actions/IsGrounded")]
    public class IsGrounded : StateActions
    {
        public float groundedDis = 1.4f;
        public float onAirDis = 1f;

        public override void Execute(StateManager states)
        {
            Vector3 origin = states.transform.position;
            origin.y += 0.7f;
            Vector3 dir = -Vector3.up;
            float dis = groundedDis;
            if (!states.isGrounded)
                dis = onAirDis;


            RaycastHit hit;
            Debug.DrawRay(origin, dir * dis);

            if(Physics.SphereCast(origin, 0.3f, dir, out hit, dis, states.ignoreLayers))
            {
                states.isGrounded = true;
            }
            else
            {
                states.isGrounded = false;
            }
        }

    }
}
