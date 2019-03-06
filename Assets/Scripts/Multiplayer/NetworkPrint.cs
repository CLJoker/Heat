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

            object[] data = new object[2];
            data[0] = photonId;
            data[1] = photonView.instantiationData[0];

            PhotonNetwork.Instantiate("MultiplayerController", Vector3.zero, Quaternion.identity, 0, data);
        }
    }
}
