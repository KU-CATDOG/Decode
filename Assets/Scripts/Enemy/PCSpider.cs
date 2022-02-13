using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PCSpider : Enemy
{
    protected override void Start()
    {
        MaxHealth = Health = 10f;
        AttackDamage = 5f;
        MovementSpeed = 1f;
        MaxMovementSpeed = 10f;
        Range = 1.5f;     // 공격 범위
        Eyesight = 10f;
        Interval = 1.0f;
        ConnectValue(Define.ChangableValue.Hp, typeof(Enemy).GetProperty("MaxHealth"), typeof(Enemy).GetProperty("Health"));
        base.Start();
    }

    protected override Queue<IEnumerator> DecideNextRoutine()
    {
        Queue<IEnumerator> nextRoutines = new Queue<IEnumerator>();

        if (CheckPlayer())
        {
            if (DistToPlayer() < Eyesight) // 플레이어가 시야 안에 들어왔다
            {
                if(DistToPlayer() < Range)  nextRoutines.Enqueue(NewActionRoutine(AttackRoutine(AttackDamage)));
                else
                {
                    if (GetComponent<Rigidbody2D>().velocity.y >= 0)
                        nextRoutines.Enqueue(NewActionRoutine(MoveTowardPlayer(MovementSpeed)));
                    else nextRoutines.Enqueue(NewActionRoutine(WaitRoutine(1f)));
                }
            }
            else
            {
                nextRoutines.Enqueue(NewActionRoutine(WaitRoutine(1f)));
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
