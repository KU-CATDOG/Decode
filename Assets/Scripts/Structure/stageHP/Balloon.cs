using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Balloon : Structure
{
    private Rigidbody2D rb;

    [SerializeField] private float damage;
    [SerializeField] private float hp;


    private bool canBoom=false;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        MaxHealth = Health = hp;
        ConnectValue(Define.ChangableValue.Hp, typeof(Structure).GetProperty("MaxHealth"), typeof(Structure).GetProperty("Health"));
        base.Start();
    }

    private void Update()
    {
        if (Health <= 0f)
        {
            rb.velocity = new Vector3(0, 1, 0);
            canBoom = true;
        }
    }

    private void OnTriggerEnter2D (Collider2D other)
    {
        if(canBoom)
        { 
            if (other.tag=="Player")
                GameManager.Instance.GetDamaged(damage/2);
            for (int i = 0; i < barofChangableValues.Length; i++)
            {
                Destroy(barofChangableValues[i].gameObject);
            }
            Destroy(gameObject);
        }
    }


}
