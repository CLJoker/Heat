using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;
using SO;

namespace SA
{
    public class MultiplayerLauncher : Photon.PunBehaviour
    {
        public delegate void OnSceneLoaded();
        bool isLoading;
        bool isInGame;

        public int gameVersion = 1;
        public PhotonLogLevel logLevel = PhotonLogLevel.ErrorsOnly;

        public static MultiplayerLauncher singleton;
        public GameEvent onConnectedToMaster;
        public GameEvent onJoinedRoom;
        public BoolVariable isConnected;
        public BoolVariable isMultiplayer;

        #region Init
        private void Awake()
        {
            if(singleton == null)
            {
                singleton = this;
                DontDestroyOnLoad(this.gameObject);
            }
            else
            {
                Destroy(this.gameObject);

            }
        }

        private void Start()
        {
            isConnected.value = PhotonNetwork.connected;
            ConnectToServer();
        }

        public void ConnectToServer()
        {
            PhotonNetwork.logLevel = logLevel;
            PhotonNetwork.autoJoinLobby = false;
            PhotonNetwork.automaticallySyncScene = false;
            PhotonNetwork.ConnectUsingSettings(this.gameVersion.ToString());
        }
        #endregion

        #region Photon Callback
        public override void OnConnectedToMaster()
        {
            isConnected.value = true;
            onConnectedToMaster.Raise();
        }

        //This maybe will be change after change Unity version
        public override void OnFailedToConnectToPhoton(DisconnectCause cause)
        {
            base.OnFailedToConnectToPhoton(cause);
            isConnected.value = false;
        }

        public override void OnCreatedRoom()
        {
            Room r = ScriptableObject.CreateInstance<Room>();

            object sceneName;
            PhotonNetwork.room.CustomProperties.TryGetValue("scene", out sceneName);
            r.sceneName = (string)sceneName;
            r.roomName = PhotonNetwork.room.Name;

            GameManagers.GetResourcesManager().currentRoom.value = r;
        }

        public override void OnJoinedRoom()
        {
            onJoinedRoom.Raise();
            InstantiateMultiplayerManager();
        }

        public override void OnJoinedLobby()
        {
            Debug.Log("OnJoinedLobby");
            StartCoroutine(RoomCheck());
        }

        IEnumerator RoomCheck()
        {
            yield return new WaitForSeconds(3);
            if (!isInGame)
            {
                MatchMakingManager m = MatchMakingManager.singleton;
                RoomInfo[] rooms = PhotonNetwork.GetRoomList();

                Debug.Log(rooms.Length);
                m.AddMatches(rooms);

                yield return new WaitForSeconds(5);
                StartCoroutine(RoomCheck());
            }
        }
        #endregion

        #region Manager Methods
        public void JoinLobby()
        {
            PhotonNetwork.JoinLobby();
        }

        void InstantiateMultiplayerManager()
        {
            if (PhotonNetwork.isMasterClient)
            {
                PhotonNetwork.Instantiate("MultiplayerManager", Vector3.zero, Quaternion.identity, 0);
            }
        }

        public void CreateRoom(RoomButton b)
        {
            if (isMultiplayer.value)
            {
                if (!isConnected.value)
                {
                    //Handle error, when cant connect but still on multiplayer
                }
                else
                {
                    RoomOptions roomOptions = new RoomOptions();
                    roomOptions.MaxPlayers = 4;

                    ExitGames.Client.Photon.Hashtable properties = new ExitGames.Client.Photon.Hashtable
                    {
                        {"scene", b.scene }
                    };

                    roomOptions.CustomRoomPropertiesForLobby = new string[] { "scene" };
                    roomOptions.CustomRoomProperties = properties;
                    PhotonNetwork.CreateRoom(null, roomOptions, TypedLobby.Default);
                }
            }
            else
            {
                Room r = ScriptableObject.CreateInstance<Room>();
                r.sceneName = b.scene;
                GameManagers.GetResourcesManager().currentRoom.Set(r);
            }

            isInGame = true;
        }

        public void JoinRoom(RoomInfo roomInfo)
        {
            PhotonNetwork.JoinRoom(roomInfo.Name);
            isInGame = true;
        }

        public void LoadMainMenu()
        {
            StartCoroutine(LoadScene("Main", OnMainMenu));
        }

        /// <summary>
        /// Gets called by an event
        /// </summary>
        public void LoadCurrentRoom()
        {
            if (isConnected)
            {
                MultiplayerManager.singleton.BroadCastSceneChange();
            }
            else
            {
                Room r = GameManagers.GetResourcesManager().currentRoom.value;
                if (!isLoading)
                {
                    isLoading = true;
                    StartCoroutine(LoadScene(r.sceneName));
                }
            }

        }

        public void LoadCurrentSceneActual(OnSceneLoaded callback = null)
        {
            Room r = GameManagers.GetResourcesManager().currentRoom.value;
            if (!isLoading)
            {
                isLoading = true;
                StartCoroutine(LoadScene(r.sceneName, callback));
            }
        }

        IEnumerator LoadScene(string targetLevel, OnSceneLoaded callback = null)
        {
            yield return SceneManager.LoadSceneAsync(targetLevel, LoadSceneMode.Single);
            isLoading = false;
            if(callback != null)
            {
                callback.Invoke();
            }
        }
        #endregion

        #region Setup Methods
        public void OnMainMenu()
        {
            isConnected.value = PhotonNetwork.connected;
            if (isConnected.value)
            {
                OnConnectedToMaster();
            }
            else
            {
                ConnectToServer();
            }
        }

        #endregion
    }
}
