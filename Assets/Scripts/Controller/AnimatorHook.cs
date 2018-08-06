using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
    public class AnimatorHook : MonoBehaviour
    {
        Animator anim;
        StatesManager states;

        float m_h_weight;
        float o_h_weight;
        float l_weight;
        float b_weight;

        Transform rh_target;
        public Transform lh_target;
        Transform shoulder;
        Transform aimPivot;

        public bool disable_o_h;
        public bool disable_m_h;

        public void Init(StatesManager st)
        {
            states = st;

            anim = states.anim;

            shoulder = anim.GetBoneTransform(HumanBodyBones.RightShoulder).transform;
            aimPivot = new GameObject().transform;
            aimPivot.name = "aim pivot";
            aimPivot.transform.parent = states.transform;

            rh_target = new GameObject().transform;
            rh_target.name = "right hand target";
            rh_target.parent = aimPivot;

            states.inp.aimPosition = states.transform.position + transform.forward * 15;
            states.inp.aimPosition.y += 1.4f;
        }

        void HandleShoulder()
        {
            HandleShoulderPosition();
            HandleShoulderRotation();
        }

        void HandleShoulderPosition()
        {
            aimPivot.position = shoulder.position;
        }

        void HandleShoulderRotation()
        {
            Vector3 targetDir = states.inp.aimPosition - aimPivot.position;
            if(targetDir == Vector3.zero)
            {
                targetDir = aimPivot.forward;
            }
            Quaternion tr = Quaternion.LookRotation(targetDir);
            aimPivot.rotation = Quaternion.Slerp(aimPivot.rotation, tr, states.delta * 15);
        }
    }
}

