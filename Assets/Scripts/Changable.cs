using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Changable : MonoBehaviour // 값을 변경할 수 있는 object들은 이 클래스를 상속해야 한다.
{
    private Canvas canvas;
    [HideInInspector]
    public Define.ChangableValue[] changableValues;  // 변경 가능한 값의 종류
    protected Bar[] barofChangableValues; // UI 바
    [SerializeField]
    private Bar defaultBar;
    public int Selected { get; private set; } = 0;
    public Dictionary<Define.ChangableValue, System.Reflection.PropertyInfo> dict = new Dictionary<Define.ChangableValue,System.Reflection.PropertyInfo>(); //ChangableValue와  그에 해당하는 property로 구성된 dictionary  
    public Dictionary<Define.ChangableValue, System.Reflection.PropertyInfo> maxDict = new Dictionary<Define.ChangableValue, System.Reflection.PropertyInfo>(); // ChangableValue와 그에 해당하는 Max값 Property로 구성된 dictionary
    /// <summary>
    /// 값을 변경하기 위해 외부에서 호출되는 함수
    /// 중간발표 이후 수정
    /// </summary>
    public void ChangeVal(Define.ChangeType type, float value = 0)
    {
        float nextVal = AddValue(value);
        barofChangableValues[Selected].Value = nextVal;
    }
    
    private float AddValue(float value) // 값을 단순히 더하거나 빼는 무기에서 호출되는 메서드
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
    /// 각 상속받은 스크립트에서 dictionary를 완성한 후 반드시 호출해줘야 하는 함수
    /// 중간발표 후 수정
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
