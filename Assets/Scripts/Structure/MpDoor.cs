using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MpDoor : Structure
{
    private void Start()
    {
        MaxHealth = Health = 10f;
        MaxMP = MP = 20f;
        ConnectValue(Define.ChangableValue.Hp, typeof(Structure).GetProperty("MaxHealth"), typeof(Structure).GetProperty("Health"));
        ConnectValue(Define.ChangableValue.Mp, typeof(Structure).GetProperty("MaxMP"), typeof(Structure).GetProperty("MP"));
        base.Start();
    }
    protected override float AddValue(float value)
    {
        if(GetCurSelected() == Define.ChangableValue.Hp && MP != 0)
        {
            return value;
        }
        return base.AddValue(value);
    }
}