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
        delegate void OnSceneLoaded();
        bool isLoading;
        public int gameVersion = 1;
        public PhotonLogLevel logLevel = PhotonLogLevel.ErrorsOnly;

        public static MultiplayerLauncher singleton;
        public GameEvent onConnectedToMaster;
        public BoolVariable isConnected;

        #region Init
        private void Awake()
        {
            if(singleton == null)
            {
                singleton = this;
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

        #endregion

        #region Manager Methods
        public void LoadMainMenu()
        {
            StartCoroutine(LoadScene("Main", OnMainMenu));
        }

        public void LoadCurrentRoom()
        {
            Room r = GameManagers.GetResourcesManager().currentRoom.value;
            if (!isLoading)
            {
                isLoading = true;
                StartCoroutine(LoadScene(r.sceneName));
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
