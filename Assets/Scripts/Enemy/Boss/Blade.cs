using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blade : MonoBehaviour
{
    [SerializeField]
    private int dmg;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            GameManager.Instance.GetDamaged(dmg);
        }
    }
}
