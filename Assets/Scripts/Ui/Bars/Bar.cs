using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Bar : MonoBehaviour
{
    public float MaxVal { get; set; }
    public Transform Parent { get; set; }
    public Changable ParentChangable { get; set; }
    public Image BarBackGround { get; set; }
    public Image BarFill { get; set; }
    public bool IsBoss { get; set; }
    private RectTransform rect;
    private float val;
    private Text valueNameText;
    private Text valuePerMaxValueText;
    private Slider BarSlider;
    
    private void Awake()
    {
        BarSlider = GetComponent<Slider>();
        Text[] txts = GetComponentsInChildren<Text>();
        valueNameText = txts[0];
        valuePerMaxValueText = txts[1];
        Image[] images = GetComponentsInChildren<Image>();
        BarBackGround = images[0];
        BarFill = images[1];
        rect = GetComponent<RectTransform>();
    }
    private void Start()
    {
        if (IsBoss)
        {
            rect.anchorMin = new Vector2(0, 1);
            rect.anchorMax = new Vector2(1, 1);
            rect.offsetMin = new Vector2(30, -30);
            rect.offsetMax = new Vector2(-30, 0);
        }
    }
    public float Value
    {
        get
        {
            return val;
        }
        set
        {
            val = value;
            BarSlider.value = val / MaxVal;
            valuePerMaxValueText.text = $"{value}/{MaxVal}";
        }
    }
    private Define.ChangableValue valueName;
    public Define.ChangableValue ValueName
    {
        get
        {
            return valueName;
        }
        set
        {
            valueName = value;
            valueNameText.text = value.ToString();
        }
    }
    private void Update()
    {
        if(!IsBoss)
            transform.position = Camera.main.WorldToScreenPoint(Parent.position + Vector3.up * (Parent.localScale.y+1));
        BarSlider.value = val / MaxVal;
        val = (float)ParentChangable.GetValue[ValueName].GetValue(ParentChangable);
        valuePerMaxValueText.text = $"{val}/{MaxVal}";
    }
}
