using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : Singleton<UIManager>
{
    /// UI에서 사용되는 값 마다의 색상
    [HideInInspector]
    public Color[] ColorOfValues;

    public Image FadePanelPrefab;
    public Image AchievementPanelPrefab;
    Image AchievementPanel;
    Image FadePanel;
    Canvas canvas;
    [SerializeField]
    float fadeTime;

    void Awake()
    {
        ColorOfValues = new Color[System.Enum.GetValues(typeof(Define.ChangableValue)).Length];
        ColorOfValues[(int)Define.ChangableValue.Hp] = Color.red;
        ColorOfValues[(int)Define.ChangableValue.Mp] = new Color(0, 0, 0.5f);
        ColorOfValues[(int)Define.ChangableValue.Speed] = new Color(0, 0.5f, 0);
        FadePanel = Instantiate(FadePanelPrefab).GetComponent<Image>();
        FadePanel.gameObject.SetActive(false);
    }
    private void Start()
    {
        DontDestroyOnLoad(gameObject);
        canvas = FindObjectOfType<Canvas>();
        FadePanel.transform.SetParent(canvas.transform);
        FadePanel.GetComponent<RectTransform>().anchoredPosition = new Vector3(0, 0, 0); 
        StartCoroutine(FadeOut());
    }

    public void SetAchievementPanel(string fillin)
    {
        StartCoroutine(AchievePanelSpawn(fillin));
    }

    public IEnumerator AchievePanelSpawn(string fillin)
    {
        float time = 1.5f;
        AchievementPanel = Instantiate(AchievementPanelPrefab, canvas.transform);
        AchievementPanel.GetComponent<RectTransform>().anchoredPosition = new Vector3(-200, -75, 0);
        AchievementPanel.GetComponentInChildren<TextMeshProUGUI>().text = fillin;
        RectTransform rectT = AchievementPanel.GetComponent<RectTransform>();
        rectT.anchoredPosition = new Vector3(-200, 75, 0);
        yield return new WaitForSeconds(3f);
        Vector3 destination = new Vector3(rectT.anchoredPosition.x, rectT.anchoredPosition.y - 150, 0);
        Vector3 startPosition = rectT.anchoredPosition;
        for (float t = 0; t <= time; t += Time.deltaTime)
        {
            rectT.anchoredPosition =
                Vector3.Lerp(startPosition, destination, t / time);
            AchievementPanel.color = new Color(11,11,29, 1 - t/time); 
            yield return null;
        }
        rectT.anchoredPosition = destination;
        Destroy(AchievementPanel.gameObject);
    }

    IEnumerator FadeOut()
    {
        Debug.Log("clear");
        FadePanel.gameObject.SetActive(true);
        float alpha = 1f;
        FadePanel.color = new Color(0, 0, 0, 1);
        for (;FadePanel.color.a > 0;)
        {
            alpha -= Time.deltaTime / fadeTime;
            FadePanel.color = new Color(0, 0, 0, alpha);
            yield return null;
        }
        FadePanel.gameObject.SetActive(false);
    }
}
