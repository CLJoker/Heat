using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
    [CreateAssetMenu(menuName ="Job")]
    public class Job : ScriptableObject
    {
        public string targetLevel;
        public JobType type;
        public int maxPlayer = 10;

        public string jobDescription;
        public Sprite jobImg;
    }

    public enum JobType
    {
        shootout,
        heist,
        deathmatch
    }
}
