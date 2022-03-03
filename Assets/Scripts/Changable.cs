using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Changable : MonoBehaviour // 값을 변경할 수 있는 object들은 이 클래스를 상속해야 한다.
{
    private Canvas canvas;
    private List<Define.ChangableValue> changableValues = new List<Define.ChangableValue>();  // 변경 가능한 값의 종류
    protected Bar[] barofChangableValues; // UI 바
    private Bar defaultBar;
    private int selected = 0;
    private Dictionary<Define.ChangableValue, System.Reflection.PropertyInfo> dict = new Dictionary<Define.ChangableValue,System.Reflection.PropertyInfo>(); //ChangableValue와  그에 해당하는 property로 구성된 dictionary  
    public Dictionary<Define.ChangableValue, System.Reflection.PropertyInfo> GetValue
    {
        get
        {
            return dict;
        }
    }
    private Dictionary<Define.ChangableValue, System.Reflection.PropertyInfo> maxDict = new Dictionary<Define.ChangableValue, System.Reflection.PropertyInfo>(); // ChangableValue와 그에 해당하는 Max값 Property로 구성된 dictionary
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
    /// 값을 변경하기 위해 외부에서 호출되는 함수
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
    /// property와 ChangableValue를 연결시켜주는 함수
    /// Changable을 상속받는 객체의 Start에서 반드시 호출해주어야 됨
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

    protected virtual float AddValue(float value) // 값을 단순히 더하거나 빼는 무기에서 호출되는 메서드
    {
        object nowValue = dict[changableValues[selected]].GetValue(this);
        object maxVal = maxDict[changableValues[selected]].GetValue(this);
        float nextVal = Mathf.Max(0, Mathf.Min((float)nowValue + value, (float)maxVal));
        dict[changableValues[selected]].SetValue(this, nextVal);
        Debug.Log($"ADD VALUE: {changableValues[selected]}, {dict[changableValues[selected]]}, {dict[changableValues[selected]].GetValue(this)}");
        return nextVal;
    }

    /// <summary>
    /// 각 상속받은 스크립트에서 dictionary를 완성한 후 반드시 호출해줘야 하는 함수
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
