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
            GameManager.Instance.prevScene = "MainMenu";
            SceneManager.LoadScene(1);
            GameManager.Instance.player.gameObject.SetActive(true);
            GameManager.Instance.player.isControllable = true;
        }
        else
        {
            GameManager.Instance.player.gameObject.SetActive(true);
            SaveManager.Instance.LoadSaveData(SaveManager.Instance.lastSaveNumber);
            GameManager.Instance.player.isControllable = true;

        }
    }
}
