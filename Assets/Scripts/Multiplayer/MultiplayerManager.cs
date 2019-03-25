using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SO;

namespace SA
{
    public class MultiplayerManager : Photon.MonoBehaviour
    {
        MultiplayerReferences mRef;
        bool inGame;
        bool endMatch;
        [SerializeField]
        int winKillCount = 2;
        [SerializeField]
        float startingTime = 300;
        float currentTime;
        float timerInterval;
        [SerializeField]
        IntVariable timerInSeconds;
        [SerializeField]
        GameEvent timerUpdate;
        bool isMaster;

        public MultiplayerReferences GetMRef()
        {
            return mRef;
        }

        public static MultiplayerManager singleton;

        public RayBallistics ballistics;
        List<PlayerHolder> playersToSpawn = new List<PlayerHolder>();
        private void Update()
        {
            if (endMatch)
                return;

            float delta = Time.deltaTime;
            if (inGame)
            {
                currentTime -= delta;
                timerInterval += delta;
                if(timerInterval > 1)
                {
                    timerInterval = 0;
                    timerInSeconds.value = Mathf.RoundToInt(currentTime);
                    timerUpdate.Raise();

                    if (isMaster)
                    {
                        photonView.RPC("RPC_BroadcastTime", PhotonTargets.All, currentTime);
                    }
                }

                if (currentTime <= 0)
                {
                    if (isMaster)
                    {
                        TimerRunOut();
                    }                       
                }
            }

            if (!isMaster)
                return;

            for (int i = playersToSpawn.Count - 1; i >= 0; i--)
            {
                playersToSpawn[i].spawnTimer += delta;
                if (playersToSpawn[i].spawnTimer > 5)
                {
                    playersToSpawn[i].spawnTimer = 0;
                    playersToSpawn[i].health = 100;
                    int ran = Random.Range(0, mRef.spawnPositions.Length);
                    Vector3 pos = mRef.spawnPositions[ran].transform.position;
                    Quaternion rot = mRef.spawnPositions[ran].transform.rotation;

                    photonView.RPC("RPC_BroadcastPlayerHealth", PhotonTargets.All, playersToSpawn[i].photonId, 100);
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
            currentTime = startingTime;
            isMaster = PhotonNetwork.isMasterClient;
        }

        void OnMasterClientSwitched(PhotonPlayer newMasterClient)
        {
            isMaster = PhotonNetwork.isMasterClient;
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
                inGame = true;
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
                    , p.transform.position + Vector3.up * 0.1f, p.transform.rotation);
            }
        }

        public MultiplayerReferences GetMultiplayerReferences()
        {
            return mRef;
        }

        public void BroadcastPlayerHealth(int photonId, int health, int shooter)
        {
            if(health <= 0)
            {
                BroadcastPlayerIsHitBy(photonId, shooter);
                playersToSpawn.Add(mRef.GetPlayer(photonId));
            }

            photonView.RPC("RPC_BroadcastPlayerHealth", PhotonTargets.All, photonId, health);
        }

        public void BroadcastShootWeapon(StateManager states, Vector3 direction, Vector3 origin)
        {
            int photonId = states.photonId;
            photonView.RPC("RPC_ShootWeapon", PhotonTargets.All, photonId, direction, origin);
        }

        public void BroadcastPlayerIsHitBy(int photonId, int shooterId)
        {
            PlayerHolder p = mRef.GetPlayer(shooterId);
            p.killCount++;
            photonView.RPC("RPC_SyncKillCount", PhotonTargets.All, shooterId, p.killCount);

            if(p.killCount > winKillCount)
            {
                BroadcastMatchOver(shooterId);
            }
        }

        public void BroadcastMatchOver(int photonId)
        {
            photonView.RPC("RPC_BroadcastMatchOver", PhotonTargets.All, photonId);
            endMatch = true;
        }

        public void BroadcastKillPlayer(int photonId)
        {
            photonView.RPC("RPC_KillPlayer", PhotonTargets.All, photonId);
        }

        public void ClearReferences()
        {
            if(mRef.referencesParent != null)
            {
                Destroy(mRef.referencesParent.gameObject);
                Destroy(this.gameObject);
            }
        }

        public void TimerRunOut()
        {
            List<PlayerHolder> players = mRef.getPlayers();
            int killCount = 0;
            int winnerId = -1;

            for(int i = 0; i<players.Count; i++)
            {
                if(players[i].killCount > killCount)
                {
                    killCount = players[i].killCount;
                    winnerId = players[i].photonId;
                }
            }

            BroadcastMatchOver(winnerId);
        }
        #endregion

        #region RPCs
        [PunRPC]
        public void RPC_BroadcastTime(float masterTime)
        {
            if (!isMaster)
            {
                currentTime = masterTime;
            }
        }

        [PunRPC]
        public void RPC_BroadcastCreateController(int photonId, Vector3 pos, Quaternion rot)
        {
            if (photonId == mRef.localPlayer.photonId)
            {
                mRef.localPlayer.print.InstantiateController(pos, rot);
            }

        }

        [PunRPC]
        public void RPC_BroadcastMatchOver(int photonId)
        {
            bool isWinner = false;
            if(mRef.localPlayer.photonId == photonId)
            {
                isWinner = true;
                Debug.Log("winner");
            }
            MultiplayerLauncher.singleton.EndMatch(this, isWinner);
        }

        [PunRPC]
        public void RPC_SceneChange()
        {
            MultiplayerLauncher.singleton.LoadCurrentSceneActual(LevelLoadedCallback);
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
        public void RPC_KillPlayer(int photonId)
        {
            PlayerHolder playerHolder = mRef.GetPlayer(photonId);

            if (playerHolder.states != null)
                playerHolder.states.KillPlayer();
        }

        [PunRPC]
        public void RPC_SyncKillCount(int photonId, int killCount)
        {
            if(photonId == mRef.localPlayer.photonId)
            {
                mRef.localPlayer.killCount = killCount;
            }
        }

        [PunRPC]
        public void RPC_BroadcastPlayerHealth(int photonId, int health)
        {
            PlayerHolder player = mRef.GetPlayer(photonId);
            player.health = health;
            player.states.stats.health = health;
            player.states.healthChangedFlag = true;

            if(player == mRef.localPlayer)
            {
                if(player.health <= 0)
                {
                    BroadcastKillPlayer(photonId);
                    return;
                }               
            }
            player.states.anim.Play("damage2");
        }
        #endregion
    }
}
