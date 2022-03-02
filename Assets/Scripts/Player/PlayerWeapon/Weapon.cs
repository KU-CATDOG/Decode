using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Weapon : MonoBehaviour
{
    protected float[] magnitudeOfChange;                  // �ѹ��� �����ϴ� �� ũ�� ( ���� ������ ������ �ϳ��� ���� )
    [SerializeField]
    protected int range;                                // ���� �����Ÿ�
    protected HitBox hb;                                // ��Ʈ�ڽ� ������Ʈ
    protected Define.MouseEvent mouse;
    protected void Awake()
    {
        hb = GetComponentInChildren<HitBox>();
    }
    protected virtual void Start()
    {
        int numOfValues = System.Enum.GetValues(typeof(Define.ChangableValue)).Length;
        //GameManager.Instance.player.curCoolTime = 0;
        magnitudeOfChange = new float[numOfValues];
    }
    protected void Update()
    {
        if (GameManager.Instance.player.curCoolTime > 0)
        {
            GameManager.Instance.player.curCoolTime 
                = Mathf.Max(GameManager.Instance.player.curCoolTime - Time.deltaTime, 0);
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            ChangeSign();
        }
    }
    //�浹������ ��ӹ��� class���� �� ���⿡ �°� ������
    protected abstract void OnTriggerEnter2D(Collider2D collision);
    /*EXAMPLE CODE
     * {

        hb.gameObject.setActive(false);
        if(normalAttack){
            
        }
        else{
        
        }

    }*/
    public void ChangeSign()
    {
        for(int i = 0; i < magnitudeOfChange.Length; i++)
        {
            magnitudeOfChange[i] = -magnitudeOfChange[i];
        }
    }
    public IEnumerator AttackRoutine(Define.MouseEvent mouse) // normalAttack: ��Ŭ���� �� true�� ����
    {
        
        this.mouse = mouse;
        if (GameManager.Instance.player.curCoolTime > 0)
        {
            GameManager.Instance.player.dirLock = false;
            yield break;
        }
        GameManager.Instance.player.curCoolTime = GameManager.Instance.player.coolTime;
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        hb.transform.position = (Vector2)transform.position + (mousePos - (Vector2)transform.position).normalized * range;
        hb.gameObject.SetActive(true);
        yield return new WaitForSeconds(0.1f);
        hb.gameObject.SetActive(false);
        GameManager.Instance.player.dirLock = false;

    }
    
    
}
