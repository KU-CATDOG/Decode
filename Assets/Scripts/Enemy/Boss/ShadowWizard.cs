using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShadowWizard : Boss
{

    private System.Func<IEnumerator>[] AttackRoutines; // 공격 루틴들
    private int switchingCount = 0; // 값을 변경한 횟수
    [SerializeField]
    private int maxSwitchingCount; // 은신하게되는 기준 변경 횟수
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
    /// 1. 보스를 대상으로 HP/MP 스위칭 N번 시 은신해서 N초간 대상지정불가
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
    /// 1. 플레이어 발 밑에서부터 그림자 칼날을 쏘아냄 (올라오기 전에 위치 표시)
    /// </summary>
    /// <returns></returns>
    private IEnumerator RisingBlade()
    {
        yield break;
    }
    /// <summary>
    /// 손을 뻗어 그림자 칼날을 발사
    /// </summary>
    private IEnumerator ShootingBlade()
    {
        yield break;
    }
    /// <summary>
    /// 1. 보스를 대상으로 HP/MP 스위칭 N번 시 은신해서 N초간 대상지정불가
    /// </summary>
    private IEnumerator Stealth()
    {
        yield break;
    }
    /// <summary>
    /// N 초 힘을 모으고 전신에서 사방으로(방사형) 그림자 칼날을 발사 (구르기로 회피가능)
    /// </summary>
    /// <returns></returns>
    private IEnumerator FanOfBlade()
    {
        yield break;
    }

}
