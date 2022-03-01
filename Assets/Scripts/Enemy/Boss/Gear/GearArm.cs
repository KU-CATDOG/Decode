using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GearArm : MonoBehaviour
{
    private Gear gear;
    private void Awake()
    {
        gear = transform.parent.GetComponent<Gear>();
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if(collision.tag == "Player" && gear.OnNormalAttack && !gear.NormalAttackHit)
        {
            gear.NormalAttackHit = true;
            GameManager.Instance.GetDamaged(10f);
        }
    }
}
