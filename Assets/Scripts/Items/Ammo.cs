using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
    [CreateAssetMenu(menuName = "Items/Ammo")]
    public class Ammo : ScriptableObject
    {
        public int carryingAmount;

        public virtual void OnHit()
        {

        }
    }
}
