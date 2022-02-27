using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShadowWizard : Boss
{

    private System.Func<IEnumerator>[] AttackRoutines; // ���� ��ƾ��
    private int switchingCount = 0; // ���� ������ Ƚ��
    [SerializeField]
    private int maxSwitchingCount; // �����ϰԵǴ� ���� ���� Ƚ��
    [SerializeField]
    private int MpForFanRoutine; // FanRoutine�� ����Ǳ� ���� �ʿ��� mp
    private bool onStealth = false;
    private bool onTeleport = false;
    private SpriteRenderer sprite;
    private BoxCollider2D col;
    [SerializeField]
    private GameObject risingBladeWarningField;
    private GameObject blade;
    private GameObject risingBladePrefab;
    private GameObject shootingBladePrefab;
    private float time; // �÷��̾ ���� �Ÿ� ���� �����־��� �ð�
    private Animator animator;
    protected override Queue<IEnumerator> DecideNextRoutine()
    {
        Queue<IEnumerator> ret = new Queue<IEnumerator>();
        if (CheckPlayer())
        {
            if (MP>=MpForFanRoutine)
            {
                ret.Enqueue(NewActionRoutine(FanOfBlade()));
            }
            else if (time>5f)
            {
                time = 0;
                ret.Enqueue(NewActionRoutine(Teleport()));
            }
            else
            {
                ret.Enqueue(NewActionRoutine(AttackRoutines[Random.Range(0, 2)]()));
            }
        }
        else
        {
            ret.Enqueue(NewActionRoutine(WaitRoutine(1.0f)));
        }
        return ret;
    }
    /// <summary>
    /// 1. ������ ������� HP/MP ����Ī N�� �� �����ؼ� N�ʰ� ��������Ұ�
    /// </summary>
    public override void SelectValuetoChange()
    {
        base.SelectValuetoChange();
        if(maxSwitchingCount == ++switchingCount)
        {
            switchingCount = 0;
            StartCoroutine(Stealth());
        }
    }
    private new void Awake()
    {
        base.Awake();
        blade = Tool.AssetLoader.LoadPrefab<GameObject>("Enemy/Boss/ShadowWizard/Blade");
        shootingBladePrefab = Tool.AssetLoader.LoadPrefab<GameObject>("Enemy/Boss/ShadowWizard/ShootingBlade");
        risingBladePrefab = Tool.AssetLoader.LoadPrefab<GameObject>("Enemy/Boss/ShadowWizard/RisingBlade");
        animator = GetComponent<Animator>();
        col = GetComponent<BoxCollider2D>();
        sprite = GetComponent<SpriteRenderer>();
        AttackRoutines = new System.Func<IEnumerator>[2] { RisingBlade, ShootingBlade };
    }
    protected override void Start()
    {
        MaxHealth = Health = 50f;
        AttackDamage = 10f;
        MaxMovementSpeed = 10f;
        MaxMP = 100;
        MP = 0;
        Range = 3.0f;
        ConnectValue(Define.ChangableValue.Hp, typeof(Enemy).GetProperty("MaxHealth"), typeof(Enemy).GetProperty("Health"));
        ConnectValue(Define.ChangableValue.Mp, typeof(Enemy).GetProperty("MaxMP"), typeof(Enemy).GetProperty("MP"));
        base.Start();
        StartCoroutine(MPRestoreRoutine()); 
    }
    private new void Update()
    {
        base.Update();
        if(onStealth == true)
        {
            sprite.color = new Color(sprite.color.r, sprite.color.g, sprite.color.b, 0.5f);
        }
        else
        {
            sprite.color = new Color(sprite.color.r, sprite.color.g, sprite.color.b, 1f);
        }
        LookAt(player.transform.position);
        if (DistToPlayer() < 7)
        {
            time += Time.deltaTime;
        }
        else
        {
            time = 0;
        }
    }
    private IEnumerator MPRestoreRoutine()
    {
        while (gameObject)
        {
            MP = Mathf.Min(MP + 5, MaxMP);
            yield return new WaitForSeconds(1.0f);
        }
    }
    /// <summary>
    /// 1. �÷��̾� �� �ؿ������� �׸��� Į���� ��Ƴ� (�ö���� ���� ��ġ ǥ��)
    /// </summary>
    /// <returns></returns>
    private IEnumerator RisingBlade()
    {
        risingBladeWarningField.SetActive(true);
        animator.SetTrigger("RisingBlade");
        yield return new WaitUntil(() => { risingBladeWarningField.transform.position = player.transform.position; return animator.IsInTransition(0); });
        yield return new WaitUntil(() => { risingBladeWarningField.transform.position = player.transform.position; return !animator.IsInTransition(0); });
        yield return new WaitForSeconds(0.3f);
        risingBladeWarningField.SetActive(false);
        GameObject risingBlade = Instantiate(risingBladePrefab);
        risingBlade.transform.position = risingBladeWarningField.transform.position + Vector3.down * 5;
        risingBlade.GetComponent<Rigidbody2D>().velocity = new Vector2(0, 20);
        StartCoroutine(DestroyBlade(risingBlade));
        animator.SetTrigger("EndAction");
        yield return new WaitUntil(() => animator.IsInTransition(0));
        yield return new WaitUntil(() => !animator.IsInTransition(0));
        risingBlade.SetActive(false);
    }
    /// <summary>
    /// ���� ���� �׸��� Į���� �߻�
    /// </summary>
    private IEnumerator ShootingBlade()
    {
        animator.SetTrigger("ShootingBlade");
        yield return new WaitUntil(() => animator.IsInTransition(0));
        yield return new WaitUntil(() => !animator.IsInTransition(0));
        yield return new WaitForSeconds(0.5f);
        GameObject shootingBlade = Instantiate(shootingBladePrefab);
        shootingBlade.transform.position = transform.position + transform.right;
        shootingBlade.transform.rotation = transform.rotation;
        shootingBlade.GetComponent<Rigidbody2D>().velocity = transform.right * 20;
        animator.SetTrigger("EndAction");
        yield return new WaitUntil(() => animator.IsInTransition(0));
        yield return new WaitUntil(() => !animator.IsInTransition(0));
        DestroyBlade(shootingBlade);

    }
    
    /// <summary>
    /// 1. ������ ������� HP/MP ����Ī N�� �� �����ؼ� N�ʰ� ��������Ұ�
    /// </summary>
    private IEnumerator Stealth()
    {
        onStealth = true;
        col.enabled = false;
        yield return new WaitForSeconds(5f);
        col.enabled = true;
        onStealth = false;
    }
    /// <summary>
    /// N �� ���� ������ ���ſ��� �������(�����) �׸��� Į���� �߻� (������� ȸ�ǰ���)
    /// </summary>
    /// <returns></returns>
    private IEnumerator FanOfBlade()
    {
        animator.SetTrigger("FanOfBladeReady");
        yield return new WaitUntil(() => animator.IsInTransition(0));
        yield return new WaitUntil(() => !animator.IsInTransition(0));
        yield return new WaitForSeconds(3.0f);
        animator.SetTrigger("FanOfBlade");
        yield return new WaitUntil(() => animator.IsInTransition(0));
        yield return new WaitUntil(() => !animator.IsInTransition(0));
        int degree = 0;
        bool first = true;
        while (MP>=MpForFanRoutine || first)
        {
            Vector2 movVec;
            if (degree < 90 || degree>=270)
            {
                movVec = new Vector2(1, Mathf.Tan(degree / 180f * Mathf.PI)).normalized;
            }
            else
            {
                movVec = new Vector2(-1, -Mathf.Tan(degree / 180f * Mathf.PI)).normalized;
            }
            GameObject bladeForFan = Instantiate(blade);
            bladeForFan.transform.rotation = Quaternion.Euler(new Vector3(0, 0, degree));
            Debug.Log(movVec);
            bladeForFan.transform.position = (Vector2)transform.position + movVec;
            bladeForFan.GetComponent<Rigidbody2D>().velocity = movVec * 10;
            StartCoroutine(DestroyBlade(bladeForFan));
            degree += 10;
            if(degree == 360)
            {
                first = false;
            }
            degree %= 360;
            yield return new WaitForSeconds(0.05f);
        }
        animator.SetTrigger("FanOfBladeEnd");
        yield return new WaitUntil(() => animator.IsInTransition(0));
        yield return new WaitUntil(() => !animator.IsInTransition(0));
        animator.SetTrigger("EndAction");
        yield return new WaitUntil(() => animator.IsInTransition(0));
        yield return new WaitUntil(() => !animator.IsInTransition(0));
    }
    private IEnumerator DestroyBlade(GameObject obj)
    {
        yield return new WaitForSeconds(2.0f);
        Destroy(obj);
    }
    private IEnumerator Teleport()
    {
        RaycastHit2D raycasthitLeft = Physics2D.Raycast(player.transform.position, Vector2.left, 15, LayerMask.GetMask("Wall") | LayerMask.GetMask("Floor"));
        RaycastHit2D raycasthitRight = Physics2D.Raycast(player.transform.position, Vector2.right, 15, LayerMask.GetMask("Wall") | LayerMask.GetMask("Floor"));
        animator.SetTrigger("Teleport");
        yield return new WaitUntil(() => animator.IsInTransition(0));
        yield return new WaitUntil(() => !animator.IsInTransition(0));
        yield return new WaitForSeconds(2.0f);
        if (!raycasthitLeft && !raycasthitRight)
        {
            int rand = Random.Range(0, 2);
            if(rand == 0)
            {
                transform.position = new Vector3(player.transform.position.x + 15, transform.position.y);
            }
            else
            {
                transform.position = new Vector3(player.transform.position.x - 15, transform.position.y);
            }
        }
        else if (!raycasthitLeft)
        {
            transform.position = new Vector3(player.transform.position.x - 15, transform.position.y);
        }
        else if (!raycasthitRight)
        {
            transform.position = new Vector3(player.transform.position.x + 15, transform.position.y);
        }
        else
        {
            if (raycasthitLeft.distance < raycasthitRight.distance)
            {
                transform.position = new Vector3(player.transform.position.x + raycasthitRight.distance - 1, transform.position.y);

            }
            else
            {
                transform.position = new Vector3(player.transform.position.x + raycasthitRight.distance - 1, transform.position.y);
            }
        }
        animator.SetTrigger("TeleportEnd");
        yield return new WaitUntil(() => animator.IsInTransition(0));
        yield return new WaitUntil(() => !animator.IsInTransition(0));
        animator.SetTrigger("EndAction");
        yield return new WaitUntil(() => animator.IsInTransition(0));
        yield return new WaitUntil(() => !animator.IsInTransition(0));
        yield break;
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

}
