using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
    [CreateAssetMenu(menuName ="Actions/State Actions/Ragdoll Status")]
    public class RagdollStatus : StateActions
    {
        public bool enableRagdoll;

        public override void Execute(StateManager states)
        {
            if (enableRagdoll)
            {
                for(int i = 0; i< states.ragdollCols.Count; i++)
                {
                    states.ragdollCols[i].isTrigger = false;
                    states.ragdollRB[i].isKinematic = false;
                }
            }
            else
            {
                Rigidbody[] rigidbodies = states.mTransform.GetComponentsInChildren<Rigidbody>();
                foreach (Rigidbody r in rigidbodies)
                {
                    if (r == states.rigidbody)
                        continue;

                    r.isKinematic = true;
                    states.ragdollRB.Add(r);

                    Collider col = r.GetComponent<Collider>();
                    col.isTrigger = true;
                    states.ragdollCols.Add(col);
                }
            }

        }

    }
}
