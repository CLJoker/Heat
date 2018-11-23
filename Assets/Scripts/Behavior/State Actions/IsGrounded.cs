using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace SA
{
    [CreateAssetMenu(menuName = "Actions/State Actions/IsGrounded")]
    public class IsGrounded : StateActions
    {
        public override void Execute(StateManager states)
        {
            Vector3 origin = states.transform.position;
            origin.y += 0.7f;
            Vector3 dir = -Vector3.up;
            float dis = 1.4f;
            RaycastHit hit;
            Debug.DrawRay(origin, dir * dis);

            if(Physics.Raycast(origin, dir, out hit, dis, states.ignoreLayers))
            {
                Vector3 targetPosition = hit.point;
                targetPosition.x = states.mTransform.position.x;
                targetPosition.z = states.mTransform.position.z;

                states.mTransform.position = targetPosition;
            }
        }

    }
}
