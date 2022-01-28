using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Snail : Enemy
{
    private bool chargeMP = false;
    private bool trigger = false;
    private float timer = 0f;
    private float shield = 0f;

    protected override void Start()
    {
        base.Start();
        MaxHealth = Health = 10f;
        AttackDamage = 5f;
        MP = 0f;
        MaxMP = 100f;
        Range = 1.5f;    // 공격 범위
        MovementSpeed = 1f;
        Interval = 1.0f;
    }
    private void FixedUpdate()  //FIXME
    {
        timer += Time.deltaTime;
        if (timer >= 1.0f && MP < MaxMP)
        {
            MP += 10f;
            timer = 0f;
            Debug.Log(MP);
        }

        //TODO : 플레이어의 공격으로 MP가 0이 되면 보호막이 없어지고 2초동안 기절
        
    }

    protected override Queue<IEnumerator> DecideNextRoutine()
    {
        Queue<IEnumerator> nextRoutines = new Queue<IEnumerator>();

        if (CheckPlayer())
        {
            if (MP >= MaxMP) // MP가 100상태에서 플레이어와 접촉했다
            {
                nextRoutines.Enqueue(NewActionRoutine(GenerateShield()));
            }
            else
            {
                if (DistToPlayer() < Range) // 플레이어가 공격 범위안에 들어왔다
                {
                    nextRoutines.Enqueue(NewActionRoutine(AttackRoutine(AttackDamage)));
                }
                else
                {
                    nextRoutines.Enqueue(NewActionRoutine(MoveTowardPlayer(MovementSpeed)));
                }
            }

        }
        else nextRoutines.Enqueue(NewActionRoutine(WaitRoutine(1f)));

        return nextRoutines;
    }

    private IEnumerator GenerateShield()
    {
        Health += (float)(MaxHealth * 0.2); MP = 0;
        yield return new WaitForSeconds(0.5f);
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
