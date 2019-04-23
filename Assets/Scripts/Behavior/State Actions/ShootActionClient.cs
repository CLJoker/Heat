using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
    [CreateAssetMenu(menuName = "Actions/State Actions/Shoot Action Client")]
    public class ShootActionClient : StateActions
    {
        public override void Execute(StateManager states)
        {
            if (states.isReloading)
            {
                if (!states.isInteracting)
                {
                    states.isInteracting = true;
                    states.PlayAnimation("Rifle Reload");
                    states.anim.SetBool("isInteracting", true);
                }
                else
                {
                    if (!states.anim.GetBool("isInteracting"))
                    {
                        states.isReloading = false;
                        states.isInteracting = false;
                        //ReloadCurrentWeapon(states.inventory.currentWeapon);
                    }
                }
            }

            if (states.isShooting && !states.isReloading && states.isAiming)
            {
                states.isShooting = false;
                Weapon w = states.inventory.currentWeapon;
                w.runTime.weaponHook.Shoot();

                states.animHook.RecoilAnim();
            }

        }

    }
}
