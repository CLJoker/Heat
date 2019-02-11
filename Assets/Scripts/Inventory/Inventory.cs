using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
    [System.Serializable]
    public class Inventory
    {
        public string weaponID;
        public Weapon currentWeapon;

        public void ReloadCurrentWeapon()
        {
            int target = currentWeapon.megazineBullets;
            currentWeapon.currentBullets = target;
        }
    }
}
