using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
    public class MainMenu_InventoryManager : MonoBehaviour
    {
        public Transform[] cameraPosition;
        public int camIndex;

        public new Transform camera;
        float t;
        public float speed = 5;
        Vector3 startPos;
        Vector3 endPos;
        Quaternion startRot;
        Quaternion endRot;
        bool isInit;
        bool isLerping;

        private void OnEnable()
        {
            camera.position = cameraPosition[0].position;
            camera.rotation = cameraPosition[0].rotation;
        }

        private void Update()
        {
            MoveCameraToPosition();
        }

        public void AssignCameraPosition(int index)
        {
            camIndex = index;
            isLerping = true;
            isInit = false;
        }

        void MoveCameraToPosition()
        {
            if (!isLerping)
                return;

            if (!isInit)
            {
                startPos = camera.position;
                endPos = cameraPosition[camIndex].position;
                startRot = camera.rotation;
                endRot = cameraPosition[camIndex].rotation;
                isInit = true;
                t = 0;
            }

            t += Time.deltaTime * speed;
            if(t > 1)
            {
                t = 1;
                isInit = false;
                isLerping = false;

            }

            Vector3 tp = Vector3.Lerp(startPos, endPos, t);
            Quaternion tr = Quaternion.Slerp(startRot, endRot, t);

            camera.position = tp;
            camera.rotation = tr;
        }

    }
}
