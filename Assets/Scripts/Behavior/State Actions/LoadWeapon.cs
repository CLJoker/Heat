using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
    [CreateAssetMenu(menuName ="Actions/State Actions/Load Weapon")]
    public class LoadWeapon : StateActions
    {
        public override void Execute(StateManager states)
        {
            ResourcesManager rm = GameManagers.GetResourcesManager();

            Weapon targetWeapon = (Weapon) rm.GetItemInstance(states.inventory.weaponID);
            states.inventory.currentWeapon = targetWeapon;
            targetWeapon.Init();

            Transform rightHand = states.anim.GetBoneTransform(HumanBodyBones.RightHand);
            targetWeapon.runTime.modelInstance.transform.parent = rightHand;
            targetWeapon.runTime.modelInstance.transform.localPosition = Vector3.zero;
            targetWeapon.runTime.modelInstance.transform.localEulerAngles = Vector3.zero;
            targetWeapon.runTime.modelInstance.transform.localScale = Vector3.one;

            states.animHook.LoadWeapon(targetWeapon);
        }

    }
}
