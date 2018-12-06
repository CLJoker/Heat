using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SO;

namespace SA
{
    [CreateAssetMenu(menuName = "Actions/Mono Actions/Handle Transform LocalPosition On Axis")]
    public class HandleTransformLocalPositionOnAxis : Action
    {
        public TransformVariable targetTransform;

        public BoolVariable targetBool;
        public float defaultValue;
        public float affectedValue;
        public Axis targetAxis;

        public float speed = 9;
        public float actualValue;

        public override void Execute()
        {
            if (targetTransform.value == null)
                return;

            float targetValue = defaultValue;

            if (targetBool.value)
            {
                targetValue = affectedValue;
            }

            actualValue = Mathf.Lerp(actualValue, targetValue, speed * Time.deltaTime);
            Vector3 targetPosition = targetTransform.value.localPosition;
            switch (targetAxis)
            {
                case Axis.x:
                    targetPosition.x = actualValue;
                    break;
                case Axis.y:
                    targetPosition.y = actualValue;
                    break;
                case Axis.z:
                    targetPosition.z = actualValue;
                    break;
                default:
                    break;
            }

            targetTransform.value.localPosition = targetPosition;
        }

        public enum Axis
        {
            x,y,z
        }

    }
}
