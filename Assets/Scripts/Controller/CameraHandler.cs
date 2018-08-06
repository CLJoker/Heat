using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
    public class CameraHandler : MonoBehaviour
    {
        public Transform camTrans;
        public Transform target;
        public Transform pivot;
        public Transform mTransform;
        public bool leftPivot;
        float delta;

        float mouseX;
        float mouseY;
        float smoothX;
        float smoothY;
        float smoothXVelocity;
        float smoothYVelocity;
        float lookAngle;
        float tiltAngle;

        public CameraValues camValues;

        StatesManager targetStates;
        public void Init(InputHandler inp)
        {
            targetStates = inp.states;
            target = targetStates.mTransform;
            mTransform = this.transform;

        }

        public void FixedTick(float d)
        {
            delta = d;

            if (target == null)
                return;

            HandlePositions();
            HandleRotation();

            float speed = camValues.moveSpeed;
            if(targetStates.states.isAiming)
            {
                speed = camValues.aimSpeed;
            }

            Vector3 targetPosition = Vector3.Lerp(mTransform.position, target.position, delta * speed);
            mTransform.position = targetPosition;
        }

        void HandlePositions()
        {
            float targetX = camValues.normalX;
            float targetZ = camValues.normalZ;
            float targetY = camValues.normalY;

            if(targetStates.states.isCrouching)
            {
                targetY = camValues.crouchY;
            }

            if(targetStates.states.isAiming)
            {
                targetX = camValues.aimX;
                targetZ = camValues.aimZ;
            }

            if(leftPivot)
            {
                targetX = -targetX;
            }

            Vector3 newPivotPosition = pivot.localPosition;
            newPivotPosition.x = targetX;
            newPivotPosition.y = targetY;

            Vector3 newCamPosition = camTrans.localPosition;
            newCamPosition.z = targetZ;
            float t = delta * camValues.adaptSpeed;
            pivot.localPosition = Vector3.Lerp(pivot.localPosition, newPivotPosition, t);
            camTrans.localPosition = Vector3.Lerp(camTrans.localPosition, newCamPosition, t);
        }

        void HandleRotation()
        {
            mouseX = Input.GetAxis("Mouse X");
            mouseY = Input.GetAxis("Mouse Y");

            if(camValues.turnSmooth > 0)
            {
                smoothX = Mathf.SmoothDamp(smoothX, mouseX, ref smoothXVelocity, camValues.turnSmooth);
                smoothY = Mathf.SmoothDamp(smoothY, mouseY, ref smoothYVelocity, camValues.turnSmooth);
            }
            else
            {
                smoothX = mouseX;
                smoothY = mouseY;
            }

            lookAngle += smoothX * camValues.y_rotate_speed;
            Quaternion targetRot = Quaternion.Euler(0, lookAngle, 0);
            mTransform.rotation = targetRot;

            tiltAngle -= smoothY * camValues.x_rotate_speed;
            tiltAngle = Mathf.Clamp(tiltAngle, camValues.minAngle, camValues.maxAngle);
            pivot.localRotation = Quaternion.Euler(tiltAngle, 0, 0);
        }
    }
}

