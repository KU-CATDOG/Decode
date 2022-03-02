using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gear : Boss
{
    [SerializeField]
    private int SecondPhaseHealth; // ��ϰ� �������� ���� ü��
    [SerializeField]
    private int DoubleSkillMp; // ��ų�� 2�� ����ϰ� �Ǵ� ���� ����
    [SerializeField]
    private Animator spring;
    private Animator leftArm;
    private Animator rightArm;
    private Animator body;
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
    private GameObject[] Presser;
    private Rigidbody2D[] rbPresserMid;
    private GameObject[] PresserMid;
    private Rigidbody2D[] rbPresserBottom;
    private GameObject[] PresserBottom;
    [SerializeField]
    private Transform[] Platforms;
    private GameObject fallingGear;
    private new void Awake()
    {
        base.Awake();
        fallingGear = Tool.AssetLoader.LoadPrefab<GameObject>("Enemy/Boss/Gear/FallingGear");
        PresserMid = new GameObject[4];
        PresserBottom = new GameObject[4];
        rbPresserMid = new Rigidbody2D[4];
        rbPresserBottom = new Rigidbody2D[4];
        for (int i = 0; i < 4; i++)
        {
            PresserMid[i] = Presser[i].transform.GetChild(1).gameObject;
            rbPresserMid[i] = PresserMid[i].GetComponent<Rigidbody2D>();
            PresserBottom[i] = Presser[i].transform.GetChild(2).gameObject;
            rbPresserBottom[i] = PresserBottom[i].GetComponent<Rigidbody2D>();
            Presser[i].transform.position = Platforms[i].position + new Vector3(0, 5, 0);
        }
        AttackRoutines = new System.Func<IEnumerator>[3] { NormalAttack, RazerAttack, Press };
        Rigidbody2D[] rb = GetComponentsInChildren<Rigidbody2D>();
        rbLeftArm = rb[0];
        rbRightArm = rb[1];
        leftArmPos = rbLeftArm.position.x;
        rightArmPos = rbRightArm.position.x;
        body = GetComponent<Animator>();
        leftArm = rbLeftArm.GetComponent<Animator>();
        rightArm = rbRightArm.GetComponent<Animator>();
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
        MaxHealth = Health = 100f;
        MP = 0;
        MaxMP = 100f;
        AttackDamage = 10f;
        MovementSpeed = 0f;
        MaxMovementSpeed = 100f;
        ConnectValue(Define.ChangableValue.Hp, typeof(Enemy).GetProperty("MaxHealth"), typeof(Enemy).GetProperty("Health"));
        ConnectValue(Define.ChangableValue.Mp, typeof(Enemy).GetProperty("MaxMP"), typeof(Enemy).GetProperty("MP"));
        //�ӵ��� 100�� �� 3�谡 �ǵ��� �����Ѵ�.
        ConnectValue(Define.ChangableValue.Speed, typeof(Enemy).GetProperty("MaxMovementSpeed"), typeof(Enemy).GetProperty("MovementSpeed")); // ���� �� �̵��ӵ��� ���ݼӵ��� ���
        
        base.Start();
        StartCoroutine(MpRestoreRoutine());
    }
    /// <summary>
    /// Hp�� ���� �� �׸�ŭ �ӵ��� ����
    /// </summary>
    protected override float AddValue(float value)
    {
        if(GetCurSelected() == Define.ChangableValue.Hp)
        {
            MovementSpeed = Mathf.Min(MaxMovementSpeed, MovementSpeed - value);
        }
        return base.AddValue(value);
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
        rbLeftArm.velocity = new Vector2(30, 0) * (1+MovementSpeed/50f);
        yield return new WaitUntil(() =>
        {
            Vector2 viewPos = Camera.main.WorldToViewportPoint(rbLeftArm.position);
            return viewPos.x > 1;
        });
        rbLeftArm.velocity = new Vector2(-20, 0) * (1 + MovementSpeed / 50f);
        yield return new WaitUntil(() =>
        {
            return rbLeftArm.position.x < leftArmPos;
        });
        rbLeftArm.velocity = Vector2.zero;
        rbLeftArm.position = new Vector2(leftArmPos, rbLeftArm.position.y);
        yield return new WaitForSeconds(3.0f / (1 + MovementSpeed / 50f));

        NormalAttackHit = false;
        ///������
        rbRightArm.velocity = new Vector2(-30, 0) * (1 + MovementSpeed / 50f);
        yield return new WaitUntil(() =>
        {
            Vector2 viewPos = Camera.main.WorldToViewportPoint(rbRightArm.position);
            return viewPos.x < 0;
        });
        rbRightArm.velocity = new Vector2(20, 0) * (1 + MovementSpeed / 50f);
        yield return new WaitUntil(() =>
        {
            return rbRightArm.position.x > rightArmPos;
        });
        rbRightArm.velocity = Vector2.zero;
        rbRightArm.position = new Vector2(rightArmPos, rbRightArm.position.y);
        yield return new WaitForSeconds(1.0f / (1 + MovementSpeed / 50f));
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
        yield return new WaitForSeconds(1.0f / (1 + MovementSpeed / 50f));
        RazerWarningArea.SetActive(false);
        yield return new WaitForSeconds(0.5f / (1 + MovementSpeed / 50f));
        RazerObject.SetActive(true);
        yield return new WaitForSeconds(4.0f / (1 + MovementSpeed / 50f));
        OnRazerAttack = false;
    }
    /// <summary>
    /// ��������� 4�� �÷����� �ϳ��� ���� (��ġ ǥ��)
    /// </summary>
    /// <returns></returns>
    private IEnumerator Press()
    {
        OnPressAttack = true;
        bool[] pressed = new bool[4] { false, false, false, false };
        List<int> pressList = new List<int>();
        for(int j = 0; j < 4; j++)
        {
            int i = Random.Range(0,4);
            while (pressed[i])
            {
               i = Random.Range(0, 4);
            }
            pressList.Add(i);
            pressed[i] = true;
            PressWarningArea.transform.position = Platforms[i].position;
            PressWarningArea.SetActive(true);
            yield return new WaitForSeconds(1.0f / (1 + MovementSpeed / 50f));
            PressWarningArea.SetActive(false);
            rbPresserMid[i].velocity = new Vector2(0, -5) * (1 + MovementSpeed / 50f);
            rbPresserBottom[i].velocity = new Vector2(0, -10) * (1 + MovementSpeed / 50f);
            yield return new WaitUntil(() => 
            {
                PresserMid[i].transform.localScale = new Vector2(1,PresserMid[i].transform.localPosition.y * -4+1);
                return rbPresserBottom[i].position.y < Platforms[i].position.y;
            });
            rbPresserMid[i].velocity = Vector2.zero;
            rbPresserBottom[i].velocity = Vector2.zero;
            yield return new WaitForSeconds(0.5f / (1 + MovementSpeed / 50f));
        }
        for(int j = 0; j < 4; j++)
        {
            int i = pressList[j];
            rbPresserMid[i].velocity = new Vector2(0, 5) * (1 + MovementSpeed / 50f);
            rbPresserBottom[i].velocity = new Vector2(0, 10) * (1 + MovementSpeed / 50f);
            yield return new WaitUntil(() =>
            {
                PresserMid[i].transform.localScale = new Vector2(1, PresserMid[i].transform.localPosition.y * -4+1);
                return PresserMid[i].transform.localPosition.y >= 0;
            });
            rbPresserMid[i].velocity = Vector2.zero;
            rbPresserBottom[i].velocity = Vector2.zero;
            PresserMid[i].transform.localPosition = Vector2.zero;
            PresserBottom[i].transform.localPosition = Vector2.down * 1 / 3;
        }
        yield return new WaitForSeconds(1.0f / (1 + MovementSpeed / 50f));
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
                gear.GetComponent<Rigidbody2D>().gravityScale = (1 + MovementSpeed / 50f);
            }
            yield return new WaitForSeconds(1f / (1 + MovementSpeed / 50f));
        }
    }
    private IEnumerator DoubleSkillRoutine()
    {
        int tmp = Random.Range(0, 3);
        for(int i = 0; i < 3; i++)
        {
            Debug.Log(tmp);
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
        body.speed = leftArm.speed = rightArm.speed = spring.speed = (1 + MovementSpeed / 50f);
    }

}
