using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
    public class MultiplayerReferences : MonoBehaviour
    {
        public SpawnPosition[] spawnPositions;
        List<PlayerHolder> players = new List<PlayerHolder>();

        public PlayerHolder localPlayer;
        public Transform referencesParent;

        public MultiplayerReferences()
        {
            referencesParent = new GameObject().transform;
            referencesParent.name = "references";
        }

        public int getPlayerCount()
        {
            return players.Count;
        }

        public List<PlayerHolder> getPlayers()
        {
            return players;
        }

        public PlayerHolder AddNewPlayer(NetworkPrint print)
        {
            if (!IsUniquePlayer(print.photonId))
                return null;

            PlayerHolder playerHolder = new PlayerHolder();
            playerHolder.photonId = print.photonId;
            playerHolder.print = print;
            players.Add(playerHolder);
            return playerHolder;
        }

        public PlayerHolder GetPlayer(int photonId)
        {
            for(int i = 0; i < players.Count; i++)
            {
                if (players[i].photonId == photonId)
                    return players[i];
            }

            return null;
        }

        public bool IsUniquePlayer(int id)
        {
            for(int i = 0; i < players.Count; i++)
            {
                if (players[i].photonId == id)
                    return false;
            }
            return true;
        }

    }
}
