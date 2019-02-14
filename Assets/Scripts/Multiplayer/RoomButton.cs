using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SA
{
    public class RoomButton : MonoBehaviour
    {
        public bool isRoomCreated;
        public Room room;
        public string scene = "City";
        public Map mapInformation;

        public void Start()
        {
            AutoAssignMapInformation();
        }

        public void OnClick()
        {
            GameManagers.GetResourcesManager().currentRoom.SetRoomButton(this);
            GameManagers.GetResourcesManager().currentMap.SetMap(mapInformation);
        }

        #region Auto Assign Map Information
        [System.Serializable]
        public class MapButton
        {
            public Text name;
            public Text currentPlayer;
            public Text currentProgress;
        }
        public MapButton mapButton;

        public void AutoAssignMapInformation()
        {
            if (mapInformation == null)
                return;

            mapButton.name.text = mapInformation.mapName;
            mapButton.currentProgress.text = mapInformation.currentProgress.ToString() + "%";
            mapButton.currentPlayer.text = mapInformation.currentUser.ToString();
        }

        #endregion
    }
}
