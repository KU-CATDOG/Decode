using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Bar : MonoBehaviour
{
    public float MaxVal { get; set; }
    public Transform parent { get; set; }
    private float val;
    private Text valueNameText;
    private Text valuePerMaxValueText;
    private Slider BarSlider;
    public Image BarBackGround { get; set; }
    public Image BarFill { get; set; }
    private void Awake()
    {
        BarSlider = GetComponent<Slider>();
        Text[] txts = GetComponentsInChildren<Text>();
        valueNameText = txts[0];
        valuePerMaxValueText = txts[1];
        Image[] images = GetComponentsInChildren<Image>();
        BarBackGround = images[0];
        BarFill = images[1];
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
        transform.position = Camera.main.WorldToScreenPoint(parent.position + Vector3.up * (parent.localScale.y+1));
    }
}
