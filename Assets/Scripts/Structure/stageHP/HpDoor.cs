using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HpDoor : Structure
{
    [SerializeField] private float hp;

    private void Start()
    {
        MaxHealth = Health = hp;
        ConnectValue(Define.ChangableValue.Hp, typeof(Structure).GetProperty("MaxHealth"), typeof(Structure).GetProperty("Health"));
        base.Start();
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