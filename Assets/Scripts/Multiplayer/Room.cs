using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
    [CreateAssetMenu(menuName = "Room")]
    public class Room : ScriptableObject
    {
        public string sceneName;
        public string roomName;
    }
}
