using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Snail : Enemy
{
    private bool chargeMP = false;
    private bool trigger = false;
    private float timer = 0f;
    private float shield = 0f;
    private bool stunned = false;

    Animator anim;

    protected override void Start()
    {
        anim = GetComponent<Animator>();

        Eyesight = 10f;
        MaxHealth = Health = 10f;
        AttackDamage = 5f;
        MP = 0f;
        MaxMP = 100f;
        Range = 1.5f;    // ���� ����
        MovementSpeed = 1f;
        Interval = 1.0f;
        ConnectValue(Define.ChangableValue.Hp, typeof(Enemy).GetProperty("MaxHealth"), typeof(Enemy).GetProperty("Health"));
        ConnectValue(Define.ChangableValue.Mp, typeof(Enemy).GetProperty("MaxMP"), typeof(Enemy).GetProperty("MP"));
        base.Start();
    }
    private void FixedUpdate()  //FIXME
    {
        timer += Time.deltaTime;
        if (timer >= 1.0f && MP < MaxMP)
        {
            MP += 10f;
            timer = 0f;
            //Debug.Log(MP);
        }

        if (onHit && MP == 0)
        {
            onHit = false;
            stunned = true;
            if(shield >= 0)
            {
                Health -= shield;
                shield = 0;
            }
            StartCoroutine(StunTimer());
        }
        if(stunned) Debug.Log(stunned);
    }

    protected override Queue<IEnumerator> DecideNextRoutine()
    {
        Queue<IEnumerator> nextRoutines = new Queue<IEnumerator>();

        if (!stunned && CheckPlayer())
        {
            if (MP >= MaxMP) // MP�� 100���¿��� �÷��̾�� �����ߴ�
            {
                anim.SetBool("Moving", false);

                nextRoutines.Enqueue(NewActionRoutine(GenerateShield()));
            }
            else
            {
                if (DistToPlayer() < Eyesight)
                {
                    Vector3 dir = GetPlayerPos() - transform.position;
                    float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
                    bool isLookRight = angle < 90 && angle > -90;
                    GetComponent<SpriteRenderer>().flipX = isLookRight;
                    if (DistToPlayer() < Range) // �÷��̾ ���� �����ȿ� ���Դ�
                    {
                        anim.SetTrigger("Attack");
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

        }
        else nextRoutines.Enqueue(NewActionRoutine(WaitRoutine(1f)));

        return nextRoutines;
    }

    private IEnumerator GenerateShield()
    {
        if(shield <= 0)
        {
            shield = MaxHealth * 0.2f;
            Health += shield;
        }
        else
        {
            Health -= shield;
            shield = MaxHealth * 0.2f;
            Health += shield;
        }
        MP = 0;
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

    private IEnumerator StunTimer()
    {
        for(float i=0; i<=2; i += Time.deltaTime)
        {
            yield return null;
        }
        stunned = false;
        yield return null;
    }
}
