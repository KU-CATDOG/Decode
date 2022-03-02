using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HowToMove : MonoBehaviour
{
    [SerializeField] private Image image;
    [SerializeField] public Text tx;
    [SerializeField] private Player player;

    private string first;
    private string second;
    bool isfaded = false;
    bool isTyping = false;
    bool typedAll = false;
    RectTransform rectT;

    void Start()
    {
        player = GameManager.Instance.player;
        first = "Standard control\n\"WASD\"";
        second = "Press Space to evade";
        StartCoroutine(TypeEff(first));
    }
    private void Update()
    {
        if (!isTyping && !typedAll)
        {
            StartCoroutine(TypeEff(second));
        }
        if (!isTyping && typedAll && !isfaded) StartCoroutine(FadeOutCoroutine());
    }
    IEnumerator FadeOutCoroutine()
    {
        float fadeCount = 0;
        isfaded = true;
        while (fadeCount < 1.0f)
        {
            fadeCount += 0.01f;
            yield return new WaitForSeconds(0.01f);
            image.color = new Color(255, 255, 255, 1.0f - fadeCount);
            tx.color = new Color(50, 50, 50, 1.0f - fadeCount);
        }
        Destroy(gameObject);
    }

    IEnumerator TypeEff(string str)
    {
        isTyping = true;
        yield return new WaitForSeconds(1f);
        for (int i = 0; i <= str.Length; i++)
        {
            tx.text = str.Substring(0, i);
            yield return new WaitForSeconds(0.1f);
        }
        yield return new WaitForSeconds(1f);

        for (int i = second.Length; i >= 0; i--)
        {
            tx.text = str.Substring(0, i);
            yield return new WaitForSeconds(0.1f);
        }
        isTyping = false;
        if (str.Equals(second)) typedAll = true;
    }
}