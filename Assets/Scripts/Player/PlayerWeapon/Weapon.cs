using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Weapon : MonoBehaviour
{
    protected float[] magnitudeOfChange;                  // 한번에 변경하는 값 크기 ( 변경 가능한 값마다 하나씩 존재 )
    [SerializeField]
    protected int range;                                // 무기 사정거리
    protected HitBox hb;                                // 히트박스 오브젝트
    [SerializeField]
    protected float coolTime;                           // 공격 후 대기시간
    protected float curCoolTime;
    protected Define.MouseEvent mouse;
    protected void Awake()
    {
        hb = GetComponentInChildren<HitBox>();
    }
    protected virtual void Start()
    {
        int numOfValues = System.Enum.GetValues(typeof(Define.ChangableValue)).Length;
        curCoolTime = 0;
        magnitudeOfChange = new float[numOfValues];
    }
    protected void Update()
    {
        if (curCoolTime > 0)
        {
            curCoolTime = Mathf.Max(curCoolTime - Time.deltaTime, 0);
        }
    }
    //충돌판정은 상속받은 class에서 각 무기에 맞게 구현함
    protected abstract void OnTriggerEnter2D(Collider2D collision);
    /*{

        hb.gameObject.setActive(false);
        if(normalAttack){
            
        }
        else{
        
        }

    }*/
    public IEnumerator AttackRoutine(Define.MouseEvent mouse) // normalAttack: 좌클릭일 때 true로 전달
    {
        this.mouse = mouse;
        if (curCoolTime > 0)
        {
            yield break;
        }
        curCoolTime = coolTime;
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        hb.transform.position = (Vector2)transform.position + (mousePos - (Vector2)transform.position).normalized * range;
        hb.gameObject.SetActive(true);
        yield return new WaitForSeconds(0.1f);
        hb.gameObject.SetActive(false);
    }
    
    
}
