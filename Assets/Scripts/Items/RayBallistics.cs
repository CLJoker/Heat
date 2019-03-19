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

            RaycastHit[] hits;
            hits = Physics.RaycastAll(origin, dir, 100);
            if (hits == null)
                return;
            if (hits.Length == 0)
                return;

            RaycastHit closestHit;
            closestHit = GetClosestHit(origin, hits, states.photonId);
            IHittable isHittbale = closestHit.transform.GetComponentInParent<IHittable>();

            if (isHittbale == null)
            {
                GameObject hitParticle = GameManagers.GetObjPool().RequestObject("Sparks");
                Quaternion rot = Quaternion.LookRotation(-dir);
                hitParticle.transform.position = closestHit.point;
                hitParticle.transform.rotation = rot;
            }
            else
            {
                isHittbale.OnHit(states, wp, dir, closestHit.point);
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

            RaycastHit[] hits;
            hits = Physics.RaycastAll(origin, dir, 100);
            if (hits == null)
                return;
            if (hits.Length == 0)
                return;

            RaycastHit closestHit;
            closestHit = GetClosestHit(origin, hits, states.photonId);

            IHittable isHittbale = closestHit.transform.GetComponentInParent<IHittable>();

            if (isHittbale == null)
            {
                GameObject hitParticle = GameManagers.GetObjPool().RequestObject("Sparks");
                Quaternion rot = Quaternion.LookRotation(-dir);
                hitParticle.transform.position = closestHit.point;
                hitParticle.transform.rotation = rot;
            }
            else
            {
                isHittbale.OnHit(states, states.inventory.currentWeapon, dir, closestHit.point);
            }
        }

        public static RaycastHit GetClosestHit(Vector3 origin, RaycastHit[] hits, int shooter)
        {
            int closest = 0;

            float minDist = float.MaxValue;
            for(int i=0; i < hits.Length; i++)
            {
                float tempDist = Vector3.Distance(origin, hits[i].point);
                if(tempDist < minDist)
                {
                    StateManager states = hits[i].transform.GetComponentInParent<StateManager>();
                    if(states != null)
                    {
                        if(states.photonId == shooter)
                        {
                            continue;
                        }
                    }

                    minDist = tempDist;
                    closest = i;
                }
            }

            return hits[closest];
        }
    }
}
