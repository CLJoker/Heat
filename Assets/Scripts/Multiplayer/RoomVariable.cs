﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
    [CreateAssetMenu(menuName ="Variables/Room Variable")]
    public class RoomVariable : ScriptableObject
    {
        public Room value;
        public RoomButtonVariable roomButtonVariable;

        public void JoinGame()
        {
            if (roomButtonVariable == null)
                return;

            SetRoom(roomButtonVariable.value);
        }

        public void Set(Room r)
        {
            value = r;
        }

        public void SetRoom(RoomButton b)
        {
            if (b.isRoomCreated)
            {
                Set(b.room);
                MultiplayerLauncher.singleton.JoinRoom(b.roomInfo);
            }
            else
            {
                Debug.Log(b.name);
                MultiplayerLauncher.singleton.CreateRoom(b);
            }
        }

        public void SetRoomButton(RoomButton b)
        {
            roomButtonVariable.value = b;
        }
    }
}
