using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
    [CreateAssetMenu(menuName = "Actions/State Actions/Vault Movement")]
    public class VaultMovement : StateActions
    {
        public override void Execute(StateManager states)
        {
            VaultData v = states.vaultData;
            
            if(!v.isInit)
            {
                v.vaultT = 0;
                v.isInit = true;

                Vector3 dir = v.endingPosition - v.startPosition;
                dir.y = 0;
                Quaternion rot = Quaternion.LookRotation(dir);
                states.mTransform.rotation = rot;
            }

            float actualSpeed = (states.delta * v.vaultSpeed) / v.animLength;

            v.vaultT += actualSpeed;

            if(v.vaultT > 1)
            {
                v.isInit = false;
                states.isVaulting = false;
            }

            Vector3 targetPosition = Vector3.Lerp(v.startPosition, v.endingPosition, v.vaultT);
            states.mTransform.position = targetPosition;
        }
    }
}
