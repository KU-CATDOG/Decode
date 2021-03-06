using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Firefly : Enemy
{
    private bool chargeMP = false;
    private bool trigger = false;
    private float timer = 0f;
    private float blastRadius = 3f; // 폭발 반경

    Animator anim;

    protected override void Start()
    {
        anim = GetComponent<Animator>();

        MaxHealth = Health = 1f;
        AttackDamage = 5f;
        MP = 0f;
        MaxMP = 100f;
        Eyesight = 10f;    // 시야 범위
        MovementSpeed = 5f;
        ConnectValue(Define.ChangableValue.Mp, typeof(Enemy).GetProperty("MaxMP"), typeof(Enemy).GetProperty("MP"));
        base.Start();
    }
    private void FixedUpdate()  //FIXME
    {
        if (chargeMP)
        {
            timer += Time.deltaTime;
            if (timer >= 1.0f && MP < MaxMP)
            {
                MP += 20f;
                timer = 0f;
                //Debug.Log(MP);
            }
        }
    }

    protected override Queue<IEnumerator> DecideNextRoutine()
    {
        Queue<IEnumerator> nextRoutines = new Queue<IEnumerator>();

        if (CheckPlayer())
        {
            if (trigger) // MP가 100상태에서 플레이어와 접촉했다
            {
                nextRoutines.Enqueue(NewActionRoutine(BlastRoutine()));
            }
            else
            {
                Vector3 dir = GetPlayerPos() - transform.position;
                float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
                bool isLookRight = angle < 90 && angle > -90;
                GetComponent<SpriteRenderer>().flipX = !isLookRight;

                if (DistToPlayer() < Eyesight) // 플레이어가 시야 범위안에 들어왔다
                {
                    chargeMP = true;
                    nextRoutines.Enqueue(NewActionRoutine(MoveTowardPlayer(MovementSpeed)));
                }
                else
                {
                    chargeMP = false;
                    nextRoutines.Enqueue(NewActionRoutine(WaitRoutine(1f)));

                }
            }

        }
        else nextRoutines.Enqueue(NewActionRoutine(WaitRoutine(1f)));

        return nextRoutines;
    }

    IEnumerator BlastRoutine()
    {
        // particles
        yield return new WaitForSeconds(2f);
        CheckForBlast();
        Debug.Log("Firefly: blast");

        anim.SetBool("Down", true);
        yield return new WaitForSeconds(5f);
        anim.SetBool("Down", false);
        trigger = false;
        MP = 0f;
        // particles
    }

    private void CheckForBlast()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, blastRadius);
        foreach(Collider2D c in colliders)
        {
            if (c.GetComponent<Player>())
            {
                GameManager.Instance.GetDamaged(AttackDamage);
            }
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if(MP >= MaxMP && collision.GetComponent<Player>() != null) 
            trigger = true;
    }


    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, blastRadius);
    }
}
