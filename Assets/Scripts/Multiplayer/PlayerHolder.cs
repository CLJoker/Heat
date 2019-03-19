using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
    public class PlayerHolder
    {
        public int photonId;
        public string userName;
        public int spawnPosition;
        public int health;
        public int killCount;
        public NetworkPrint print;
        public StateManager states;
        public float spawnTimer;
    }
}
