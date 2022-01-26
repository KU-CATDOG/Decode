using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Firefly : Enemy
{
    private bool chargeMP = false;
    private bool trigger = false;
    private float timer = 0f;
    private float blastRadius = 3f; // ���� �ݰ�

    protected override void Start()
    {
        base.Start();
        MaxHealth = Health = 1f;
        AttackDamage = 5f;
        MP = 0f;
        MaxMP = 100f;
        Range = 10f;    // �þ� ����
        MovementSpeed = 1f;
    }
    private void FixedUpdate()  //FIXME
    {
        if (chargeMP)
        {
            timer += Time.deltaTime;
            if (timer >= 1.0f && MP < MaxMP)
            {
                MP += 10f;
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
            if (trigger) // MP�� 100���¿��� �÷��̾�� �����ߴ�
            {
                nextRoutines.Enqueue(NewActionRoutine(BlastRoutine()));
            }
            else
            {
                if (DistToPlayer() < Range) // �÷��̾ �þ� �����ȿ� ���Դ�
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
        yield return new WaitForSeconds(5f);
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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(MP >= 100f && collision.GetComponent<Player>() != null) 
            trigger = true;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, blastRadius);
    }
}
