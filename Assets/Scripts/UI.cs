using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UI : MonoBehaviour
{
    [SerializeField]
    private Button startButton;
    [SerializeField]
    private Button exitButton;
    [SerializeField]
    private Button openSettings;
    [SerializeField]
    private Button closeSettings;
    [SerializeField]
    private GameObject settings;
    [SerializeField]
    private GameObject reload;
    [SerializeField]
    private GameObject cancel;

    private int sceneToLoad;

    private void Start()
    {

    }

    #region MainMenu
    public void Play()
    {
        // Load Scene
        //SceneManager.LoadScene(sceneToLoad);
    }

    public void Quit()
    {
        Application.Quit();
        Debug.Log("quit");
    }
    #endregion

    public void SettingsButton()
    {
        Time.timeScale = 0;
        settings.SetActive(true);
        if (GameManager.Instance.player != null)
        {
            GameManager.Instance.player.isControllable = false;
        }
    }

    public void CloseSettingsButton()
    {
        Time.timeScale = 1;
        settings.SetActive(false);
        if (GameManager.Instance.player != null)
        {
            GameManager.Instance.player.isControllable = true;
        }
    }

    public void RestartButton()
    {
        settings.SetActive(false);
        reload.SetActive(true);
    }

    public void ToMenuButton()
    {
        cancel.SetActive(true);
        settings.SetActive(false);
    }

    public void ReloadReload()
    {
        Time.timeScale = 1;
        if (GameManager.Instance.player != null)
        {
            GameManager.Instance.player.isControllable = true;
        }
        SaveManager.Instance.LoadSaveData(SaveManager.Instance.lastSaveNumber);
    }
    public void ReloadCancel()
    {
        settings.SetActive(true);
        reload.SetActive(false);
    }
    public void QuitQuit()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("MainMenu");
    }
    public void QuitCancel()
    {
        cancel.SetActive(false);
        settings.SetActive(true);
    }
}