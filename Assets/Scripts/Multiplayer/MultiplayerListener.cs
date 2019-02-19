using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
    public class MultiplayerListener : Photon.MonoBehaviour
    {
        public State local;
        public StateActions initLocalPlayer;
        public State client;
        public StateActions initClientPlayer;

        StateManager states;

        void OnPhotonInstantiate(PhotonMessageInfo info)
        {
            states = GetComponent<StateManager>();

            if (photonView.isMine)
            {
                states.SetCurrentState(local);
                initLocalPlayer.Execute(states);
            }
            else
            {
                states.SetCurrentState(client);
                initClientPlayer.Execute(states);
            }
        }

        public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
        {

        }

    }
}
