using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefaultWeapon : Weapon // 값의 더하기, 빼기를 구현하는 무기
{

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log($"Trigger: {collision}");
        hb.gameObject.SetActive(false);
        Changable changable;
        if (changable = collision.GetComponent<Changable>()){
            if (mouse == Define.MouseEvent.LClick)
            {
                changable.AddValue(magnitudeOfChange[changable.Selected]);
            }
            else
            {
                changable.SelectValuetoChange();
            }
        }
    }
}
