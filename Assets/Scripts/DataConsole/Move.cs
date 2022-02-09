using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Move : MonoBehaviour
{
    [SerializeField] private int consoleNumber;

    void Start()
    {
        if (SaveManager.Instance.activeDataConsole[consoleNumber - 1])
            this.gameObject.SetActive(true);
    }

    public void OnClickExit()
    {
        GameManager.Instance.prevScene = "dataConsole";
        Time.timeScale = 1;
        SceneManager.LoadScene(SaveManager.Instance.activeDataConsoleSceneName[consoleNumber - 1]);
    }
}
