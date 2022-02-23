using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShadowWizard : Boss
{

    private System.Func<IEnumerator>[] AttackRoutines; // ���� ��ƾ��
    private int switchingCount = 0; // ���� ������ Ƚ��
    [SerializeField]
    private int maxSwitchingCount; // �����ϰԵǴ� ���� ���� Ƚ��
    protected override Queue<IEnumerator> DecideNextRoutine()
    {
        Queue<IEnumerator> ret = new Queue<IEnumerator>();
        if (CheckPlayer())
        {
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
    protected override void Start()
    {
        MaxHealth = Health = 50f;
        AttackDamage = 10f;
        MovementSpeed = 5f;
        MaxMovementSpeed = 10f;
        Range = 3.0f;
        ConnectValue(Define.ChangableValue.Hp, typeof(Enemy).GetProperty("MaxHealth"), typeof(Enemy).GetProperty("Health"));
        ConnectValue(Define.ChangableValue.Mp, typeof(Enemy).GetProperty("Mp"), typeof(Enemy).GetProperty("MaxMp"));
        base.Start();
    }
    /// <summary>
    /// 1. �÷��̾� �� �ؿ������� �׸��� Į���� ��Ƴ� (�ö���� ���� ��ġ ǥ��)
    /// </summary>
    /// <returns></returns>
    private IEnumerator RisingBlade()
    {
        yield break;
    }
    /// <summary>
    /// ���� ���� �׸��� Į���� �߻�
    /// </summary>
    private IEnumerator ShootingBlade()
    {
        yield break;
    }
    /// <summary>
    /// 1. ������ ������� HP/MP ����Ī N�� �� �����ؼ� N�ʰ� ��������Ұ�
    /// </summary>
    private IEnumerator Stealth()
    {
        yield break;
    }
    /// <summary>
    /// N �� ���� ������ ���ſ��� �������(�����) �׸��� Į���� �߻� (������� ȸ�ǰ���)
    /// </summary>
    /// <returns></returns>
    private IEnumerator FanOfBlade()
    {
        yield break;
    }

}
