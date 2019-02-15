using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
    public class MultiplayerManager : Photon.MonoBehaviour
    {
        MultiplayerReferences mRef;

        public static MultiplayerManager singleton;


        void OnPhotonInstantiate(PhotonMessageInfo info)
        {
            singleton = this;
            mRef = new MultiplayerReferences();

            if (PhotonNetwork.isMasterClient)
            {
                InstantiateNetworkPrint();
            }
        }

        void InstantiateNetworkPrint()
        {
            GameObject go = PhotonNetwork.Instantiate("NetworkPrint", Vector3.zero, Quaternion.identity, 0) as GameObject;

        }

        public void AddNewPlayer(NetworkPrint print)
        {
            PlayerHolder playerH = mRef.AddNewPlayer(print);
            if (print.isLocal)
            {
                mRef.localPlayer = playerH;
            }
        }
    }
}
