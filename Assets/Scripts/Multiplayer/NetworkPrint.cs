using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
    public class NetworkPrint : Photon.MonoBehaviour
    {
        public int photonId;
        public bool isLocal;

        void OnPhotonInstantiate(PhotonMessageInfo info)
        {
            MultiplayerManager mm = MultiplayerManager.singleton;

            photonId = photonView.ownerId;
            isLocal = photonView.isMine;

            mm.AddNewPlayer(this);
        }
    }
}
