using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
    public class ExplodingBarrel : MonoBehaviour, IHittable
    {
        public string targetParticle = "BloodSplat_FX";

        public void OnHit(StateManager shooter, Weapon wp, Vector3 dir, Vector3 pos)
        {
            GameObject hitParticle = GameManagers.GetObjPool().RequestObject(targetParticle);
            Quaternion rot = Quaternion.LookRotation(-dir);
            hitParticle.transform.position = pos;
            hitParticle.transform.rotation = rot;
        }
    }
}
