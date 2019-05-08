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
        int winKillCount = 5;
        [SerializeField]
        float startingTime = 300;
        float currentTime;
        float timerInterval;
        [SerializeField]
        IntVariable timerInSeconds;
        [SerializeField]
        GameEvent timerUpdate;
        bool isMaster;
        [SerializeField]
        IntVariable firstTeamKillCount;
        [SerializeField]
        IntVariable secondTeamKillCount;
        [SerializeField]
        GameEvent updateTeamKillCount;

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
                    
                    Vector3 pos;
                    Quaternion rot;
                    if (playersToSpawn[i].team == 1)
                    {
                        int ran = Random.Range(0, mRef.firstTeamSpawnPositions.Count - 1);
                        pos = mRef.firstTeamSpawnPositions[ran].transform.position;
                        rot = mRef.firstTeamSpawnPositions[ran].transform.rotation;
                    }
                    else
                    {
                        int ran = Random.Range(0, mRef.secondTeamSpawnPositions.Count - 1);
                        pos = mRef.secondTeamSpawnPositions[ran].transform.position;
                        rot = mRef.secondTeamSpawnPositions[ran].transform.rotation;
                    }

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
            firstTeamKillCount.value = 0;
            secondTeamKillCount.value = 0;
        }

        void OnMasterClientSwitched(PhotonPlayer newMasterClient)
        {
            isMaster = PhotonNetwork.isMasterClient;
        }

        void InstantiateNetworkPrint()
        {
            PlayerProfile profile = GameManagers.GetPlayerProfile();
            object[] data = new object[2];
            data[0] = profile.itemIds[0];
            data[1] = profile.modelId;

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
            foreach(SpawnPosition spawn in spawnPositions)
            {
                if(spawn.teamSpawn == 1)
                {
                    mRef.firstTeamSpawnPositions.Add(spawn);
                }
                else
                {
                    mRef.secondTeamSpawnPositions.Add(spawn);
                }
            }
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
            inGame = true;
        }

        void AssignSpawnPositions()
        {
            List<PlayerHolder> firstTeamPlayers = mRef.getTeamOne();
            List<PlayerHolder> secondTeamPlayers = mRef.getTeamTwo();

            for (int i = 0; i < firstTeamPlayers.Count; i++)
            {
                int index = i % mRef.firstTeamSpawnPositions.Count;
                SpawnPosition p = mRef.firstTeamSpawnPositions[index];
                photonView.RPC("RPC_BroadcastCreateController", PhotonTargets.All, firstTeamPlayers[i].photonId
                    , p.transform.position + Vector3.up * 0.1f, p.transform.rotation);
            }

            for (int i = 0; i < secondTeamPlayers.Count; i++)
            {
                int index = i % mRef.secondTeamSpawnPositions.Count;
                SpawnPosition p = mRef.secondTeamSpawnPositions[index];
                photonView.RPC("RPC_BroadcastCreateController", PhotonTargets.All, secondTeamPlayers[i].photonId
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
            Debug.Log("Player team: " + p.team);
            if(p.team == 1)
            {
                firstTeamKillCount.value += 1;
                photonView.RPC("RPC_SyncTeamKillCount", PhotonTargets.All, 1, firstTeamKillCount.value);
            }
            if(p.team == 2)
            {
                secondTeamKillCount.value += 1;
                photonView.RPC("RPC_SyncTeamKillCount", PhotonTargets.All, 2, secondTeamKillCount.value);
            }
            photonView.RPC("RPC_SyncKillCount", PhotonTargets.All, shooterId, p.killCount);

            Debug.Log("first team: " + firstTeamKillCount.value);
            Debug.Log("Second team: " + secondTeamKillCount.value);

            if(firstTeamKillCount.value > winKillCount)
            {
                BroadcastMatchOver(1);
            }
            else if(secondTeamKillCount.value > winKillCount)
            {
                BroadcastMatchOver(2);
            }
        }

        public void BroadcastMatchOver(int winnerTeam)
        {
            photonView.RPC("RPC_BroadcastMatchOver", PhotonTargets.All, winnerTeam);
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
            int winnerTeam = -1;

            if(firstTeamKillCount.value > secondTeamKillCount.value)
            {
                winnerTeam = 1;
            }
            else if(firstTeamKillCount.value < secondTeamKillCount.value)
            {
                winnerTeam = 2;
            }

            BroadcastMatchOver(winnerTeam);
        }
        #endregion

        #region RPCs
        [PunRPC]
        public void RPC_BroadcastTime(float masterTime)
        {
            if (!isMaster)
            {
                currentTime = masterTime;
                timerInSeconds.value = Mathf.RoundToInt(currentTime);
                timerUpdate.Raise();
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
        public void RPC_BroadcastMatchOver(int winnerTeam)
        {
            bool isWinner = false;
            Debug.Log("local player's team:" + mRef.localPlayer.team);
            Debug.Log("winner team" + winnerTeam);
            if (mRef.localPlayer.team == winnerTeam)
            {
                isWinner = true;
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

        [PunRPC]
        public void RPC_SyncTeamKillCount(int team, int teamKill)
        {
            if(team == 1)
            {
                firstTeamKillCount.value = teamKill;
            }
            if(team == 2)
            {
                secondTeamKillCount.value = teamKill;
            }
            updateTeamKillCount.Raise();
        }
        #endregion
    }
}
