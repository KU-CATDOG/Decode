using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Weapon : MonoBehaviour
{
    protected float[] magnitudeOfChange;                  // �ѹ��� �����ϴ� �� ũ�� ( ���� ������ ������ �ϳ��� ���� )
    [SerializeField]
    protected int range;                                // ���� �����Ÿ�
    protected HitBox hb;                                // ��Ʈ�ڽ� ������Ʈ
    [SerializeField]
    protected float coolTime;                           // ���� �� ���ð�
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
    //�浹������ ��ӹ��� class���� �� ���⿡ �°� ������
    protected abstract void OnTriggerEnter2D(Collider2D collision);
    /*{

        hb.gameObject.setActive(false);
        if(normalAttack){
            
        }
        else{
        
        }

    }*/
    public IEnumerator AttackRoutine(Define.MouseEvent mouse) // normalAttack: ��Ŭ���� �� true�� ����
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
