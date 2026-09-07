using System.IO;
using UnityEngine;

public static class SaveManager
{

    public static bool StartNewGameRequested { get; private set; }
    public static bool LoadGameRequested { get; private set; }
    private static string SavePath =>
        Path.Combine(
            Application.persistentDataPath,
            "fishangle_save.json"
        );

    public static void SaveGame(
        SaveData data
    )
    {
        if (data == null)
        {
            Debug.LogWarning(
                "SaveGame wurde ohne SaveData aufgerufen."
            );

            return;
        }

        string json =
            JsonUtility.ToJson(
                data,
                true
            );

        File.WriteAllText(
            SavePath,
            json
        );

        Debug.Log(
            "Spiel gespeichert: " +
            SavePath
        );
    }

    public static SaveData LoadGame()
    {
        if (!SaveExists())
        {
            return null;
        }

        string json =
            File.ReadAllText(
                SavePath
            );

        SaveData data =
            JsonUtility.FromJson<SaveData>(
                json
            );

        return data;
    }

    public static bool SaveExists()
    {
        return File.Exists(
            SavePath
        );
    }

    public static void DeleteSave()
    {
        if (!SaveExists())
        {
            return;
        }

        File.Delete(
            SavePath
        );

        Debug.Log(
            "Spielstand gelöscht."
        );
    }
    public static void RequestNewGame()
    {
        StartNewGameRequested = true;
        LoadGameRequested = false;
    }

    public static void RequestContinue()
    {
        StartNewGameRequested = false;
        LoadGameRequested = true;
    }

    public static void ClearSceneRequest()
    {
        StartNewGameRequested = false;
        LoadGameRequested = false;
    }
}