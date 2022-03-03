using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class StartTheCode : MonoBehaviour
{
    [SerializeField]
    GameObject Player;
    public void OnClickExit()
    {
        if(SaveManager.Instance.lastSaveNumber == 0)
        {
            Player.SetActive(true);
            GameManager.Instance.prevScene = "MainMenu";
            SceneManager.LoadScene(0);
        }
        else
        {
            Player.SetActive(true);
            SaveManager.Instance.LoadSaveData(SaveManager.Instance.lastSaveNumber);
        }
    }
}
