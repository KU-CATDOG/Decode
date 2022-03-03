using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class StartTheCode : MonoBehaviour
{
    public void OnClickExit()
    {
        if(SaveManager.Instance.lastSaveNumber == 0)
        {
            SceneManager.LoadScene(1);
        }
        else
        {
            SaveManager.Instance.LoadSaveData(SaveManager.Instance.lastSaveNumber);
        }
    }
}
