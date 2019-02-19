using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SO;

namespace SA.MonoAcions
{
    [CreateAssetMenu(menuName ="Actions/Mono Actions/RotateViaInput")]
    public class RotateViaInput : Action
    {
        public InputAxis targetInput;
        public TransformVariable targetTransform;

        public float angle;
        public float speed;
        public bool negative;
        public bool clamp;
        public float minClamp = -35;
        public float maxClamp = 35;
        public RotateAxis targetAxis;
        public FloatVariable delta;
        
        public enum RotateAxis
        {
            x,y,z
        }

        public override void Execute()
        {
            float t = delta.value * speed;

            if (!negative)
                angle = Mathf.Lerp(angle, angle + targetInput.value, t);
            else
                angle = Mathf.Lerp(angle, angle - targetInput.value, t);

            if (clamp)
            {
                angle = Mathf.Clamp(angle, minClamp, maxClamp);
            }

            switch (targetAxis)
            {
                case RotateAxis.x:
                    targetTransform.value.localRotation = Quaternion.Euler(angle, 0, 0);
                    break;
                case RotateAxis.y:
                    targetTransform.value.localRotation = Quaternion.Euler(0, angle, 0);
                    break;
                case RotateAxis.z:
                    targetTransform.value.localRotation = Quaternion.Euler(0, 0, angle);
                    break;
                default:
                    break;
            }
        }
    }
}
