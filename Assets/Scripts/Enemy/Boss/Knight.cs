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
    private float attackSpeed;      // ( 공격횟수 / s )
    private float dmgMultiplier = 1.0f;    // 받는 데미지 배율
    private IEnumerator[] attackRoutines;
    private float attackCoefficient; // 스킬마다 적용되는 데미지, 기획나오면 적용
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
    /// 나이트의 체력이 한칸 달았을 때 실행되는 강화 루틴
    /// </summary>
    private IEnumerator SecondPhase()
    {
        MovementSpeed *= SecondPhaseSpeedMultiplier;
        attackSpeed *= SecondPhaseASMultiplier;
        dmgMultiplier = secondPhaseDamageMultiplier;
        yield return new WaitForSeconds(1.0f);
    }
    /// <summary>
    /// 나이트의 체력이 두칸 달았을 때 실행되는 강화 루틴
    /// </summary>
    private IEnumerator LastPhase()
    {
        MovementSpeed = MovementSpeed / SecondPhaseSpeedMultiplier * LastPhaseSpeedMultiplier;
        attackSpeed = attackSpeed / SecondPhaseASMultiplier * LastPhaseASMultiplier;
        dmgMultiplier = LastPhaseDamageMultiplier;
        yield return new WaitForSeconds(1.0f);
    }
    /// <summary>
    /// 1. 전방/후방 공격
    /// 플레이어가 보스 근처에 있을때 플레이어를 바라보고 찌르기
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
    /// 2. 돌진
    /// 랜스를 두손으로 잡고 잠시 후 맵의 양끝 중 현재 위치에서 먼 쪽으로 돌진
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
    /// 3. 랜스 던지기
    ///1. 플레이어를 바라보고 랜스를 던짐(바닥과 평행하게)
    ///2. 랜스는 플레이어나 맵 끝에 맞으면 그 자리에 떨어짐
    ///3. 랜스가 바닥에 떨어지면 보스는 랜스를 줍기 위해 이동
    ///4. 랜스를 주운  후 돌진 시전
    /// </summary>
    private IEnumerator ThrowLance()
    {
        if (CheckPlayer())
        {
            Vector2 throwVec = new Vector2(player.transform.position.x - transform.position.x, 0).normalized;
            rbLance.velocity = throwVec * (5.0f);
            lance.transform.parent = null;
            HaveLance = false;
            while (!HaveLance) // 창을 줍는 로직은 Lance에서 구현
            {
                rb.velocity = throwVec * MovementSpeed;
            }
            yield return Rush();
        }
    }
    /// <summary>
    /// 4. 짧은 돌진
    ///1. 1초동안 멈추었다가(이때는 바라보는 방향 안바뀜) 바라보는 방향으로 짧게 돌진
    ///2. 전방/후방 공격 직후 시전할 수 있음
    /// </summary>
    private IEnumerator ShortRush()
    {
        if (CheckPlayer())
        {
            yield return new WaitForSeconds(1.0f);
            isRushing = true;
            float rushDistance = 5.0f; // 기획에 따라 변경
            float rushTime = 0.5f; // 기획에 따라 변경
            rb.velocity = new Vector2(rushDistance / rushTime, 0);
            yield return new WaitForSeconds(rushTime);
            isRushing = false;
            rb.velocity = Vector2.zero;
        }
    }
}
