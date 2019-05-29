using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
    public class MatchMakingManager : MonoBehaviour
    {
        public Transform spawnParent;

        Transform[] spawnPosition;
        List<MatchSpawnPosition> spawnPos = new List<MatchSpawnPosition>();

        Dictionary<string, RoomButton> roomsDict = new Dictionary<string, RoomButton>();

        public static MatchMakingManager singleton;

        public Transform matchParent;
        public GameObject matchPrefab;

        private void Awake()
        {
            if (singleton == null)
            {
                singleton = this;
            }
            else
            {
                Destroy(this.gameObject);
            }
        }

        // Use this for initialization
        void Start()
        {
            Transform[] p = spawnParent.GetComponentsInChildren<Transform>();
            foreach(Transform t in p)
            {
                if(t != spawnParent)
                {
                    MatchSpawnPosition m = new MatchSpawnPosition();
                    m.pos = t;
                    spawnPos.Add(m);
                }
            }
        }

        RoomButton GetRoomFromDictionary(string id)
        {
            RoomButton result = null;
            roomsDict.TryGetValue(id, out result);

            return result;
        }

        public void AddMatches(RoomInfo[] rooms)
        {
            SetDirtyRoom();

            for(int i = 0; i < rooms.Length; i++)
            {
                RoomInfo r = rooms[i];

                RoomButton createdRoom = GetRoomFromDictionary(rooms[i].Name);
                if(createdRoom == null)
                {
                    AddMatch(r);
                }
                else
                {
                    createdRoom.isValid = true;
                }
            }

            ClearNonValidRoom();
        }

        void SetDirtyRoom()
        {
            List<RoomButton> allRooms = new List<RoomButton>();
            allRooms.AddRange(roomsDict.Values);

            foreach(RoomButton r in allRooms)
            {
                r.isValid = false;
            }
        }

        void ClearNonValidRoom()
        {
            List<RoomButton> allRooms = new List<RoomButton>();
            allRooms.AddRange(roomsDict.Values);

            foreach (RoomButton r in allRooms)
            {
                if (!r.isValid)
                {
                    roomsDict.Remove(r.roomInfo.Name);
                    Destroy(r.gameObject);
                }
            }
        }

        public void AddMatch(RoomInfo roomInfo)
        {
            GameObject go = Instantiate(matchPrefab);
            go.transform.SetParent(matchParent);

            MatchSpawnPosition p = GetSpawnPos();
            p.isUsed = true;
            go.transform.position = p.pos.position;
            go.transform.localScale = Vector3.one;

            RoomButton roomButton = go.GetComponent<RoomButton>();
            roomButton.roomInfo = roomInfo;
            roomButton.isRoomCreated = true;
            roomButton.isValid = true;

            roomButton.room = ScriptableObject.CreateInstance<Room>();
            object sceneObj = null;
            roomInfo.CustomProperties.TryGetValue("scene", out sceneObj);
            string sceneName = (string)sceneObj;

            Map mapInfor = GameManagers.GetResourcesManager().GetMapInstance(sceneName);
            roomButton.mapInformation = mapInfor;
            roomButton.AutoAssignMapInformation();

            roomButton.room.sceneName = sceneName;
            roomButton.room.roomName = roomInfo.Name;

            roomsDict.Add(roomInfo.Name, roomButton);
        }

        public MatchSpawnPosition GetSpawnPos()
        {
            List<MatchSpawnPosition> l = GetUnused();

            int ran = Random.Range(0, l.Count);

            return l[ran];
        }

        public List<MatchSpawnPosition> GetUnused()
        {
            List<MatchSpawnPosition> r = new List<MatchSpawnPosition>();
            for(int i = 0; i < spawnPos.Count; i++)
            {
                if(!spawnPos[i].isUsed)
                {
                    r.Add(spawnPos[i]);
                }
            }

            return r;
        }

        public class MatchSpawnPosition
        {
            public Transform pos;
            public bool isUsed;
        }
    }
}
