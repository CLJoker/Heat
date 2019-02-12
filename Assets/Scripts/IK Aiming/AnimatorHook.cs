using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
    public class AnimatorHook : MonoBehaviour
    {
        Animator anim;
        StateManager states;

        float m_h_weight;
        float o_h_weight;
        float l_weight;
        float b_weight;

        Transform rh_target;
        public Transform lh_target;
        public Transform shoulder;
        public Transform aimPivot;
        Vector3 lookDir;
        Weapon currentWeapon;

        public void Init(StateManager st)
        {
            states = st;
            anim = GetComponent<Animator>();

            if (shoulder == null)
                shoulder = anim.GetBoneTransform(HumanBodyBones.RightShoulder).transform;

            aimPivot = new GameObject().transform;
            aimPivot.name = "aim pivot";
            aimPivot.transform.parent = states.transform;

            rh_target = new GameObject().transform;
            rh_target.name = "right hand target";
            rh_target.parent = aimPivot;

            states.movementValues.aimPosition = states.mTransform.position + transform.forward * 15;
            states.movementValues.aimPosition.y += 1.4f;

        }

        private void OnAnimatorMove()
        {
            lookDir = states.movementValues.aimPosition - aimPivot.position;
            HandleShoulder();
        }

        public void LoadWeapon(Weapon w)
        {
            currentWeapon = w;
            rh_target.localPosition = w.rightHandPosition.value;
            rh_target.localEulerAngles = w.rightHandEuler.value;
            lh_target = w.runTime.weaponHook.leftHandIK;

            basePosition = w.rightHandPosition.value;
            baseRotation = w.rightHandEuler.value;
        }

        void HandleShoulder()
        {
            HandleShoulderPositon();
            HandleShoulderRotation();
        }

        void HandleShoulderPositon()
        {
            aimPivot.position = shoulder.position;
        }

        void HandleShoulderRotation()
        {
            Vector3 targetDir = lookDir;

            if (targetDir == Vector3.zero)
                targetDir = aimPivot.forward;

            Quaternion tr = Quaternion.LookRotation(targetDir);
            aimPivot.rotation = Quaternion.Slerp(aimPivot.rotation, tr, states.delta * 15);
        }

        void HandleWeights()
        {
            if (states.isInteracting || states.isVaulting)
            {
                m_h_weight = 0;
                o_h_weight = 0;
                l_weight = 0;
                return;
            }

            float t_l_weight = 0;
            float t_m_weight = 0;

            if (states.isAiming)
            {
                t_m_weight = 1;
                b_weight = 0.4f;
            }
            else
            {
                b_weight = 0.3f;
            }

            if (lh_target != null)
                o_h_weight = 1;
            else
                o_h_weight = 0;

            Vector3 ld = states.movementValues.aimPosition - aimPivot.position;
            float angle = Vector3.Angle(states.mTransform.forward, ld);
            if (angle < 75)
                t_l_weight = 1;
            else
                t_l_weight = 0;

            if (angle > 60)
                t_m_weight = 0;

            if (!states.isAiming)
            {

            }

            l_weight = Mathf.Lerp(l_weight, t_l_weight, states.delta * 1);
            m_h_weight = Mathf.Lerp(m_h_weight, t_m_weight, states.delta * 9);

        }

        void OnAnimatorIK()
        {
            HandleWeights();

            anim.SetLookAtWeight(l_weight, b_weight, 1, 1, 1);
            anim.SetLookAtPosition(states.movementValues.aimPosition);

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

        #region Recoil
        float recoilT;
        Vector3 offsetPosition;
        Vector3 offsetRotation;
        Vector3 basePosition;
        Vector3 baseRotation;
        bool recoilIsInit;

        public void RecoilAnim()
        {
            recoilIsInit = true;
            recoilT = 0;
            offsetPosition = Vector3.zero;
        }

        public void RecoilActual()
        {
            if (recoilIsInit)
            {
                recoilT += states.delta * 3;
                if(recoilT > 1)
                {
                    recoilT = 1;
                    recoilIsInit = false;
                }

                offsetPosition = Vector3.forward * currentWeapon.recoilZ.Evaluate(recoilT);
                offsetRotation = Vector3.right * 90 * -currentWeapon.recoilY.Evaluate(recoilT);

                rh_target.localPosition = basePosition + offsetPosition;
                rh_target.localEulerAngles = baseRotation + offsetRotation;
            }
        }

        public void Tick()
        {
            RecoilActual();
        }


        #endregion

    }
}
