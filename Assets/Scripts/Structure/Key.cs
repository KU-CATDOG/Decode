using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Key : MonoBehaviour
{
    public int keyNumber;
    
    private GameObject player;

    void Awake()
    {
        player = GameObject.Find("Player");
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            SaveManager.Instance.lockedDoorKey[keyNumber - 1] = true;
            Destroy(gameObject);
        }
    }
}
