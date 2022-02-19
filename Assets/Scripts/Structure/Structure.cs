using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Structure : Changable
{
    public float Health { get; protected set; }
    public float MaxHealth { get; protected set; }
    public float MP { get; protected set; }
    public float MaxMP { get; protected set; }

    protected Player player;
    protected Rigidbody2D rb;

    public bool onHit;

    protected virtual void Start()
    {
        InitializeBars();
        player = FindObjectOfType<Player>();
        rb = GetComponent<Rigidbody2D>();
    }
    
    private void Update()
    {
        if (Health <= 0f)
        {
            for (int i = 0; i < barofChangableValues.Length; i++)
            {
                Destroy(barofChangableValues[i].gameObject);
            }
            Destroy(gameObject);

        }
    }
}
