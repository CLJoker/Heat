using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using SA;

public class TeamSelect : MonoBehaviour {
    public Text teamDisplayText;
	// Use this for initialization
	public void SelectTeam(int team)
    {
        int Id = MultiplayerManager.singleton.GetMRef().localPlayer.photonId;
        MultiplayerManager.singleton.BroadcastSelectTeam(Id, team);
        teamDisplayText.text = team.ToString();
    }	
}
