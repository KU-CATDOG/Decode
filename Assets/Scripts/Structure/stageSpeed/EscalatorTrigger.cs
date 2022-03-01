using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EscalatorTrigger : MonoBehaviour
{
    private Escalator escalator;

    void Start()
    {
        escalator = transform.parent.gameObject.GetComponent<Escalator>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            escalator.underPlayer = true;
        }
    }
    
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            escalator.underPlayer = false;
        }
    }
}
