using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SA
{
    [CreateAssetMenu(menuName = "Maps")]
    public class Map : ScriptableObject
    {
        public string mapName;
        public Sprite mapImage;
        [TextArea(3, 6)]
        public string description;
        public int currentUser = 0;
        public int currentProgress = 0;
        
    }
}
