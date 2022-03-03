using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flying : Enemy
{
    public bool attacking = false;
    private float flightTime = 1f;

    protected override void Start()
    {
        MaxHealth = Health = 10f;
        AttackDamage = 5f;
        Eyesight = 10f;    // 시야 범위
        MovementSpeed = 1f;
        ConnectValue(Define.ChangableValue.Hp, typeof(Enemy).GetProperty("MaxHealth"), typeof(Enemy).GetProperty("Health"));

        base.Start();
    }

    protected override Queue<IEnumerator> DecideNextRoutine()
    {
        Queue<IEnumerator> nextRoutines = new Queue<IEnumerator>();

        if (CheckPlayer())
        {
            if (DistToPlayer() < Eyesight) // 플레이어가 시야 범위안에 들어왔다
            {
                if (!attacking)
                {
                    Vector3 dir = GetPlayerPos() - transform.position;
                    float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
                    bool isLookRight = angle < 90 && angle > -90;
                    GetComponent<SpriteRenderer>().flipX = !isLookRight;
                }
                if (DistToPlayer() < Eyesight / 2)
                {
                    nextRoutines.Enqueue(NewActionRoutine(AttackRoutine()));
                    nextRoutines.Enqueue(NewActionRoutine(WaitRoutine(1f)));

                }
                else nextRoutines.Enqueue(NewActionRoutine(MoveTowardPlayer(MovementSpeed)));
            }
            else
            {
                nextRoutines.Enqueue(NewActionRoutine(WaitRoutine(1f)));

            }       

        }
        else nextRoutines.Enqueue(NewActionRoutine(WaitRoutine(1f)));

        return nextRoutines;
    }
    
    private IEnumerator AttackRoutine()
    {
        Vector3 posDiff = GetPlayerPos() - GetObjectPos();
        attacking = true;
        StartCoroutine(MoveRoutine(GetPlayerPos() + posDiff, flightTime));
        yield return new WaitForSeconds(1f);
        attacking = false;
        //yield return null;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (attacking && collision.GetComponent<Player>() != null) GameManager.Instance.GetDamaged(1f);
    }

}
