using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
    [CreateAssetMenu(menuName = "Characters/Mask")]
    public class Mask : ScriptableObject
    {
        public CharObject obj;
        public bool enableHair;
        public bool enableEyebrow;
    }
}
