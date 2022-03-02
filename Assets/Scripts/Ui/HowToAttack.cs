 using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HowToAttack : MonoBehaviour
{
    [SerializeField] private GameObject obj;
    private GameObject howToAttack;
    private Image image;
    private Canvas canvas;

    void Start()
    {
        canvas = FindObjectOfType<Canvas>();
        howToAttack = Instantiate(obj, canvas.transform);
        image = howToAttack.GetComponent<Image>();
        howToAttack.GetComponent < HowToAttackPanel > ().enemyName = this.gameObject.name;
    }

    void Update()
    {
        Vector3 monsterPos = Camera.main.WorldToViewportPoint(new Vector3(this.transform.position.x, this.transform.position.y, 0));
        if ((monsterPos.x < 0.9f) && (monsterPos.x > 0.1f) && (monsterPos.y < 0.9f) && (monsterPos.y > 0.1f)) // 카메라 안 일정 범위에 들어오면 공격 조작법
        {
            image.gameObject.SetActive(true);
            Vector3 panelPos = Camera.main.WorldToScreenPoint(new Vector3(this.transform.position.x, this.transform.position.y + 3.5f, 0));
            image.transform.position = panelPos;
        }
        else
            image.gameObject.SetActive(false);
    }

}