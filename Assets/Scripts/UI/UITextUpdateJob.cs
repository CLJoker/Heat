using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SA
{
    public class UITextUpdateJob : MonoBehaviour
    {
        public Text modeType;
        public Text levelName;
        public Text currentUsers;
        public Text maxUsers;
        public Job targetJob;

        private void Start()
        {
            LoadJob(targetJob);
        }

        public void LoadJob(Job j)
        {
            targetJob = j;
            modeType.text = StaticFunctions.JobTypeToString(j.type);
            levelName.text = j.targetLevel;
            
        }

        public void UpdateJobOnGameSettings()
        {
            GameSettings gs = Resources.Load("GameSettings") as GameSettings;
            gs.UpdateCurrentJob(targetJob);
        }
    }
}
