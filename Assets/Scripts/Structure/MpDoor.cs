using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MpDoor : Structure
{
    [SerializeField]
    SpriteRenderer sprite;
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
        float nextValue = base.AddValue(value);
        if(nextValue == 0 && GetCurSelected() == Define.ChangableValue.Mp)
        {
            sprite.color = new Color(1, 0, 0, 100 / 255f);
        }
        return nextValue;
    }
}