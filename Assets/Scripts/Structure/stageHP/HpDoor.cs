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

}