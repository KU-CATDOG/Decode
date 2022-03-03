using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MoveScene : MonoBehaviour
{
    [SerializeField] private string curScene;
    [SerializeField] private string nextScene;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.GetComponent<Player>() != null)
        {
            GameManager.Instance.prevScene = curScene;
            SceneManager.LoadScene(nextScene);
            GameManager.Instance.player.horizontal = 0;
        }

    }
}
