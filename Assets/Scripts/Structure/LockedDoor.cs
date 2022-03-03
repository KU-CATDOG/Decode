using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockedDoor : MonoBehaviour
{
    private GameObject door;
    private GameObject player;
    private GameObject key;
    private bool alreadyDroppedKey = false;
    private string monsterName;
    private Vector3 whereDropKey;

    [SerializeField] private int doorNumber;
    [SerializeField] private GameObject keyObject;
    [SerializeField] private GameObject monsterHadKey;

    void Awake()
    {
        door = transform.parent.gameObject;
        player = GameObject.Find("Player");
        monsterName = monsterHadKey.name;
    }

    void Update()
    {
        if(GameObject.Find(monsterName) != null)
            whereDropKey = monsterHadKey.transform.position;
        if (GameObject.Find(monsterName) == null && !alreadyDroppedKey)
        {
            key = Instantiate(keyObject, whereDropKey, transform.rotation);
            key.GetComponent<Key>().keyNumber = doorNumber;
            alreadyDroppedKey = true;
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player" && Input.GetKey(KeyCode.G) && Player.Instance.lockedDoorKey[doorNumber-1])
        {
            Destroy(door);
        }
    }

}