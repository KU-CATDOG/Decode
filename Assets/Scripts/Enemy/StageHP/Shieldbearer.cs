using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shieldbearer : Enemy
{
    private bool chargeMP = false;
    private bool trigger = false;
    private float timer = 0f;
    private float shield = 0f;
    private bool stunned = false;

    private Vector3 patrolPoint;
    float distToPatrol;
    Vector3 point1, point2;
    bool moveRight = false;

    [SerializeField]
    private GameObject collider;

    protected override void Start()
    {
        MaxHealth = Health = 10f;
        AttackDamage = 5f;
        Range = 1.5f;       // 공격 범위
        Eyesight = 10f;     // 시야
        MovementSpeed = 0.7f;
        Interval = 2.0f;
        patrolPoint = transform.position;
        point1 = patrolPoint - new Vector3(5f, 0, 0);
        point2 = patrolPoint + new Vector3(5f, 0, 0);
        ConnectValue(Define.ChangableValue.Hp, typeof(Enemy).GetProperty("MaxHealth"), typeof(Enemy).GetProperty("Health"));
        
        base.Start();
    }

    protected override Queue<IEnumerator> DecideNextRoutine()
    {
        Queue<IEnumerator> nextRoutines = new Queue<IEnumerator>();

        distToPatrol = Vector3.Distance(patrolPoint, GetObjectPos());

        Debug.Log(DistToPlayer());

        if (CheckPlayer())
        {

            if (DistToPlayer() < Eyesight) // 플레이어가 시야 안에 들어왔다
            {
                if (DistToPlayer() < Range)
                {
                    nextRoutines.Enqueue(NewActionRoutine(AttackRoutine(AttackDamage)));
                }
                else
                {
                    if(distToPatrol < 5f)
                    {
                        if (GetComponent<Rigidbody2D>().velocity.y >= 0)
                            nextRoutines.Enqueue(NewActionRoutine(MoveTowardPlayerHorizontal(MovementSpeed)));
                        else nextRoutines.Enqueue(NewActionRoutine(WaitRoutine(1f)));
                    }
                    else
                    {
                        nextRoutines.Enqueue(NewActionRoutine(WaitRoutine(0.5f)));
                    }

                }
            }
            else
            {
                nextRoutines.Enqueue(NewActionRoutine(Patrol()));

            }
        } 
        else nextRoutines.Enqueue(NewActionRoutine(WaitRoutine(1f)));

        return nextRoutines;
    }
    private IEnumerator AttackRoutine(float dmg)
    {
        if (CheckPlayer())
        {
            yield return new WaitForSeconds(Interval);
            if(DistToPlayer() < Range)  GameManager.Instance.GetDamaged(dmg);
        }
        else yield return null;
    }

    private IEnumerator Patrol()
    {
        Vector2 dir;
        if(moveRight && Mathf.Abs(transform.position.x - point2.x) < 0.1)
        {
            moveRight = false;
            yield return new WaitForSeconds(1f);
        }
        if(!moveRight && Mathf.Abs(transform.position.x - point1.x) < 0.1)
        {
            moveRight = true;
            yield return new WaitForSeconds(1f);

        }
        if (moveRight)
        {
            dir = (point2 - GetObjectPos()).normalized;
            dir.y = 0;
            rb.MovePosition(rb.position + dir * MovementSpeed * Time.fixedDeltaTime);
            yield return null;

        }
        else
        {
            dir = (point1 - GetObjectPos()).normalized;
            dir.y = 0;
            rb.MovePosition(rb.position + dir * MovementSpeed * Time.fixedDeltaTime);
            yield return null;

        }
        yield return null;
    }

    private IEnumerator BackToPatrolPoint()
    {
        Vector2 direction = (patrolPoint - GetObjectPos()).normalized;
        direction.y = 0;
        rb.MovePosition(rb.position + direction * MovementSpeed * Time.fixedDeltaTime);
        yield return null;
    }


}
