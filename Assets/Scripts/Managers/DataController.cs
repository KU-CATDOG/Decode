using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

[System.Serializable]
public class SaveData
{
    public bool[] activeDataConsole = new bool[9];
    public string[] activeDataConsoleSceneName = new string[9];
    public float[] playerHealth = new float[3];
    public int[] dataConsoleNumber = new int[3];
    public string[] saveScene = new string[3];
    public bool[] lockedDoorKey = new bool[9];
    public bool[] signlockActivated = new bool[3];
    public bool[] mplockActivated = new bool[3];
    public bool[] speedlockActivated = new bool[3];

    public int lastSaveNumber;

}

public class DataController : Singleton<DataController>
{
    string path;

    void Start()
    {
        path = Path.Combine(Application.dataPath, "DecodeData.json");
        JsonLoad();
    }

    public void JsonLoad()
    {
        SaveData saveData = new SaveData();

        if (!File.Exists(path))
        {
            SaveManager.Instance.lastSaveNumber = 0;
            JsonSave();
        }
        else
        {
            string loadJson = File.ReadAllText(path);
            saveData = JsonUtility.FromJson<SaveData>(loadJson);

            if (saveData != null)
            {

                SaveManager.Instance.activeDataConsole = saveData.activeDataConsole;
                SaveManager.Instance.activeDataConsoleSceneName = saveData.activeDataConsoleSceneName;
                SaveManager.Instance.playerHealth = saveData.playerHealth;
                SaveManager.Instance.dataConsoleNumber = saveData.dataConsoleNumber;
                SaveManager.Instance.saveScene = saveData.saveScene;
                SaveManager.Instance.lockedDoorKey = saveData.lockedDoorKey;
                SaveManager.Instance.lastSaveNumber = saveData.lastSaveNumber;
                SaveManager.Instance.signlockActivated = saveData.signlockActivated;
                SaveManager.Instance.mplockActivated = saveData.mplockActivated;
                SaveManager.Instance.speedlockActivated = saveData.speedlockActivated;

            }
        }
    }

    public void JsonSave()
    {
        SaveData saveData = new SaveData();

        saveData.activeDataConsole = SaveManager.Instance.activeDataConsole;
        saveData.activeDataConsoleSceneName = SaveManager.Instance.activeDataConsoleSceneName;
        saveData.playerHealth = SaveManager.Instance.playerHealth;
        saveData.dataConsoleNumber = SaveManager.Instance.dataConsoleNumber;
        saveData.saveScene = SaveManager.Instance.saveScene;
        saveData.lockedDoorKey = SaveManager.Instance.lockedDoorKey;
        saveData.lastSaveNumber = SaveManager.Instance.lastSaveNumber;
        saveData.signlockActivated = SaveManager.Instance.signlockActivated;
        saveData.mplockActivated = SaveManager.Instance.mplockActivated;
        saveData.speedlockActivated = SaveManager.Instance.speedlockActivated;

        string json = JsonUtility.ToJson(saveData, true);

        if(path != null)    File.WriteAllText(path, json);
    }
}