using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{

    public GameObject host;
    public float dmg;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.GetComponent<Player>() != null)
        {
            GameManager.Instance.GetDamaged(dmg);
            Destroy(gameObject);
        }

    }
}
