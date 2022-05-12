using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;


/// <summary>
/// A static class for loading and saving the game's latest parameters
/// </summary>
public static class DataManager
{
/// <summary>
/// Saves the game's parameters which are listed in the GameData class
/// </summary>
/// <param name="gameManager">The Game Manager class which overlooks the whole game</param>
    public static void SaveOptions(GameManager gameManager)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/Goal_data4";
        FileStream stream = new FileStream(path, FileMode.Create);

        AppData data = new AppData();

        formatter.Serialize(stream, data);
        stream.Close();
    }

/// <summary>
/// Retrieves the relevant saved paremeters in order to initialize GameManager last status
/// </summary>
/// <returns name="gameManager">The GameData class with the saved parameters from last game session></returns>
    public static AppData LoadOptions()
    {
       // return null;
        string path = Application.persistentDataPath + "/Goal_data4";
        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);
            AppData data = null;
            if (stream.Length > 0)
            {
               
                data = formatter.Deserialize(stream) as AppData;
            }
            else
            {
                Debug.Log("STREAM LENGTH IS 0");
            }
            

            stream.Close();

            return data;

        }
        else
        {
            Debug.LogError("Save file not found in " + path);
            return null;
        }
    }
}
