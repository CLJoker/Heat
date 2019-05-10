using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using SA;

public class SaveProfile : MonoBehaviour {
    public InputField playerName;
    public Image image;

    private void Start()
    {
        playerName.text = DataManager.LoadLocalPlayer().playerName;
    }

    public void SavePlayerProfile()
    {
        GameManagers.GetPlayerProfile().playerName = playerName.text;

        PlayerDataBin player = DataManager.LoadLocalPlayer();
        player.playerName = playerName.text;
        DataManager.SavePlayer(player);
    }
}
