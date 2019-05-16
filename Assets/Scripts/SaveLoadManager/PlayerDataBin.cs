using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerDataBin {

    public string playerName = "player";
    public string modelId;
    public string mainWeapon;
    public string subWeapon;
    public byte[] avatar;

    public PlayerDataBin()
    {

    }

    public PlayerDataBin(PlayerDataBin player)
    {
        playerName = player.playerName;
        modelId = player.modelId;
        mainWeapon = player.mainWeapon;
        subWeapon = player.subWeapon;
        avatar = player.avatar;
    }
}
