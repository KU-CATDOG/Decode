using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HpDoor : Structure
{

    private void Start()
    {
        MaxHealth = Health = 10f;
        ConnectValue(Define.ChangableValue.Hp, typeof(Structure).GetProperty("MaxHealth"), typeof(Structure).GetProperty("Health"));
        base.Start();
    }
}