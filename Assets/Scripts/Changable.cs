using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Changable : MonoBehaviour // ���� ������ �� �ִ� object���� �� Ŭ������ ����ؾ� �Ѵ�.
{
    private Canvas canvas;
    private List<Define.ChangableValue> changableValues = new List<Define.ChangableValue>();  // ���� ������ ���� ����
    protected Bar[] barofChangableValues; // UI ��
    private Bar defaultBar;
    private int selected = 0;
    private Dictionary<Define.ChangableValue, System.Reflection.PropertyInfo> dict = new Dictionary<Define.ChangableValue,System.Reflection.PropertyInfo>(); //ChangableValue��  �׿� �ش��ϴ� property�� ������ dictionary  
    public Dictionary<Define.ChangableValue, System.Reflection.PropertyInfo> GetValue
    {
        get
        {
            return dict;
        }
    }
    private Dictionary<Define.ChangableValue, System.Reflection.PropertyInfo> maxDict = new Dictionary<Define.ChangableValue, System.Reflection.PropertyInfo>(); // ChangableValue�� �׿� �ش��ϴ� Max�� Property�� ������ dictionary
    public Dictionary<Define.ChangableValue, System.Reflection.PropertyInfo> GetMaxValue
    {
        get
        {
            return maxDict;
        }
    }

    public Define.ChangableValue GetCurSelected()
    {
        return changableValues[selected];
    }
    /// <summary>
    /// ���� �����ϱ� ���� �ܺο��� ȣ��Ǵ� �Լ�
    /// </summary>
    public void ChangeVal(Define.ChangeType type, float value = 0)
    {
        float nextVal;
        switch (type)
        {
            case Define.ChangeType.Add:
                nextVal= AddValue(value);
                break;
            default:
                nextVal = -1;
                break;
        }

        if (GetCurSelected() == (int)Define.ChangableValue.Hp && nextVal == 0)
        {
            Die();
        }
            barofChangableValues[selected].Value = nextVal;
    }
    /// <summary>
    /// property�� ChangableValue�� ��������ִ� �Լ�
    /// Changable�� ��ӹ޴� ��ü�� Start���� �ݵ�� ȣ�����־�� ��
    /// </summary>
    public void ConnectValue(Define.ChangableValue changableValue, System.Reflection.PropertyInfo maxVal, System.Reflection.PropertyInfo value)
    {
        changableValues.Add(changableValue);
        dict[changableValue] = value;
        maxDict[changableValue] = maxVal;
    }
    public virtual void SelectValuetoChange()
    {
        barofChangableValues[selected].gameObject.SetActive(false);
        selected = (selected + 1) % changableValues.Count;
        barofChangableValues[selected].gameObject.SetActive(true);
        Debug.Log($"Select Value: {selected}");
    }

    protected virtual float AddValue(float value) // ���� �ܼ��� ���ϰų� ���� ���⿡�� ȣ��Ǵ� �޼���
    {
        object nowValue = dict[changableValues[selected]].GetValue(this);
        object maxVal = maxDict[changableValues[selected]].GetValue(this);
        float nextVal = Mathf.Max(0, Mathf.Min((float)nowValue + value, (float)maxVal));
        dict[changableValues[selected]].SetValue(this, nextVal);
        Debug.Log($"ADD VALUE: {changableValues[selected]}, {dict[changableValues[selected]]}, {dict[changableValues[selected]].GetValue(this)}");
        return nextVal;
    }

    /// <summary>
    /// �� ��ӹ��� ��ũ��Ʈ���� dictionary�� �ϼ��� �� �ݵ�� ȣ������� �ϴ� �Լ�
    /// </summary>
    protected void InitializeBars(bool isBoss = false)
    {
        canvas = FindObjectOfType<Canvas>();
        barofChangableValues = new Bar[changableValues.Count];
        for(int i = 0; i<changableValues.Count; i++)
        {
            Debug.Log("1");
            barofChangableValues[i] = Instantiate(defaultBar, canvas.transform);
            barofChangableValues[i].IsBoss = isBoss;
            barofChangableValues[i].Parent = transform;
            barofChangableValues[i].ParentChangable = this;
            if (selected != i)
                barofChangableValues[i].gameObject.SetActive(false);
            barofChangableValues[i].ValueName = changableValues[i];
            barofChangableValues[i].MaxVal = (float)maxDict[changableValues[i]].GetValue(this);
            barofChangableValues[i].Value = (float)dict[changableValues[i]].GetValue(this);
            barofChangableValues[i].transform.position = Camera.main.WorldToScreenPoint(transform.position);
            barofChangableValues[i].BarFill.color = UIManager.Instance.ColorOfValues[(int)changableValues[i]];
        }
    }
    protected void Awake()
    {
        defaultBar = Tool.AssetLoader.LoadPrefab<GameObject>("UI/Bar").GetComponent<Bar>();
    }

    protected virtual void Die()
    {
        for (int i = 0; i < barofChangableValues.Length; i++)
        {
            Destroy(barofChangableValues[i].gameObject);
        }
        Destroy(gameObject);
        
    }


}
