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

        public void InstantiateController(Vector3 pos, Quaternion rot)
        {
            GameObject inputHandler = Instantiate(Resources.Load("Input Handler")) as GameObject;

            object[] data = new object[2];
            data[0] = photonId;
            data[1] = photonView.instantiationData[0];

            GameObject go = PhotonNetwork.Instantiate("MultiplayerController", pos, rot, 0, data);
        }
    }
}
