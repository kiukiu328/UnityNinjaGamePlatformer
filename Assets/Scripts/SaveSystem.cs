using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
// class for turn savedata class to save.dat file
public static class SaveSystem
{
    static string path = Application.persistentDataPath + "/save.dat";


    public static void Save(SaveData saveData)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        FileStream stream = new FileStream(path, FileMode.Create);
        formatter.Serialize(stream, saveData);
        stream.Close();
    }

    public static SaveData Load()
    {
        if (!File.Exists(path))
        {
            Debug.Log("Dont have saved data");
            return new SaveData();
        }
        BinaryFormatter formatter = new BinaryFormatter();
        FileStream stream = new FileStream(path, FileMode.Open);
        SaveData data = formatter.Deserialize(stream) as SaveData;
        stream.Close();
        return data;
    }

    public static void ClearData()
    {
        Save(new SaveData());
    }
}
