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

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.name=="HitBox")
        { 
            Vector3 currentVelocity = rb.velocity;
            rb.velocity = new Vector3(currentVelocity.x +MovementSpeed, currentVelocity.y, currentVelocity.z);
        }
    }
}
