using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
    [CreateAssetMenu(menuName ="Controller/Camera Values")]
    public class CameraValues : ScriptableObject
    {
        public float turnSmooth = 0.1f;
        public float moveSpeed = 9f;
        public float x_rotate_speed = 8;
        public float y_rotate_speed = 8;
        public float minAngle = -35;
        public float maxAngle = 35;
        public float normalX;
        public float normalY;
        public float normalZ = -3f;
        public float aimZ = -0.5f;
        public float aimX;
        public float crouchY;
        public float adaptSpeed = 9f;
        public float aimSpeed = 25;
    }
}

