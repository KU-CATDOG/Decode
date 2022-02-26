using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StalactiteTrigger : MonoBehaviour
{
    private Stalactite stalactite;

    void Start()
    {
        stalactite = transform.parent.gameObject.GetComponent<Stalactite>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            stalactite.rb.gravityScale = 1.0f;
        }
    }
}
