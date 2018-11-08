using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
    public class RaplaceEventOnSession : MonoBehaviour
    {
        public SessionManager sess;
        public GameEvent targetEvent;

        public void ReplaceSceneSingleEvent()
        {
            sess.onSceneLoadedSingle = targetEvent;
        }

        public void ReplaceSceneAdditivEvent()
        {
            sess.onSceneLoadedAdditive = targetEvent;
        }

    }
}
