using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Changable : MonoBehaviour // ���� ������ �� �ִ� object���� �� Ŭ������ ����ؾ� �Ѵ�.
{
    protected Define.ChangableValue[] changableValues;  // ���� ������ ���� ����
    public int Selected { get; private set; } = 0;
    protected Dictionary<Define.ChangableValue, System.Reflection.PropertyInfo> dict = new Dictionary<Define.ChangableValue,System.Reflection.PropertyInfo>(); //ChangableValue��  �׿� �ش��ϴ� property�� ������ dictionary
                                                                                    //�� class�� ��ӹ޾� ����ϴ� Start���� �ʱ�ȭ
    public void AddValue(float value) // ���� �ܼ��� ���ϰų� ���� ���⿡�� ȣ��Ǵ� �޼���
    {
        Changable changable = this;
        System.Reflection.PropertyInfo property = changable.dict[(Define.ChangableValue)changable.Selected];
        object nowValue = property.GetValue(changable);
        changable.dict[(Define.ChangableValue)changable.Selected].SetValue(changable, (float)nowValue + value);
        Debug.Log($"ADD VALUE: {(Define.ChangableValue)changable.Selected}, {dict[(Define.ChangableValue)Selected]}, {dict[(Define.ChangableValue)Selected].GetValue(this)}");
    }
    public void SelectValuetoChange()
    {
        Selected = (Selected + 1) % changableValues.Length;
        Debug.Log($"Select Value: {Selected}");
    }



}
