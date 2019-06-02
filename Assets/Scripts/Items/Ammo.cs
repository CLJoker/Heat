using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
    [CreateAssetMenu(menuName = "Items/Ammo")]
    public class Ammo : ScriptableObject
    {
        public int carryingAmount;
        public int carryingTotal;
        public int damageValue = 20;

        public virtual void OnHit()
        {

        }
    }
}
