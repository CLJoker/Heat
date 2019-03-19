using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
    public class MultiplayerManager : Photon.MonoBehaviour
    {
        MultiplayerReferences mRef;
        public MultiplayerReferences GetMRef()
        {
            return mRef;
        }

        public static MultiplayerManager singleton;

        public RayBallistics ballistics;
        List<PlayerHolder> playersToSpawn = new List<PlayerHolder>();
        private void Update()
        {
            float delta = Time.deltaTime;
            for(int i = playersToSpawn.Count - 1; i >= 0; i--)
            {
                playersToSpawn[i].spawnTimer = delta;
                if(playersToSpawn[i].spawnTimer > 5)
                {
                    playersToSpawn[i].spawnTimer = 0;
                    int ran = Random.Range(0, mRef.spawnPositions.Length);
                    Vector3 pos = mRef.spawnPositions[ran].transform.position;
                    Quaternion rot = mRef.spawnPositions[ran].transform.rotation;

                    photonView.RPC("RPC_SpawnPlayer", PhotonTargets.All, playersToSpawn[i].photonId, pos, rot);
                    playersToSpawn.RemoveAt(i);
                }
            }
        }

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
            PlayerProfile profile = GameManagers.GetPlayerProfile();
            object[] data = new object[1];
            data[0] = profile.itemIds[0];

            GameObject go = PhotonNetwork.Instantiate("NetworkPrint", Vector3.zero, Quaternion.identity, 0, data) as GameObject;

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
        public void FindSpawnPositionOnLevel()
        {
            SpawnPosition[] spawnPositions = GameObject.FindObjectsOfType<SpawnPosition>();
            mRef.spawnPositions = spawnPositions;
        }

        public void BroadCastSceneChange()
        {
            if (PhotonNetwork.isMasterClient)
            {
                photonView.RPC("RPC_SceneChange", PhotonTargets.All);
            }
        }

        public void LevelLoadedCallback()
        {
            //mRef.localPlayer.print.InstantiateController(mRef.localPlayer.spawnPosition);

            if (PhotonNetwork.isMasterClient)
            {
                FindSpawnPositionOnLevel();
                AssignSpawnPositions();
            }
        }

        void AssignSpawnPositions()
        {
            List<PlayerHolder> players = mRef.getPlayers();

            for (int i = 0; i < players.Count; i++)
            {
                int index = i % mRef.spawnPositions.Length;
                SpawnPosition p = mRef.spawnPositions[index];
                photonView.RPC("RPC_BroadcastCreateController", PhotonTargets.All, players[i].photonId
                    , p.transform.position, p.transform.rotation);
            }
        }

        public MultiplayerReferences GetMultiplayerReferences()
        {
            return mRef;
        }

        public void BroadcastShootWeapon(StateManager states, Vector3 direction, Vector3 origin)
        {
            int photonId = states.photonId;
            photonView.RPC("RPC_ShootWeapon", PhotonTargets.All, photonId, direction, origin);
        }

        public void BroadcastKillPlayer(int photonId, int shooter)
        {
            photonView.RPC("RPC_ReceiveKillPlayer", PhotonTargets.MasterClient, photonId, shooter);
        }
        #endregion

        #region RPCs
        [PunRPC]
        public void RPC_BroadcastCreateController(int photonId, Vector3 pos, Quaternion rot)
        {
            if (photonId == mRef.localPlayer.photonId)
            {
                mRef.localPlayer.print.InstantiateController(pos, rot);
            }


        }

        [PunRPC]
        public void RPC_SceneChange()
        {
            MultiplayerLauncher.singleton.LoadCurrentSceneActual(LevelLoadedCallback);
        }

        [PunRPC]
        public void RPC_SetSpawnPositionForPlayer(int photonId, int spawnPosition)
        {
            if(photonId == mRef.localPlayer.photonId)
            {
                mRef.localPlayer.spawnPosition = spawnPosition;
            }
        }

        [PunRPC]
        public void RPC_ShootWeapon(int photonId, Vector3 dir, Vector3 origin)
        {
            if(photonId == mRef.localPlayer.photonId)
            {
                return;
            }
            PlayerHolder shooter = mRef.GetPlayer(photonId);
            if (shooter == null)
                return;


            ballistics.ClientShoot(shooter.states, dir, origin);
        }

        [PunRPC]
        public void RPC_SpawnPlayer(int photonId, Vector3 targetPosition, Quaternion targetRotation)
        {
            PlayerHolder playerHolder = mRef.GetPlayer(photonId);

            if (playerHolder.states != null)
                playerHolder.states.SpawnPlayer(targetPosition, targetRotation);

        }

        [PunRPC]
        public void RPC_ReceiveKillPlayer(int photonId, int shooter)
        {
            // Master Client
            photonView.RPC("RPC_KillPlayer", PhotonTargets.All, photonId, shooter);
            playersToSpawn.Add(mRef.GetPlayer(photonId));
        }

        [PunRPC]
        public void RPC_KillPlayer(int photonId, int shooter)
        {
            PlayerHolder playerHolder = mRef.GetPlayer(photonId);

            if (playerHolder.states != null)
                playerHolder.states.KillPlayer();
        }
        #endregion
    }
}
