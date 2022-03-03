using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Load : Singleton<Load>
{
    [SerializeField] private int loadNumber;

    private Player player;

    public void Start()
    {
        player = FindObjectOfType<Player>();
        if (SaveManager.Instance.saveScene[loadNumber - 1] != null)
            transform.GetChild(0).GetComponent<Text>().text = SaveManager.Instance.saveScene[loadNumber - 1];
    }

    public void LoadSaveData(int saveNumber)
    {
        GameManager.Instance.prevScene = "dataConsole";
        player.health = SaveManager.Instance.playerHealth[saveNumber - 1];
        SceneManager.LoadScene(SaveManager.Instance.saveScene[saveNumber - 1]);
        player.Signlock = SaveManager.Instance.signlockActivated[saveNumber - 1];
    }

    public void OnClickExit()
    {
        Time.timeScale = 1;
        LoadSaveData(loadNumber);
    }
}