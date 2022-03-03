using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DataConsole : MonoBehaviour
{
    [SerializeField] 
    private GameObject defaultPanel;
    

    public int consoleNumber;
    public string sceneName;

    private GameObject panel;
    private GameObject dataConsole;
    private GameObject player;
    private Canvas canvas;   


    void Awake()
    {
        canvas = FindObjectOfType<Canvas>();
        dataConsole = transform.parent.gameObject;
        player = GameObject.Find("Player");
        sceneName = SceneManager.GetActiveScene().name;
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        

        if (other.gameObject.tag == "Player" && Input.GetKey(KeyCode.G))
        {
            panel = Instantiate(defaultPanel, canvas.transform);
            Vector3 panelPos = Camera.main.WorldToScreenPoint(new Vector3(player.transform.position.x, player.transform.position.y + 2.0f, 0));
            SaveManager.Instance.activeDataConsole[consoleNumber-1] = true;
            SaveManager.Instance.activeDataConsoleSceneName[consoleNumber - 1] = sceneName;
            Time.timeScale = 0;
        }
    }

}
