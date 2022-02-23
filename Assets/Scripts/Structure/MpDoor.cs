using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MpDoor : Structure
{
    private void Start()
    {
        MaxHealth = Health = 1000f;
        MaxMP = MP = 10f;
        ConnectValue(Define.ChangableValue.Hp, typeof(Structure).GetProperty("MaxHealth"), typeof(Structure).GetProperty("Health"));
        ConnectValue(Define.ChangableValue.Mp, typeof(Structure).GetProperty("MaxMP"), typeof(Structure).GetProperty("MP"));
        base.Start();
    }

    private void FixedUpdate()
    {
        if(MP <=0f)
        {
            Health = 0f;
        }
    }
}