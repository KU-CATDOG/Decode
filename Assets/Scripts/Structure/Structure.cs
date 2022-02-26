using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Structure : Changable
{
    public float Health { get; protected set; }
    public float MaxHealth { get; protected set; }
    public float MP { get; protected set; }
    public float MaxMP { get; protected set; }
    public float MovementSpeed { get; protected set; }
    public float MaxMovementSpeed { get; protected set; }

    protected Player player;
    protected Rigidbody2D rb;

    public bool onHit;

    protected virtual void Start()
    {
        InitializeBars();
        player = FindObjectOfType<Player>();
        rb = GetComponent<Rigidbody2D>();
    }
}
