using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Presser : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            GameManager.Instance.GetDamaged(10f);
        }
    }
}
