using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveManager : Singleton<SaveManager>
{
    public bool[] activeDataConsole = new bool[9];
    public string[] activeDataConsoleSceneName = new string[9];
    public float[] playerHealth = new float[3];
    public int[] dataConsoleNumber = new int[3];
    public string[] saveScene = new string[3];
    public bool[] lockedDoorKey = new bool[9];

    public int lastSaveNumber;

    void Start()
    {
        DontDestroyOnLoad(gameObject);
        DataController.Instance.JsonLoad();
        DataController.Instance.JsonSave();
    }

    private void OnApplicationQuit()
    {
        DataController.Instance.JsonSave();
    }
}
