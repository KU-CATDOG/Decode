using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Changable : MonoBehaviour // 값을 변경할 수 있는 object들은 이 클래스를 상속해야 한다.
{
    protected Define.ChangableValue[] changableValues;  // 변경 가능한 값의 종류
    public int Selected { get; private set; } = 0;
    protected Dictionary<Define.ChangableValue, System.Reflection.PropertyInfo> dict = new Dictionary<Define.ChangableValue,System.Reflection.PropertyInfo>(); //ChangableValue와  그에 해당하는 property로 구성된 dictionary
                                                                                    //이 class를 상속받아 사용하는 Start에서 초기화
    public void AddValue(float value) // 값을 단순히 더하거나 빼는 무기에서 호출되는 메서드
    {
        Changable changable = this;
        System.Reflection.PropertyInfo property = changable.dict[changableValues[changable.Selected]];
        object nowValue = property.GetValue(changable);
        changable.dict[changableValues[changable.Selected]].SetValue(changable, (float)nowValue + value);
        Debug.Log($"ADD VALUE: {changableValues[changable.Selected]}, {dict[changableValues[changable.Selected]]}, {dict[changableValues[changable.Selected]].GetValue(this)}");
    }
    public void SelectValuetoChange()
    {
        Selected = (Selected + 1) % changableValues.Length;
        Debug.Log($"Select Value: {Selected}");
    }



}
