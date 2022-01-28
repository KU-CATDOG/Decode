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
        Range = 1.5f;    // ���� ����
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

        //TODO : �÷��̾��� �������� MP�� 0�� �Ǹ� ��ȣ���� �������� 2�ʵ��� ����
        
    }

    protected override Queue<IEnumerator> DecideNextRoutine()
    {
        Queue<IEnumerator> nextRoutines = new Queue<IEnumerator>();

        if (CheckPlayer())
        {
            if (MP >= MaxMP) // MP�� 100���¿��� �÷��̾�� �����ߴ�
            {
                nextRoutines.Enqueue(NewActionRoutine(GenerateShield()));
            }
            else
            {
                if (DistToPlayer() < Range) // �÷��̾ ���� �����ȿ� ���Դ�
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
