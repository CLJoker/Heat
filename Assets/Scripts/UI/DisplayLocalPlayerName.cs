using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using SA;

public class DisplayLocalPlayerName : MonoBehaviour {
    public Text playerNameText;

	// Use this for initialization
	void Start () {
        playerNameText.text = MultiplayerManager.singleton.GetMRef().localPlayer.userName;
	}
	
}
