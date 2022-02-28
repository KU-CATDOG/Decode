using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingGear : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            GameManager.Instance.GetDamaged(5);
        }
        if(collision.tag == "Floor")
        {
            Destroy(gameObject);
        }
    }
}
