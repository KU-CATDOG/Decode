using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stone : Structure
{
    private Rigidbody2D rb;

    [SerializeField] private float speed;
    [SerializeField] private float maxSpeed;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        MovementSpeed = speed;
        MaxMovementSpeed = maxSpeed;
        ConnectValue(Define.ChangableValue.Speed, typeof(Structure).GetProperty("MaxMovementSpeed"), typeof(Structure).GetProperty("MovementSpeed"));
        base.Start();
    }

    void Update()
    {
        if (MovementSpeed >= 1)
            rb.gravityScale = MovementSpeed;
        else
            rb.gravityScale = 1.0f;
    }
   

    public void DestroyStone()
    {
        for (int i = 0; i < barofChangableValues.Length; i++)
        {
            Destroy(barofChangableValues[i].gameObject);
        }
        Destroy(gameObject);
    }
}
