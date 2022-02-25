using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gear : Boss
{
    [SerializeField]
    private int SecondPhaseHealth; // 톱니가 떨어지는 기준 체력
    [SerializeField]
    private int DoubleSkillMp; // 스킬을 2개 사용하게 되는 기준 마나
    private bool DoubleSkill = false;
    private bool GearDrop = false; // 현재 톱니가 떨어지고 있는지 여부
    public bool OnNormalAttack { get; set; } = false;
    private bool OnRazerAttack = false;
    private bool OnPressAttack = false;
    public bool NormalAttackHit { get; set; } = false;
    private Coroutine GearDropCoroutine; //톱니가 떨어지는 코루틴
    private Rigidbody2D rbLeftArm;
    private Rigidbody2D rbRightArm;
    private float leftArmPos; // x값
    private float rightArmPos;// x값
    private System.Func<IEnumerator>[] AttackRoutines;
    [SerializeField]
    private GameObject PressWarningArea;
    [SerializeField]
    private GameObject RazerWarningArea;
    [SerializeField]
    private GameObject RazerObject;
    [SerializeField]
    private GameObject[] Pressor;
    private Rigidbody2D[] rbPressor;
    [SerializeField]
    private Transform[] Platforms;
    private GameObject fallingGear;
    private new void Awake()
    {
        base.Awake();
        fallingGear = Tool.AssetLoader.LoadPrefab<GameObject>("Enemy/Boss/Gear/FallingGear");
        rbPressor = new Rigidbody2D[4];
        for(int i = 0; i < 4; i++)
        {
            rbPressor[i] = Pressor[i].GetComponent<Rigidbody2D>();
        }
        AttackRoutines = new System.Func<IEnumerator>[3] { NormalAttack, RazerAttack, Press };
        Rigidbody2D[] rb = GetComponentsInChildren<Rigidbody2D>();
        rbLeftArm = rb[0];
        rbRightArm = rb[1];
        leftArmPos = rbLeftArm.position.x;
        rightArmPos = rbRightArm.position.x;
    }
    protected override Queue<IEnumerator> DecideNextRoutine()
    {
        Queue<IEnumerator> ret = new Queue<IEnumerator>();
        if (CheckPlayer())
        {
            if (DoubleSkill)
            {
                ret.Enqueue(NewActionRoutine(DoubleSkillRoutine()));
            }
            else
            {
                ret.Enqueue(NewActionRoutine(AttackRoutines[Random.Range(0, 3)]()));
            }
        }
        else
        {
            ret.Enqueue(NewActionRoutine(WaitRoutine(1.0f)));
        }
        return ret;
    }
    
    private new void Start()
    {
        MaxHealth = Health = 50f;
        MP = 0;
        MaxMP = 100f;
        AttackDamage = 10f;
        MovementSpeed = 5f;
        MaxMovementSpeed = 10f;
        ConnectValue(Define.ChangableValue.Hp, typeof(Enemy).GetProperty("MaxHealth"), typeof(Enemy).GetProperty("Health"));
        ConnectValue(Define.ChangableValue.Mp, typeof(Enemy).GetProperty("MaxMP"), typeof(Enemy).GetProperty("MP"));
        ConnectValue(Define.ChangableValue.Speed, typeof(Enemy).GetProperty("MaxMovementSpeed"), typeof(Enemy).GetProperty("MovementSpeed")); // 편의 상 이동속도를 공격속도로 사용
        base.Start();
        StartCoroutine(MpRestoreRoutine());
    }
    private IEnumerator MpRestoreRoutine()
    {
        while (true)
        {

            MP = Mathf.Min(MP + 3, MaxMP);
            yield return new WaitForSeconds(1.0f);
        }
    }
    /// <summary>
    /// 집게발로 좌→우, 우→좌 한번씩 훑고감 (회피 가능)
    /// </summary>
    /// <returns></returns>
    private IEnumerator NormalAttack()
    {
        OnNormalAttack = true;
        NormalAttackHit = false;
        ///왼쪽
        rbLeftArm.velocity = new Vector2(30, 0);
        yield return new WaitUntil(() =>
        {
            Vector2 viewPos = Camera.main.WorldToViewportPoint(rbLeftArm.position);
            return viewPos.x > 1;
        });
        rbLeftArm.velocity = new Vector2(-20, 0);
        yield return new WaitUntil(() =>
        {
            return rbLeftArm.position.x < leftArmPos;
        });
        rbLeftArm.velocity = Vector2.zero;
        rbLeftArm.position = new Vector2(leftArmPos, rbLeftArm.position.y);
        yield return new WaitForSeconds(3.0f);

        NormalAttackHit = false;
        ///오른쪽
        rbRightArm.velocity = new Vector2(-30, 0);
        yield return new WaitUntil(() =>
        {
            Vector2 viewPos = Camera.main.WorldToViewportPoint(rbRightArm.position);
            return viewPos.x < 0;
        });
        rbRightArm.velocity = new Vector2(20, 0);
        yield return new WaitUntil(() =>
        {
            return rbRightArm.position.x > rightArmPos;
        });
        rbRightArm.velocity = Vector2.zero;
        rbRightArm.position = new Vector2(rightArmPos, rbRightArm.position.y);
        yield return new WaitForSeconds(1.0f);
        OnNormalAttack = false;
    }
    /// <summary>
    /// 측면에서 레이저 발사 (미리 범위 보여줌) 크기는 한 개 층정도
    /// </summary>
    /// <returns></returns>
    private IEnumerator RazerAttack()
    {
        OnRazerAttack = true;
        RazerWarningArea.SetActive(true);
        yield return new WaitForSeconds(1.0f);
        RazerWarningArea.SetActive(false);
        yield return new WaitForSeconds(0.5f);
        RazerObject.SetActive(true);
        yield return new WaitForSeconds(4.0f);
        OnRazerAttack = false;
    }
    /// <summary>
    /// 프레스기로 4개 플랫폼을 하나씩 찍음 (위치 표시)
    /// </summary>
    /// <returns></returns>
    private IEnumerator Press()
    {
        OnPressAttack = true;
        for(int i = 0; i < 4; i++)
        {
            PressWarningArea.transform.position = Platforms[i].position;
            PressWarningArea.SetActive(true);
            yield return new WaitForSeconds(1.0f);
            PressWarningArea.SetActive(false);
            Pressor[i].transform.position = Platforms[i].position + new Vector3(0, 5, 0);
            Pressor[i].SetActive(true);
            rbPressor[i].velocity = new Vector2(0, -10);
            yield return new WaitUntil(() => rbPressor[i].position.y < Platforms[i].position.y);
            rbPressor[i].velocity = Vector2.zero;
            yield return new WaitForSeconds(0.5f);
        }
        for(int i = 0; i < 4; i++)
        {
            Pressor[i].SetActive(false);
        }
        yield return new WaitForSeconds(1.0f);
        OnPressAttack = false;
    }
    /// <summary>
    /// 체력 N 미만 시 상단에서 부터 톱니가 떨어짐
    /// </summary>
    private IEnumerator GearDropRoutine()
    {
        while (true)
        {
            int gearNum = Random.Range(3, 7);
            for(int i = 0; i < gearNum; i++)
            {
                int gearSize = Random.Range(3, 7);
                FallingGear gear = Instantiate(fallingGear).GetComponent<FallingGear>();
                gear.transform.localScale = new Vector2(gearSize, gearSize);
                gear.transform.position = (Vector2)Camera.main.ViewportToWorldPoint(new Vector3(Random.value,1));
            }
            yield return new WaitForSeconds(1f);
        }
    }
    private IEnumerator DoubleSkillRoutine()
    {
        int tmp = Random.Range(0, 3);
        for(int i = 0; i < 3; i++)
        {
            if (tmp != i)
            {
                StartCoroutine(AttackRoutines[i]());
            }
        }
        yield return new WaitUntil(()=> !OnNormalAttack&&!OnPressAttack&&!OnRazerAttack);
    }
    private new void Update()
    {
        base.Update();
        if (Health < SecondPhaseHealth && !GearDrop)
        {
            GearDrop = true;
            GearDropCoroutine = StartCoroutine(GearDropRoutine());
        }
        else if(GearDrop && Health >= SecondPhaseHealth)
        {
            GearDrop = false;
            StopCoroutine(GearDropCoroutine);
        }
        if (MP >= DoubleSkillMp)
        {
            DoubleSkill = true;
        }
        else
        {
            DoubleSkill = false;
        }
    }

}
