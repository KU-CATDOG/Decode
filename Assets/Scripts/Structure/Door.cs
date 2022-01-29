using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    [SerializeField] private GameObject text;
    private GameObject door;
    private GameObject player;

    void Awake()
    {
        door = transform.parent.gameObject;
        player = GameObject.Find("Player");
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        Vector3 panelPos = Camera.main.WorldToScreenPoint(new Vector3(player.transform.position.x, player.transform.position.y + 2.0f, 0));
        text.transform.position = panelPos;
        text.gameObject.SetActive(true);
        if (other.gameObject.tag == "Player" && Input.GetKey(KeyCode.G))
        {
            text.gameObject.SetActive(false);
            Destroy(door);
        }
    }
    
    private void OnTriggerExit2D(Collider2D other)
    {
        text.gameObject.SetActive(false);
    }
}