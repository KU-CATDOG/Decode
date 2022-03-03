using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InSceneTeleport : MonoBehaviour
{

    public GameObject Destination;

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (Input.GetKeyDown(KeyCode.G))
        {
            FindObjectOfType<Player>().GetComponent<Transform>().position = Destination.transform.position;
        }
    }

}
