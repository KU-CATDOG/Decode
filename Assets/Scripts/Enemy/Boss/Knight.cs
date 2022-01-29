using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Knight : Enemy
{
    #region PhasePatternVar
    [SerializeField]
    private float secondPhaseSpeedMultiplier;
    public float SecondPhaseSpeedMultiplier
    {
        get
        {
            return secondPhaseSpeedMultiplier;
        }
        set
        {
            secondPhaseSpeedMultiplier = value;
        }
    }
    [SerializeField]
    private float secondPhaseASMultiplier;
    public float SecondPhaseASMultiplier
    {
        get
        {
            return secondPhaseASMultiplier;
        }
        set
        {
            secondPhaseASMultiplier = value;
        }
    }
    [SerializeField]
    private float secondPhaseDamageMultiplier;
    public float SecondPhaseDamageMultiplier
    {
        get
        {
            return secondPhaseDamageMultiplier;
        }
        set
        {
            secondPhaseDamageMultiplier = value;
        }
    }
    [SerializeField]
    private float lastPhaseSpeedMultiplier;
    public float LastPhaseSpeedMultiplier
    {
        get
        {
            return lastPhaseSpeedMultiplier;
        }
        set
        {
            lastPhaseSpeedMultiplier = value;
        }
    }
    [SerializeField]
    private float lastPhaseASMultiplier;
    public float LastPhaseASMultiplier
    {
        get
        {
            return lastPhaseASMultiplier;
        }
        set
        {
            lastPhaseASMultiplier = value;
        }
    }
    [SerializeField]
    private float lastPhaseDamageMultiplier;
    public float LastPhaseDamageMultiplier
    {
        get
        {
            return lastPhaseDamageMultiplier;
        }
        set
        {
            lastPhaseDamageMultiplier = value;
        }
    }
    #endregion
    private float attackSpeed;      // ( ����Ƚ�� / s )
    private float dmgMultiplier = 1.0f;    // �޴� ������ ����
    private IEnumerator[] attackRoutines;
    private float attackCoefficient; // ��ų���� ����Ǵ� ������, ��ȹ������ ����
    private KnightLance lance;
    private Rigidbody2D rbLance;
    private bool isRushing = false;
    public bool HaveLance { get; set; }
    protected override void Start()
    {
        attackRoutines = new IEnumerator[4] { NormalAttack(), Rush(), ThrowLance(), ShortRush() };
        attackSpeed = 0.5f;
        MaxHealth = Health = 50f * 3;
        AttackDamage = 10f;
        MovementSpeed = 1f;
        Range = 3.0f;
        lance = GetComponentInChildren<KnightLance>();
        rbLance = lance.GetComponent<Rigidbody2D>();
        base.Start();
    }
    protected override Queue<IEnumerator> DecideNextRoutine()
    {
        Queue<IEnumerator> ret = new Queue<IEnumerator>();
        if (CheckPlayer())
        {
            if (DistToPlayer() < Range)
            {
                ret.Enqueue(NewActionRoutine(attackRoutines[Random.Range(0, 4)]));
            }
            else
            {
                ret.Enqueue(NewActionRoutine(MoveTowardPlayer(MovementSpeed)));
            }
        }
        else
        {
            ret.Enqueue(NewActionRoutine(WaitRoutine(1.0f)));
        }
        return ret;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isRushing && collision.tag == "Player")
        {
            GameManager.Instance.GetDamaged(10f);
        }
    }

    /// <summary>
    /// ����Ʈ�� ü���� ��ĭ �޾��� �� ����Ǵ� ��ȭ ��ƾ
    /// </summary>
    private IEnumerator SecondPhase()
    {
        MovementSpeed *= SecondPhaseSpeedMultiplier;
        attackSpeed *= SecondPhaseASMultiplier;
        dmgMultiplier = secondPhaseDamageMultiplier;
        yield return new WaitForSeconds(1.0f);
    }
    /// <summary>
    /// ����Ʈ�� ü���� ��ĭ �޾��� �� ����Ǵ� ��ȭ ��ƾ
    /// </summary>
    private IEnumerator LastPhase()
    {
        MovementSpeed = MovementSpeed / SecondPhaseSpeedMultiplier * LastPhaseSpeedMultiplier;
        attackSpeed = attackSpeed / SecondPhaseASMultiplier * LastPhaseASMultiplier;
        dmgMultiplier = LastPhaseDamageMultiplier;
        yield return new WaitForSeconds(1.0f);
    }
    /// <summary>
    /// 1. ����/�Ĺ� ����
    /// �÷��̾ ���� ��ó�� ������ �÷��̾ �ٶ󺸰� ���
    /// </summary>
    private IEnumerator NormalAttack()
    {
        if (CheckPlayer())
        {
            transform.LookAt(player.transform);
            Debug.Log("Normal");
            GameManager.Instance.GetDamaged(AttackDamage);
            if(Random.value < 0.3f)
            {
                yield return ShortRush();
            }
            yield return new WaitForSeconds(0.5f);
        }
    }
    /// <summary>
    /// 2. ����
    /// ������ �μ����� ��� ��� �� ���� �糡 �� ���� ��ġ���� �� ������ ����
    /// </summary>
    private IEnumerator Rush()
    {
        if (CheckPlayer())
        {
            yield return new WaitForSeconds(1.0f);
            RaycastHit2D left;
            RaycastHit2D right;
            LayerMask wall = LayerMask.GetMask("Wall");
            left = Physics2D.Raycast(transform.position, new Vector2(-1, 0), Mathf.Infinity, wall);
            right = Physics2D.Raycast(transform.position, new Vector2(1, 0), Mathf.Infinity, wall);
            Vector2 rushVec = left.distance > right.distance ? new Vector2(-left.distance,0) : new Vector2(right.distance,0);
            rb.velocity = rushVec;
            isRushing = true;
            yield return new WaitForSeconds(0.9f);
            isRushing = false;
            rb.velocity = Vector2.zero;
        }
    }
    /// <summary>
    /// 3. ���� ������
    ///1. �÷��̾ �ٶ󺸰� ������ ����(�ٴڰ� �����ϰ�)
    ///2. ������ �÷��̾ �� ���� ������ �� �ڸ��� ������
    ///3. ������ �ٴڿ� �������� ������ ������ �ݱ� ���� �̵�
    ///4. ������ �ֿ�  �� ���� ����
    /// </summary>
    private IEnumerator ThrowLance()
    {
        if (CheckPlayer())
        {
            Vector2 throwVec = new Vector2(player.transform.position.x - transform.position.x, 0).normalized;
            rbLance.velocity = throwVec * (5.0f);
            lance.transform.parent = null;
            HaveLance = false;
            while (!HaveLance) // â�� �ݴ� ������ Lance���� ����
            {
                rb.velocity = throwVec * MovementSpeed;
            }
            yield return Rush();
        }
    }
    /// <summary>
    /// 4. ª�� ����
    ///1. 1�ʵ��� ���߾��ٰ�(�̶��� �ٶ󺸴� ���� �ȹٲ�) �ٶ󺸴� �������� ª�� ����
    ///2. ����/�Ĺ� ���� ���� ������ �� ����
    /// </summary>
    private IEnumerator ShortRush()
    {
        if (CheckPlayer())
        {
            yield return new WaitForSeconds(1.0f);
            isRushing = true;
            float rushDistance = 5.0f; // ��ȹ�� ���� ����
            float rushTime = 0.5f; // ��ȹ�� ���� ����
            rb.velocity = new Vector2(rushDistance / rushTime, 0);
            yield return new WaitForSeconds(rushTime);
            isRushing = false;
            rb.velocity = Vector2.zero;
        }
    }
}
