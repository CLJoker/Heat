using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
    [CreateAssetMenu(menuName ="Actions/State Actions/Shoot Action")]
    public class ShootAction : StateActions
    {
        public override void Execute(StateManager states)
        {
            if(states.inventory.currentWeapon.currentBullets < states.inventory.currentWeapon.megazineBullets 
                && states.inventory.currentWeapon.ammoType.carryingAmount > 0)
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
                            ReloadCurrentWeapon(states.inventory.currentWeapon);
                        }
                    }
                    return;
                }
            }
            else
            {
                states.isReloading = false;
            }


            if (states.isShooting)
            {
                states.isShooting = false;
                Weapon w = states.inventory.currentWeapon;
                if(w.currentBullets > 0)
                {
                    if (Time.realtimeSinceStartup - w.runTime.weaponHook.lastFired > w.fireRate)
                    {
                        w.runTime.weaponHook.lastFired = Time.realtimeSinceStartup;
                        w.runTime.weaponHook.Shoot();

                        states.animHook.RecoilAnim();
                        w.currentBullets--;
                        if (w.currentBullets < 0)
                            w.currentBullets = 0;
                    }
                }
                else
                {
                    states.isReloading = true;
                }
            }
        }


        public void ReloadCurrentWeapon(Weapon currentWeapon)
        {
            int target = currentWeapon.megazineBullets;
            if (target > currentWeapon.ammoType.carryingAmount)
            {
                target = currentWeapon.megazineBullets - currentWeapon.ammoType.carryingAmount;
            }
            currentWeapon.ammoType.carryingAmount -= target;
            currentWeapon.currentBullets = target;
        }
    }
}
