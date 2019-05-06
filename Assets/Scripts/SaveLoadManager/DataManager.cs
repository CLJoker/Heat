using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public static class DataManager {

    /// <summary>
    /// Save the data of player to local
    /// </summary>
    /// <param name="player">Data of player</param>
    public static void SavePlayer(PlayerDataBin player)
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream stream = new FileStream(Application.persistentDataPath + "/player.save", FileMode.Create);
        PlayerDataBin data = new PlayerDataBin(player);
        bf.Serialize(stream, data);
        stream.Close();
    }

    /// <summary>
    /// Load the local data of player from save file
    /// </summary>
    /// <returns>The save file of player, if exist</returns>
    public static PlayerDataBin LoadLocalPlayer()
    {
        PlayerDataBin player = null;
        if(File.Exists(Application.persistentDataPath + "/player.save"))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream stream = new FileStream(Application.persistentDataPath + "/player.save", FileMode.Open);

            player = bf.Deserialize(stream) as PlayerDataBin;
            stream.Close();
        }
        return player;
    }

}
