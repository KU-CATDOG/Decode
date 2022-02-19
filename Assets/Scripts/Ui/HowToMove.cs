using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HowToMove : MonoBehaviour
{
    [SerializeField] private Image image;
    [SerializeField] private Text text;
    [SerializeField] private GameObject player;
    bool isFaded = true;

    void Awake()
    {
        player = GameObject.Find("Player");
    }

    void Update()
    {
        Vector3 panelPos = Camera.main.WorldToScreenPoint(new Vector3(player.transform.position.x, player.transform.position.y + 2.0f, 0));
        this.transform.position = panelPos;
        if ((Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.A)) && isFaded)
            StartCoroutine(FadeOutCoroutine());
    }

    IEnumerator FadeOutCoroutine()
    {
        float fadeCount = 0;
        isFaded = false;
        yield return new WaitForSeconds(5.0f);
        while (fadeCount < 1.0f)
        {
            fadeCount += 0.01f;
            yield return new WaitForSeconds(0.01f);
            image.color = new Color(255, 255, 255, 1.0f - fadeCount);
            text.color = new Color(50, 50, 50, 1.0f - fadeCount);
        }
        Destroy(gameObject);
    }
}