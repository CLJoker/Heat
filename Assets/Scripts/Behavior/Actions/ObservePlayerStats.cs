using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SO;

namespace SA
{
    [CreateAssetMenu(menuName = "Actions/Mono Actions/Observer Player Stats")]
    public class ObservePlayerStats : Action
    {
        public StatesVariable states;

        public GameEvent healthUpdate;
        public IntVariable playerHealth;

        public GameEvent curAmmoUpdate;
        public IntVariable curAmmo;

        public override void Execute()
        {
            if (states.value == null)
                return;

            if (states.value.healthChangedFlag)
            {
                states.value.healthChangedFlag = false;
                playerHealth.value = states.value.stats.health;
                healthUpdate.Raise();
            }

            if(curAmmo.value != states.value.inventory.currentWeapon.currentBullets)
            {
                curAmmo.value = states.value.inventory.currentWeapon.currentBullets;
                curAmmoUpdate.Raise();
            }
        }

    }
}