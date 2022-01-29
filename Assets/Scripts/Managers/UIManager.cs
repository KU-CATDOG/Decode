using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : Singleton<UIManager>
{
    /// UI���� ���Ǵ� �� ������ ����
    public Color[] ColorOfValues;
    void Start()
    {
        ColorOfValues = new Color[System.Enum.GetValues(typeof(Define.ChangableValue)).Length];
        ColorOfValues[(int)Define.ChangableValue.Hp] = Color.red;
        ColorOfValues[(int)Define.ChangableValue.Mp] = new Color(0, 0, 0.5f);
        ColorOfValues[(int)Define.ChangableValue.Speed] = new Color(0, 0.5f, 0);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
