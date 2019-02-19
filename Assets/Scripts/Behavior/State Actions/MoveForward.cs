using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace SA
{
    [CreateAssetMenu(menuName ="Actions/State Actions/Move Forward")]
    public class MoveForward : StateActions
    {
        public float frontRayOffset = 0.5f;
        public float movementSpeed = 2;
        public float adaptSpeed = 10;

        public override void Execute(StateManager states)
        {
            float frontY = 0;
            RaycastHit hit;
            Vector3 origin = states.mTransform.position + (states.mTransform.forward * frontRayOffset);
            origin.y += 0.5f;

            if(Physics.Raycast(origin, -Vector3.up, out hit, 1, states.ignoreLayers))
            {
                float y = hit.point.y;
                frontY = y - states.mTransform.position.y;
            }

            Vector3 currentVelocity = states.rigidbody.velocity;
            Vector3 targetVelocity = states.mTransform.forward * states.movementValues.moveAmount * movementSpeed;

            if (states.isGrounded)
            {
                float moveAmount = states.movementValues.moveAmount;

                if(moveAmount > 0.1f)
                {
                    states.rigidbody.isKinematic = false;
                    states.rigidbody.drag = 0;
                    if(Mathf.Abs(frontY) > 0.02f)
                    {
                        targetVelocity.y = ((frontY > 0) ? frontY + 0.2f : frontY) * movementSpeed;
                    }
                }
                else
                {
                    float abs = Mathf.Abs(frontY);

                    states.rigidbody.isKinematic = false;
                    targetVelocity.y = 0;
                    states.rigidbody.drag = 4;
                }
            }
            else
            {
                states.rigidbody.isKinematic = false;
                states.rigidbody.drag = 4;
                targetVelocity.y = currentVelocity.y;
            }

            states.rigidbody.velocity = Vector3.Lerp(currentVelocity, targetVelocity, states.delta * adaptSpeed);
        }
    }
}
