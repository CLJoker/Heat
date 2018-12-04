using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SO;

namespace SA
{
    [CreateAssetMenu(menuName ="Actions/Mono Actions/Camera Zoom")]
    public class HandleCameraZoom : Action
    {

        public SO.TransformVariable actualCameraTrans;

        public InputButton aimInput;

        public float defaultZ;
        float actualZ;
        public float zoomZ;

        public float speed = 9;

        public override void Execute()
        {
            if (actualCameraTrans.value == null)
                return;

            float targetZ = defaultZ;

            if (aimInput.isPressed)
            {
                targetZ = zoomZ;
            }

            actualZ = Mathf.Lerp(actualZ, targetZ, speed * Time.deltaTime);

            Vector3 targetPosition = Vector3.zero;
            targetPosition.z = targetZ;
            actualCameraTrans.value.localPosition = targetPosition;
        }

    }
}
