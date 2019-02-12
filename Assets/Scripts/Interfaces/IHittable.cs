using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
    interface IHittable
    {
        void OnHit(StateManager shooter, Weapon wp, Vector3 dir, Vector3 pos);
    }
}
