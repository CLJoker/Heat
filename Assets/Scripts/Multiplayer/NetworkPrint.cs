using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
    public class NetworkPrint : Photon.MonoBehaviour
    {
        public int photonId;
        public bool isLocal;

        string weaponId;
        string modelId;
        [HideInInspector]
        public string playerName;
        [HideInInspector]
        public byte[] avatarData;

        void OnPhotonInstantiate(PhotonMessageInfo info)
        {
            MultiplayerManager mm = MultiplayerManager.singleton;

            photonId = photonView.ownerId;
            isLocal = photonView.isMine;

            object[] data = photonView.instantiationData;
            weaponId = (string)data[0];
            modelId = (string)data[1];
            playerName = (string)data[2];
            avatarData = (byte[])data[3];
            mm.AddNewPlayer(this);
        }

        public void InstantiateController(Vector3 pos, Quaternion rot)
        {
            GameObject inputHandler = Instantiate(Resources.Load("Input Handler")) as GameObject;

            object[] data = new object[3];
            data[0] = photonId;
            data[1] = weaponId;
            data[2] = modelId;

            GameObject go = PhotonNetwork.Instantiate("MultiplayerController", pos, rot, 0, data);
            go.GetComponent<Rigidbody>().isKinematic = true;
        }
    }
}
