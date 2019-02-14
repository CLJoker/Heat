using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
    [CreateAssetMenu(menuName ="Variables/Room Variable")]
    public class RoomVariable : ScriptableObject
    {
        public Room value;

        public void Set(Room r)
        {
            value = r;
        }

        public void SetRoom(RoomButton b)
        {
            if (b.isRoomCreated)
            {
                Set(b.room);
            }
            else
            {
                MultiplayerLauncher.singleton.CreateRoom(b);
            }
        }
    }
}
