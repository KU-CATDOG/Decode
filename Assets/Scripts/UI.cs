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
}
