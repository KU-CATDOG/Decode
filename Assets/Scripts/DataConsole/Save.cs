using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Save : MonoBehaviour
{
    [SerializeField] private int saveNumber;
    [SerializeField] private GameObject savePanel;
    [SerializeField] private GameObject buttonPanel;


    protected Player player;
    protected DataConsole dataConsole;

    protected virtual void Start()
    {
        player = FindObjectOfType<Player>();
        dataConsole = FindObjectOfType<DataConsole>();
        if(SaveManager.Instance.saveScene[saveNumber - 1]!=null)
            transform.GetChild(0).GetComponent<Text>().text = SaveManager.Instance.saveScene[saveNumber - 1];
    }

    public void OnClickExit()
    {
        SaveManager.Instance.playerHealth[saveNumber-1] = player.health;
        SaveManager.Instance.dataConsoleNumber[saveNumber - 1] = dataConsole.consoleNumber;
        SaveManager.Instance.saveScene[saveNumber - 1] = dataConsole.sceneName;
        SaveManager.Instance.lastSaveNumber = saveNumber;
        SaveManager.Instance.signlockActivated[saveNumber-1] = player.Signlock;
        SaveManager.Instance.lockedDoorKey = player.lockedDoorKey;
        transform.GetChild(0).GetComponent<Text>().text = SaveManager.Instance.saveScene[saveNumber - 1];
        savePanel.gameObject.SetActive(false);
        buttonPanel.gameObject.SetActive(true);
    }
}
