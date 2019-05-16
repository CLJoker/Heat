using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using SA;

public class SaveProfile : MonoBehaviour {
    public InputField playerName;
    public RawImage image;
    public Texture2D temp;

    private void Start()
    {
        playerName.text = DataManager.LoadLocalPlayer().playerName;
        byte[] imageData = DataManager.LoadLocalPlayer().avatar;
        if(imageData != null)
        {
            Texture2D texture = temp;
            texture.LoadImage(imageData);
            texture.Apply();
            image.texture = texture;
        }       
    }

    public void SavePlayerProfile()
    {
        Texture2D texture = image.texture as Texture2D;
        byte[] imageData = texture.EncodeToJPG();

        //Save to PlayerProfile - Online
        GameManagers.GetPlayerProfile().playerName = playerName.text;
        GameManagers.GetPlayerProfile().avatarData = imageData;
        //Save to local data

        PlayerDataBin player = DataManager.LoadLocalPlayer();
        player.playerName = playerName.text;
        player.avatar = imageData;
        DataManager.SavePlayer(player);
        Debug.Log("Save successful");
    }
}
