using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HpBar : MonoBehaviour
{
    public Player player;

    private Slider hpSlider;
    private float hp;
    private float maxHp;
    private Text hpPerMaxHptxt;

    void Start()
    {
        player = FindObjectOfType<Player>();
        hpSlider = GetComponent<Slider>();
        hpPerMaxHptxt = GetComponentsInChildren<Text>()[1];
    }

    void Update()
    {
        hp = player.health;
        maxHp = player.maxHealth;
        hpSlider.value = hp / maxHp;
        hpPerMaxHptxt.text = $"{hp}/{maxHp}";
    }
}
