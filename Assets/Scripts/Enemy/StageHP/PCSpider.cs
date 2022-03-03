using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PCSpider : Enemy
{
    Animator anim;
    protected override void Start()
    {
        anim = GetComponent<Animator>();

        MaxHealth = Health = 10f;
        AttackDamage = 5f;
        MovementSpeed = 1f;
        MaxMovementSpeed = 10f;
        Range = 2.5f;     // 공격 범위
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
                Vector3 dir = GetPlayerPos() - transform.position;
                float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
                bool isLookRight = angle < 90 && angle > -90;
                GetComponent<SpriteRenderer>().flipX = isLookRight;

                if (DistToPlayer() < Range) 
                {
                    anim.SetBool("Moving", false);
                    nextRoutines.Enqueue(NewActionRoutine(AttackRoutine(AttackDamage)));
                } 
                else
                {
                    anim.SetBool("Moving", true);

                    if (GetComponent<Rigidbody2D>().velocity.y >= 0)
                        nextRoutines.Enqueue(NewActionRoutine(MoveTowardPlayerHorizontal(MovementSpeed)));
                    else nextRoutines.Enqueue(NewActionRoutine(WaitRoutine(1f)));
                }
            }
            else
            {
                anim.SetBool("Moving", false);
                nextRoutines.Enqueue(NewActionRoutine(WaitRoutine(1f)));
            }
        }
        else nextRoutines.Enqueue(NewActionRoutine(WaitRoutine(1f)));

        return nextRoutines;
    }

    private IEnumerator AttackRoutine(float dmg)
    {

        anim.SetTrigger("Attack");
        yield return new WaitForSeconds(Interval);

        if (CheckPlayer())
        {
            if(DistToPlayer() < Range)
            {
                GameManager.Instance.GetDamaged(dmg);

            }
        }
        else yield return null;
    }
}
