using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace SA
{
    public class SessionManager : MonoBehaviour
    {
        public GameSettings gameSettings;

        public GameEvent onGameStart;
        public GameEvent onSceneLoadedAdditive;
        public GameEvent onSceneLoadedSingle;

        public void Awake()
        {
            gameSettings.r_manager.Init();
        }

        public void Start()
        {
            if (onGameStart != null)
                onGameStart.Raise();
        }

        public void LoadSceneAsyncAdditive(string lvl)
        {
            StartCoroutine(LoadSceneAsyncAdditive_Actual(lvl));
        }

        public void LoadSceneAsyncSingle(string lvl)
        {
            StartCoroutine(LoadSceneAsyncSingle_Actual(lvl));
        }

        public void LoadLevelFromGameSettings()
        {
            LoadSceneAsyncSingle(gameSettings.uiSettings.curJob.targetLevel);
        }

        IEnumerator LoadSceneAsyncAdditive_Actual(string lvl)
        {
            yield return SceneManager.LoadSceneAsync(lvl, LoadSceneMode.Additive);
            if (onSceneLoadedAdditive != null)
            {
                onSceneLoadedAdditive.Raise();
                onSceneLoadedAdditive = null;
            }
        }

        IEnumerator LoadSceneAsyncSingle_Actual(string lvl)
        {
            yield return SceneManager.LoadSceneAsync(lvl, LoadSceneMode.Single);
            if (onSceneLoadedSingle != null)
            {
                onSceneLoadedSingle.Raise();
                onSceneLoadedSingle = null;
            }
        }
    }
}
