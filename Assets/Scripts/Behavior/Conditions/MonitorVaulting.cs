using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
    [CreateAssetMenu(menuName = "Conditions/Monitor Vaulting")]
    public class MonitorVaulting : Condition
    {
        public float origin1Offset = 1;
        public float rayForwardDis = 1;
        public float rayHigherForwardDis = 1;
        public float origin2Offset = 0.2f;
        public float rayDownDis = 1.5f;
        public float vaultOffsetPosition = 2;

        public AnimationClip vaultWalkClip;

        public override bool CheckCondition(StateManager state)
        {
            bool result = false;

            RaycastHit hit;
            Vector3 origin = state.mTransform.position;
            origin.y += origin1Offset;
            Vector3 direction = state.mTransform.forward;


            Debug.DrawRay(origin, direction * rayForwardDis);
            if(Physics.Raycast(origin, direction, out hit, rayForwardDis, state.ignoreLayers))
            {
                Vector3 origin2 = origin;
                origin2.y += origin2Offset;

                Vector3 firstHit = hit.point;
                firstHit.y -= origin1Offset;

                Vector3 normalDir = -hit.normal;

                Debug.DrawRay(origin2, direction * rayForwardDis);
                if(Physics.Raycast(origin2, direction, out hit, rayDownDis, state.ignoreLayers))
                {

                }
                else
                {
                    Vector3 origin3 = origin2 + (direction * rayHigherForwardDis);
                    Debug.DrawRay(origin3, -Vector3.up * rayDownDis);
                    if(Physics.Raycast(origin3, -Vector3.up, out hit, rayDownDis, state.ignoreLayers))
                    {
                        result = true;
                        state.anim.SetBool(state.hashes.isInteracting, true);
                        state.anim.CrossFade(state.hashes.VaultWalk, 0.2f);
                        state.vaultData.animLength = vaultWalkClip.length;
                        state.vaultData.isInit = false;
                        state.isVaulting = true;

                        state.vaultData.startPosition = state.mTransform.position;

                        Vector3 endPosition = firstHit;
                        endPosition += normalDir * vaultOffsetPosition;

                        state.vaultData.endingPosition = endPosition;
                    }
                }
            }

            return result;
        }
    }
}
