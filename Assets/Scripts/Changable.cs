using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Changable : MonoBehaviour // ���� ������ �� �ִ� object���� �� Ŭ������ ����ؾ� �Ѵ�.
{
    private Canvas canvas;
    [HideInInspector]
    public Define.ChangableValue[] changableValues;  // ���� ������ ���� ����
    protected Bar[] barofChangableValues; // UI ��
    [SerializeField]
    private Bar defaultBar;
    public int Selected { get; private set; } = 0;
    public Dictionary<Define.ChangableValue, System.Reflection.PropertyInfo> dict = new Dictionary<Define.ChangableValue,System.Reflection.PropertyInfo>(); //ChangableValue��  �׿� �ش��ϴ� property�� ������ dictionary  
    public Dictionary<Define.ChangableValue, System.Reflection.PropertyInfo> maxDict = new Dictionary<Define.ChangableValue, System.Reflection.PropertyInfo>(); // ChangableValue�� �׿� �ش��ϴ� Max�� Property�� ������ dictionary
    /// <summary>
    /// ���� �����ϱ� ���� �ܺο��� ȣ��Ǵ� �Լ�
    /// �߰���ǥ ���� ����
    /// </summary>
    public void ChangeVal(Define.ChangeType type, float value = 0)
    {
        float nextVal = AddValue(value);
        barofChangableValues[Selected].Value = nextVal;
    }
    
    private float AddValue(float value) // ���� �ܼ��� ���ϰų� ���� ���⿡�� ȣ��Ǵ� �޼���
    {
        object nowValue = dict[changableValues[Selected]].GetValue(this);
        object maxVal = maxDict[changableValues[Selected]].GetValue(this);
        float nextVal = Mathf.Max(0, Mathf.Min((float)nowValue + value, (float)maxVal));
        dict[changableValues[Selected]].SetValue(this, nextVal);
        Debug.Log($"ADD VALUE: {changableValues[Selected]}, {dict[changableValues[Selected]]}, {dict[changableValues[Selected]].GetValue(this)}");
        return nextVal;
    }

    public void SelectValuetoChange()
    {
        barofChangableValues[Selected].gameObject.SetActive(false);
        Selected = (Selected + 1) % changableValues.Length;
        barofChangableValues[Selected].gameObject.SetActive(true);
        Debug.Log($"Select Value: {Selected}");
    }
    /// <summary>
    /// �� ��ӹ��� ��ũ��Ʈ���� dictionary�� �ϼ��� �� �ݵ�� ȣ������� �ϴ� �Լ�
    /// �߰���ǥ �� ����
    /// </summary>
    protected void InitializeBars()
    {
        canvas = FindObjectOfType<Canvas>();
        barofChangableValues = new Bar[changableValues.Length];
        for(int i = 0; i<changableValues.Length; i++)
        {
            barofChangableValues[i] = Instantiate(defaultBar, canvas.transform);
            barofChangableValues[i].Parent = transform;
            barofChangableValues[i].ParentChangable = this;
            if (Selected != i)
                barofChangableValues[i].gameObject.SetActive(false);
            barofChangableValues[i].ValueName = changableValues[i];
            barofChangableValues[i].MaxVal = (float)maxDict[changableValues[i]].GetValue(this);
            barofChangableValues[i].Value = (float)dict[changableValues[i]].GetValue(this);
            barofChangableValues[i].transform.position = Camera.main.WorldToScreenPoint(transform.position);
            barofChangableValues[i].BarFill.color = UIManager.Instance.ColorOfValues[(int)changableValues[i]];
        }
    }
    



}
