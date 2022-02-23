using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Boss : Enemy
{
    protected override void Start()
    {
        InitializeBars(true);
        player = FindObjectOfType<Player>();
        rb = GetComponent<Rigidbody2D>();

        //FIXME
        //Physics2D.IgnoreCollision(player.GetComponent<Collider2D>(), GetComponent<Collider2D>());
        Physics2D.IgnoreLayerCollision(8, 8);
        Physics2D.IgnoreLayerCollision(7, 8);

    }
}
