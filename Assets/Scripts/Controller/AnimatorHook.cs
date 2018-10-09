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
        public bool onDisableOh;

        Vector3 lookDir;

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

            //rh_target.transform.localPosition = new Vector3(0.231f, -0.123f, 0.167f);
            //rh_target.transform.localEulerAngles = new Vector3(-5.426f, 20.812f, -105.86f);

            //lh_target = new GameObject().transform;
            //lh_target.name = "left hand target";
            //lh_target.parent = rh_target;
            //lh_target.transform.localPosition = new Vector3(-0.002f, -0.169f, 0.427f);
            //lh_target.transform.localEulerAngles = new Vector3(0.02f, -0.044f, 265.331f);

            states.inp.aimPosition = states.transform.position + transform.forward * 15;
            states.inp.aimPosition.y += 1.4f;
        }

        public void EquipWeapon(RuntimeWeapon rw)
        {
            Weapon w = rw.w_actual;
            lh_target = rw.w_hook.leftHandIK;

            rh_target.localPosition = w.m_h_ik.pos;
            rh_target.localEulerAngles = w.m_h_ik.rot;
            onDisableOh = rw.w_actual.onIdleDisableOh;
            basePosition = w.m_h_ik.pos;
            baseRotation = w.m_h_ik.rot;
        }

        private void OnAnimatorMove()
        {
            lookDir = states.inp.aimPosition - aimPivot.position;
            HandleShoulder();
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
            Vector3 targetDir = lookDir;
            if (targetDir == Vector3.zero)
            {
                targetDir = aimPivot.forward;
            }
            Quaternion tr = Quaternion.LookRotation(targetDir);
            aimPivot.rotation = Quaternion.Slerp(aimPivot.rotation, tr, states.delta * 15);
        }

        void HandleWeight()
        {
            if(states.states.isInteracting)
            {
                m_h_weight = 0;
                o_h_weight = 0;
                l_weight = 0;
                return;
            }

            float t_l_weight = 0;
            float t_m_weight = 0;

            if(states.states.isAiming)
            {
                t_m_weight = 1;
                b_weight = 0.4f;
            }
            else
            {
                b_weight = 0.3f;
            }

            if (disable_m_h)
                t_m_weight = 0;

            if (lh_target != null)
                o_h_weight = 1;
            else
                o_h_weight = 0;

            if (disable_o_h)
                o_h_weight = 0;


            Vector3 ld = states.inp.aimPosition - states.mTransform.position;
            float angle = Vector3.Angle(states.mTransform.forward, ld);
            if (angle < 76)
            {
                t_l_weight = 1;
            }
            else
            {
                t_l_weight = 0;
            }

            if(angle > 45)
            {
                t_m_weight = 0;
            }

            if(!states.states.isAiming && onDisableOh)
            {
                o_h_weight = 0;
            }

            l_weight = Mathf.Lerp(l_weight, t_l_weight, states.delta * 3);
            m_h_weight = Mathf.Lerp(m_h_weight, t_m_weight, states.delta * 9);
        }

        private void OnAnimatorIK(int layerIndex)
        {
            HandleWeight();
            anim.SetLookAtWeight(l_weight, b_weight, 1, 1, 1);
            anim.SetLookAtPosition(states.inp.aimPosition);

            if(lh_target != null)
            {
                UpdateIK(AvatarIKGoal.LeftHand, lh_target, o_h_weight);
            }

            UpdateIK(AvatarIKGoal.RightHand, rh_target, m_h_weight);
        }

        void UpdateIK(AvatarIKGoal goal, Transform t, float w)
        {
            anim.SetIKPositionWeight(goal, w);
            anim.SetIKRotationWeight(goal, w);
            anim.SetIKPosition(goal, t.position);
            anim.SetIKRotation(goal, t.rotation);
        }

        public void Tick()
        {
            RecoilActual();
        }

        #region Recoil
        float recoilT;
        Vector3 offsetPosition;
        Vector3 offsetRotation;
        Vector3 basePosition;
        Vector3 baseRotation;
        bool recoilIsInit;

        public void RecoilAnim()
        {
            if(!recoilIsInit)
            {
                recoilIsInit = true;
                recoilT = 0;
                offsetPosition = Vector3.zero;
            }
        }

        public void RecoilActual()
        {
            if(recoilIsInit)
            {
                recoilT += states.delta * 3;
                if(recoilT > 1)
                {
                    recoilT = 1;
                    recoilIsInit = false;
                }

                offsetPosition = Vector3.forward * states.w_manager.GetCurrentWeapon().w_actual.recoilZ.Evaluate(recoilT);
                offsetRotation = Vector3.right * 90 * -states.w_manager.GetCurrentWeapon().w_actual.recoilY.Evaluate(recoilT);
                rh_target.localPosition = basePosition + offsetPosition ;
                rh_target.localEulerAngles = baseRotation + offsetRotation;
            }
        }
        #endregion
    }
}

