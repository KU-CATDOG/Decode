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
        Range = 5f;
    }

    protected override Queue<IEnumerator> DecideNextRoutine()
    {
        Queue<IEnumerator> nextRoutines = new Queue<IEnumerator>();

        if (CheckPlayer())
        {
            if(DistToPlayer() < 1.5f) // 플레이어가 공격 범위안에 들어왔다
            {
                nextRoutines.Enqueue(NewActionRoutine(AttackRoutine(AttackDamage)));
            }
            else
            {
                nextRoutines.Enqueue(NewActionRoutine(MoveTowardPlayer(3.0f)));
            }
        }
        else nextRoutines.Enqueue(NewActionRoutine(WaitRoutine(1f)));

        return nextRoutines;
    }

    private IEnumerator AttackRoutine(float dmg)
    {
        if (CheckPlayer())
        {
            player.GetDamaged(dmg);
            yield return new WaitForSeconds(1f);
        }
        else yield return null;
    }
}
