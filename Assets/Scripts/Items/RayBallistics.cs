using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
    [CreateAssetMenu(menuName = "Ballistics/Ray")]
    public class RayBallistics : Ballistics
    {
        public override void Execute(StateManager states, Weapon wp)
        {
            Vector3 origin = wp.runTime.modelInstance.transform.position;
            Vector3 dir = states.movementValues.aimPosition - origin;
            Ray ray = new Ray(origin, dir);
            RaycastHit hit;
            if(Physics.Raycast(ray, out hit, 100, states.ignoreLayers))
            {
                IHittable isHittbale = hit.transform.GetComponentInParent<IHittable>();

                if (isHittbale == null)
                {
                    GameObject hitParticle = GameManagers.GetObjPool().RequestObject("Sparks");
                    Quaternion rot = Quaternion.LookRotation(-dir);
                    hitParticle.transform.position = hit.point;
                    hitParticle.transform.rotation = rot;
                }
                else
                {
                    isHittbale.OnHit(states, wp, dir, hit.point);
                }
            }

            MultiplayerManager mm = MultiplayerManager.singleton;
            if(mm != null)
            {
                mm.BroadcastShootWeapon(states, dir, origin);
            }
        }

        public void ClientShoot(StateManager states, Vector3 dir, Vector3 origin)
        {
            Ray ray = new Ray(origin, dir);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, 100))
            {
                IHittable isHittbale = hit.transform.GetComponentInParent<IHittable>();

                if (isHittbale == null)
                {
                    GameObject hitParticle = GameManagers.GetObjPool().RequestObject("Sparks");
                    Quaternion rot = Quaternion.LookRotation(-dir);
                    hitParticle.transform.position = hit.point;
                    hitParticle.transform.rotation = rot;
                }
                else
                {
                    isHittbale.OnHit(states, states.inventory.currentWeapon, dir, hit.point);
                }
            }
        }
    }
}
