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
            DontDestroyOnLoad(this.gameObject);
            mRef = new MultiplayerReferences();
            DontDestroyOnLoad(mRef.referencesParent.gameObject);
            InstantiateNetworkPrint();
        }

        void InstantiateNetworkPrint()
        {
            GameObject go = PhotonNetwork.Instantiate("NetworkPrint", Vector3.zero, Quaternion.identity, 0) as GameObject;

        }

        public void AddNewPlayer(NetworkPrint print)
        {
            print.transform.parent = mRef.referencesParent;
            PlayerHolder playerH = mRef.AddNewPlayer(print);
            if (print.isLocal)
            {
                mRef.localPlayer = playerH;
            }
        }

        #region MyCalls
        public void BroadCastSceneChange()
        {
            if (PhotonNetwork.isMasterClient)
            {
                photonView.RPC("RPC_SceneChange", PhotonTargets.All);
            }
        }

        public void CreateController()
        {
            mRef.localPlayer.print.InstantiateController(mRef.localPlayer.spawnPosition);
        }

        public MultiplayerReferences GetMultiplayerReferences()
        {
            return mRef;
        }
        #endregion

        #region RPCs
        [PunRPC]
        public void RPC_SceneChange()
        {
            MultiplayerLauncher.singleton.LoadCurrentSceneActual(CreateController);
        }

        [PunRPC]
        public void RPC_SetSpawnPositionForPlayer(int photonId, int spawnPosition)
        {
            if(photonId == mRef.localPlayer.photonId)
            {
                mRef.localPlayer.spawnPosition = spawnPosition;
            }
        }
        #endregion
    }
}
