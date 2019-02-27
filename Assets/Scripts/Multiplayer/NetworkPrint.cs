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

        public void InstantiateController(int spawnIndex)
        {
            GameObject inputHandler = Instantiate(Resources.Load("Input Handler")) as GameObject;
            PhotonNetwork.Instantiate("MultiplayerController", Vector3.zero, Quaternion.identity, 0, photonView.instantiationData);
        }
    }
}
