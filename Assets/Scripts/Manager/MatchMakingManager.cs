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

        public static MatchMakingManager singleton;

        public Transform matchParent;
        public GameObject matchPrefab;

        private void Awake()
        {
            singleton = this;
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

        public void AddMatch()
        {
            GameObject go = Instantiate(matchPrefab);
            go.transform.SetParent(matchParent);

            MatchSpawnPosition p = GetSpawnPos();
            p.isUsed = true;
            go.transform.position = p.pos.position;
            go.transform.localScale = Vector3.one;
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
