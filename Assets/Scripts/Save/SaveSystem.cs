using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public class SaveSystem 
{

    string path = Application.persistentDataPath + "/LUTD.data";

    public void Save()
    {
        BinaryFormatter binaryFormatter = new BinaryFormatter();
        
        FileStream fileStream = new FileStream(path, FileMode.Create);

        PlayerData data = new PlayerData(App.gameManager.GetSceneIndex(),
                                         App.player.GetCoins(), 
                                         App.player.GetVaccines());

        binaryFormatter.Serialize(fileStream, data);
        fileStream.Close();
    }

    public PlayerData Load()
    {
        if (File.Exists(path))
        {
            BinaryFormatter binaryFormatter = new BinaryFormatter();
            FileStream fileStream = new FileStream(path, FileMode.Open);

            PlayerData data = binaryFormatter.Deserialize(fileStream) as PlayerData;

            return data;
        }

        return null;
    }
}
