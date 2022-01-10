using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public static class SaveLoad 
{
    public static void SaveScores(int[] scores)
    {
        if (scores.Length != 9)
        {
            Debug.LogWarning("Trying to save a score that is not 9 numbers long. Weird");
        }

        string path = Application.persistentDataPath + "/Scores/";
        if (!Directory.Exists(path))
        {
            Directory.CreateDirectory(path);
            Debug.Log("Folder created on: " + path);
        }
        path += "hiscores.score";

        BinaryFormatter formatter = new BinaryFormatter();
        FileStream stream = new FileStream(path, FileMode.Create);
        formatter.Serialize(stream, scores);
        stream.Close();
    }

    public static int[] LoadScores()
    {
        int[] ret = new int[9];
        string path = Application.persistentDataPath + "/Scores/hiscores.score";
        if(File.Exists(path))
        {
            Debug.Log("File foudn in " + path);
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);
            ret = (int[])formatter.Deserialize(stream);
            stream.Close();
        }
        else
        {
            Debug.LogWarning("A save file was not found, creating a new one");
            for (int i = 0; i < 9; ++i)
                ret[i] = 0;
        }
        return ret;
    }
}
