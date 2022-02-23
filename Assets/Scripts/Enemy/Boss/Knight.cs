using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Knight : Boss
{
    #region PhasePatternVar
    private bool onFirstPhase = true;
    private bool onSecondPhase = false;
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
    private System.Func<IEnumerator>[] ShortRangeAttackRoutines;
    private System.Func<IEnumerator>[] LongRangeAttackRoutines;
    private float attackCoefficient; // ��ų���� ����Ǵ� ������, ��ȹ������ ����
    private KnightLance lance;
    private Rigidbody2D rbLance;
    private bool isRushing = false;
    private bool isLongRange = true;
    private float longRange;
    public Animator animator { get; private set; }
    public bool HaveLance { get; set; }
    protected override void Start()
    {
        animator = GetComponent<Animator>();
        ShortRangeAttackRoutines = new System.Func<IEnumerator>[2] { ShortRush, NormalAttack };
        LongRangeAttackRoutines = new System.Func<IEnumerator>[2] { Rush, ThrowLance };
        attackSpeed = 0.5f;
        MaxHealth = Health = 50f * 3;
        AttackDamage = 10f;
        MovementSpeed = 5f;
        MaxMovementSpeed = 10f;
        Range = 3.0f;
        longRange = 12.0f;
        lance = GetComponentInChildren<KnightLance>();
        rbLance = lance.GetComponent<Rigidbody2D>();
        ConnectValue(Define.ChangableValue.Hp, typeof(Enemy).GetProperty("MaxHealth"), typeof(Enemy).GetProperty("Health"));
        base.Start();
        lance.gameObject.SetActive(false);
    }
    protected override Queue<IEnumerator> DecideNextRoutine()
    {
        Queue<IEnumerator> ret = new Queue<IEnumerator>();
        if (CheckPlayer())
        {
            if (!isLongRange && DistToPlayer() < Range)
            {
                ret.Enqueue(NewActionRoutine(ShortRangeAttackRoutines[Random.Range(0,2)]()));
                isLongRange = Random.Range(0, 2) == 0;
            }
            else if ( isLongRange && DistToPlayer() < longRange)
            {
                ret.Enqueue(NewActionRoutine(LongRangeAttackRoutines[Random.Range(0,2)]()));
                isLongRange = Random.Range(0, 2) == 0;
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
        if(collision.tag == "Floor")
        {
            rb.gravityScale = 0;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.tag == "Floor")
        {
            rb.gravityScale = 1f;
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
    private new void Update()
    {
        base.Update();
        if (Health <= MaxHealth * 2 / 3f && onFirstPhase)
        {
            StartCoroutine(SecondPhase());
            onFirstPhase = false;
            onSecondPhase = true;
        }
        if(Health<=MaxHealth/3f && onSecondPhase)
        {
            StartCoroutine(LastPhase());
            onSecondPhase = false;
        }
    }
    /// <summary>
    /// 1. ����/�Ĺ� ����
    /// �÷��̾ ���� ��ó�� ������ �÷��̾ �ٶ󺸰� ���
    /// </summary>
    private IEnumerator NormalAttack()
    {
        if (CheckPlayer())
        {
            LookAt(player.transform.position);
            animator.SetTrigger("Stabbing");
            yield return new WaitForSeconds(0.5f);
            animator.SetTrigger("EndAction");
            yield return new WaitUntil(() => animator.IsInTransition(0));
            GameManager.Instance.GetDamaged(AttackDamage);
            if(Random.value < 0.3f)
            {
                yield return ShortRush();
            }
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
            Debug.Log("RUSH");
            RaycastHit2D left;
            RaycastHit2D right;
            LayerMask wall = LayerMask.GetMask("Wall");
            left = Physics2D.Raycast(transform.position, new Vector2(-1, 0), Mathf.Infinity, wall);
            right = Physics2D.Raycast(transform.position, new Vector2(1, 0), Mathf.Infinity, wall);
            Vector2 rushVec = left.distance > right.distance ? new Vector2(-left.distance,0) : new Vector2(right.distance,0);
            LookDir(rushVec);
            animator.SetTrigger("DashReady");
            yield return new WaitForSeconds(2.0f);
            animator.SetTrigger("Dashing");
            rb.velocity = rushVec;
            isRushing = true;
            yield return new WaitForSeconds(0.9f);
            isRushing = false;
            rb.velocity = Vector2.zero;
            animator.SetTrigger("EndAction");
            yield return new WaitUntil(() => animator.IsInTransition(0));
            LookAt(player.transform.position);
            yield return new WaitForSeconds(1.0f);
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
            LookAt(player.transform.position);
            animator.SetTrigger("ReadyThrow");
            yield return new WaitForSeconds(1f);
            animator.SetTrigger("Throwing");
            yield return new WaitUntil(() => animator.IsInTransition(0));
            yield return new WaitUntil(() => !animator.IsInTransition(0));
            Vector2 throwVec = new Vector2(player.transform.position.x - transform.position.x, 0).normalized;
            lance.gameObject.SetActive(true);
            lance.transform.localPosition = Vector2.down;
            rbLance.velocity = throwVec * (50.0f);
            lance.transform.parent = null;
            HaveLance = false;
            animator.SetBool("HaveWeapon", false);
            animator.SetTrigger("Walking");
            yield return new WaitUntil(() => animator.IsInTransition(0));
            while (!HaveLance) // â�� �ݴ� ������ Lance���� ����
            {
                yield return null;
                rb.velocity = throwVec * MovementSpeed;
            }
            animator.SetTrigger("EndAction");
            yield return new WaitUntil(() => animator.IsInTransition(0));
            rb.velocity = Vector2.zero;
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
            LookAt(player.transform.position);
            animator.SetTrigger("DashReady");
            yield return new WaitForSeconds(1.0f);
            animator.SetTrigger("FastDash");
            isRushing = true;
            float rushDistance = 5.0f; // ��ȹ�� ���� ����
            float rushTime = 0.5f; // ��ȹ�� ���� ����
            rb.velocity = new Vector2(transform.right.x * rushDistance / rushTime, 0);
            yield return new WaitForSeconds(rushTime);
            animator.SetTrigger("EndAction");
            yield return new WaitUntil(() => animator.IsInTransition(0));
            isRushing = false;
            rb.velocity = Vector2.zero;
            LookAt(player.transform.position);
            yield return new WaitForSeconds(1.0f);
        }
    }

    private void LookAt(Vector2 dir)
    {
        Vector2 lookVec = dir - (Vector2)transform.position;
        LookDir(lookVec);
    }
    private void LookDir(Vector2 dir)
    {
        transform.rotation = Quaternion.Euler(Vector3.up * (90f + -90f * dir.x / Mathf.Abs(dir.x)));
    }

    protected new IEnumerator MoveTowardPlayer(float speedMultiplier)     // �÷��̾ ���� �����δ�
    {
        animator.SetTrigger("Walking");
        LookAt(player.transform.position);
        Vector2 direction = Vector2.right * (GetPlayerPos().x < GetObjectPos().x ? -1 : 1);
        rb.velocity = direction * MovementSpeed;
        yield return new WaitForSeconds(0.2f);
        yield return new WaitUntil(() => (!isLongRange && DistToPlayer() < Range || isLongRange && DistToPlayer() < longRange));
        animator.SetTrigger("EndAction");
        yield return new WaitUntil(() => animator.IsInTransition(0));
        rb.velocity = Vector2.zero;
    }
}
