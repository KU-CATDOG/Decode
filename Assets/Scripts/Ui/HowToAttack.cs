using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HowToAttack : MonoBehaviour
{
    [SerializeField] private Image image;

    void Update()
    {
        Vector3 monsterPos = Camera.main.WorldToViewportPoint(new Vector3(this.transform.position.x, this.transform.position.y, 0));
        if ((monsterPos.x < 0.9f) && (monsterPos.x > 0.1f) && (monsterPos.y < 0.9f) && (monsterPos.y > 0.1f)) // ī�޶� �� ���� ������ ������ ���� ���۹�
        {
            image.gameObject.SetActive(true);
            Vector3 panelPos = Camera.main.WorldToScreenPoint(new Vector3(this.transform.position.x, this.transform.position.y + 2.0f, 0));
            image.transform.position = panelPos;
        }
        else
            image.gameObject.SetActive(false);
    }

    void FixedUpdate()
    {
        if (this.GetComponent<Melee>().Health <= 0)
            image.gameObject.SetActive(false);
    }
}