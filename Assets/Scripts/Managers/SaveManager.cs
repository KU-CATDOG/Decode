using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class SaveManager : Singleton<SaveManager>
{
    public bool[] activeDataConsole = new bool[9];
    public string[] activeDataConsoleSceneName = new string[9];
    
    public float[] playerHealth = new float[3];
    public int[] dataConsoleNumber = new int[3];
    public string[] saveScene = new string[3];
    public bool[] signlockActivated = new bool[3];
    public bool[] mplockActivated = new bool[3];
    public bool[] speedlockActivated = new bool[3];

    public bool[] lockedDoorKey = new bool[9];

    public int lastSaveNumber;

    private Player player;

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

    public void LoadSaveData(int saveNumber)
    {
        player = FindObjectOfType<Player>();
        GameManager.Instance.prevScene = "dataConsole";
        player.health = playerHealth[saveNumber - 1];
        player.Signlock = signlockActivated[saveNumber - 1];
        player.MPChangeLock = mplockActivated[saveNumber - 1];
        player.SpeedChangeLock = speedlockActivated[saveNumber - 1];
        player.lockedDoorKey = lockedDoorKey;
        SceneManager.LoadScene(saveScene[saveNumber - 1]);
    }

}
