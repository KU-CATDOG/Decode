using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHitBox : HitBox
{
    public int Dmg { get; set; }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            GameManager.Instance.GetDamaged(Dmg);
        }
    }
}
