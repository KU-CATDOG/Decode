using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Melee : Enemy
{
    protected override void Start()
    {
        base.Start();
        MaxHealth = Health = 10f;
        AttackDamage = 5f;
        MovementSpeed = 1f;
        Range = 1.5f;     // 공격 범위
        Interval = 1.0f;
    }

    protected override Queue<IEnumerator> DecideNextRoutine()
    {
        Queue<IEnumerator> nextRoutines = new Queue<IEnumerator>();

        if (CheckPlayer())
        {
            if(DistToPlayer() < Range) // 플레이어가 공격 범위안에 들어왔다
            {
                nextRoutines.Enqueue(NewActionRoutine(AttackRoutine(AttackDamage)));
            }
            else
            {
                nextRoutines.Enqueue(NewActionRoutine(MoveTowardPlayer(MovementSpeed)));
            }
        }
        else nextRoutines.Enqueue(NewActionRoutine(WaitRoutine(1f)));

        return nextRoutines;
    }

    private IEnumerator AttackRoutine(float dmg)
    {
        if (CheckPlayer())
        {
            GameManager.Instance.GetDamaged(dmg);
            yield return new WaitForSeconds(Interval);
        }
        else yield return null;
    }
}
