using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gear : Boss
{
    [SerializeField]
    private int SecondPhaseHealth; // ��ϰ� �������� ���� ü��
    [SerializeField]
    private int DoubleSkillMp; // ��ų�� 2�� ����ϰ� �Ǵ� ���� ����
    private bool DoubleSkill = false;
    private bool GearDrop = false; // ���� ��ϰ� �������� �ִ��� ����
    public bool OnNormalAttack { get; set; } = false;
    private bool OnRazerAttack = false;
    private bool OnPressAttack = false;
    public bool NormalAttackHit { get; set; } = false;
    private Coroutine GearDropCoroutine; //��ϰ� �������� �ڷ�ƾ
    private Rigidbody2D rbLeftArm;
    private Rigidbody2D rbRightArm;
    private float leftArmPos; // x��
    private float rightArmPos;// x��
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
        ConnectValue(Define.ChangableValue.Speed, typeof(Enemy).GetProperty("MaxMovementSpeed"), typeof(Enemy).GetProperty("MovementSpeed")); // ���� �� �̵��ӵ��� ���ݼӵ��� ���
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
    /// ���Թ߷� �¡��, ����� �ѹ��� �Ȱ� (ȸ�� ����)
    /// </summary>
    /// <returns></returns>
    private IEnumerator NormalAttack()
    {
        OnNormalAttack = true;
        NormalAttackHit = false;
        ///����
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
        ///������
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
    /// ���鿡�� ������ �߻� (�̸� ���� ������) ũ��� �� �� ������
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
    /// ��������� 4�� �÷����� �ϳ��� ���� (��ġ ǥ��)
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
    /// ü�� N �̸� �� ��ܿ��� ���� ��ϰ� ������
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
