using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
    [CreateAssetMenu(menuName ="Single Instances/Game Settings")]
    public class GameSettings : ScriptableObject
    {
        public ResourcesManager r_manager;
        public int version = 0;
        public string userName;
        public bool isConnected;
        public UISettings uiSettings;
        [System.Serializable]
        public class UISettings
        {
            public Job curJob;
            public GameEvent onJobChange;
            public StringVariable jobType;
            public StringVariable jobDescription;
            public SpriteVariable jobImg;
            public StringVariable mapName;
        }

        public void UpdateCurrentJob(Job targetJob)
        {
            uiSettings.curJob = targetJob;
            uiSettings.jobDescription.value = targetJob.jobDescription;
            uiSettings.jobType.value = StaticFunctions.JobTypeToString(targetJob.type);
            uiSettings.jobImg.sprite = targetJob.jobImg;
            uiSettings.mapName.value = targetJob.targetLevel;
            if (uiSettings.onJobChange != null)
                uiSettings.onJobChange.Raise();
        }
    }
}
