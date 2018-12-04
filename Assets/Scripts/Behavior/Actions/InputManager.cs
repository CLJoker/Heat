using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
    [CreateAssetMenu]
    public class InputManager : Action
    {
        public InputAxis horizontal;
        public InputAxis vertical;
        public InputButton aimInput;

        public float moveAmount;
        public Vector3 moveDirection;

        public SO.TransformVariable cameraTransform;
        public SO.TransformVariable pivotTransform;

        public StatesVariable playerStates;


        public override void Execute()
        {

            moveAmount = Mathf.Clamp01(Mathf.Abs(horizontal.value) + Math.Abs(vertical.value));

            if(cameraTransform.value != null)
            {
                moveDirection = cameraTransform.value.forward * vertical.value;
                moveDirection += cameraTransform.value.right * horizontal.value;
            }

            if(playerStates.value != null)
            {
                playerStates.value.movementValues.horizontal = horizontal.value;
                playerStates.value.movementValues.vertical = vertical.value;
                playerStates.value.movementValues.moveAmount = moveAmount;
                playerStates.value.movementValues.moveDirection = moveDirection;

                playerStates.value.isAiming = aimInput.isPressed;
                playerStates.value.movementValues.lookDirection = cameraTransform.value.forward;

                Ray ray = new Ray(pivotTransform.value.position, pivotTransform.value.forward);
                playerStates.value.movementValues.aimPosition = ray.GetPoint(100);
            }
        }
    }
}
