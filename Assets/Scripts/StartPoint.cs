using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartPoint : MonoBehaviour
{
    [SerializeField] private string prevScene;

    private Player player;
    private GameObject dataConsole;

    void Start()
    {
        player = FindObjectOfType<Player>();

        if (prevScene == GameManager.Instance.prevScene)
        {
            player.transform.position = this.transform.position;
        }
        else if(GameManager.Instance.prevScene == "dataConsole")
        {
            dataConsole = GameObject.Find("DataConsole");
            player.transform.position = dataConsole.transform.position;
        }
        
    }

}
