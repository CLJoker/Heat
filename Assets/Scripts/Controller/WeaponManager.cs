using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
    [System.Serializable]
    public class WeaponManager
    {
        public string mw_id;
        public string sw_id;

        RuntimeWeapon curWeapon;

        public RuntimeWeapon GetCurrentWeapon()
        {
            return curWeapon;
        }

        public void SetCurrentWeapon(RuntimeWeapon wp)
        {
            curWeapon = wp;
        }

        public RuntimeWeapon m_weapon;
        public RuntimeWeapon s_weapon;
    }
}
