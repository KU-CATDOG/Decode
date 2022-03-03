using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Load : MonoBehaviour
{
    [SerializeField] private int loadNumber;

    private Player player;

    public void Start()
    {
        player = FindObjectOfType<Player>();
        if (SaveManager.Instance.saveScene[loadNumber - 1] != null)
            transform.GetChild(0).GetComponent<Text>().text = SaveManager.Instance.saveScene[loadNumber - 1];
    }

    

    public void OnClickExit()
    {
        if (SaveManager.Instance.playerHealth[loadNumber - 1] > 0)
        {
            Time.timeScale = 1;
            SaveManager.Instance.LoadSaveData(loadNumber);
        }
    }
}