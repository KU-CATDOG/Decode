using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    [SerializeField] private GameObject defaultText;
    private GameObject text;
    private GameObject door;
    private GameObject player;
    private Canvas canvas;


    void Awake()
    {
        canvas = FindObjectOfType<Canvas>();
        door = transform.parent.gameObject;
        player = GameObject.Find("Player");
        text = Instantiate(defaultText, canvas.transform);
        text.gameObject.SetActive(false);
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            Vector3 panelPos = Camera.main.WorldToScreenPoint(new Vector3(player.transform.position.x, player.transform.position.y + 2.0f, 0));
            text.transform.position = panelPos;
            text.gameObject.SetActive(true);
        }
        if (other.gameObject.tag == "Player" && Input.GetKey(KeyCode.G))
        {
            text.gameObject.SetActive(false);
            Destroy(door);
        }
    }
    
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
            text.gameObject.SetActive(false);
    }
}