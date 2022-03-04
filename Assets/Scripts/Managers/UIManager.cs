using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class UIManager : Singleton<UIManager>
{
    /// UI에서 사용되는 값 마다의 색상
    [HideInInspector]
    public Color[] ColorOfValues;
    [SerializeField]
    Sprite[] achievementSprite;
    [SerializeField]
    Sprite UnrevealedAchievement;
    [SerializeField]
    private GameObject settings;
    [SerializeField]
    private GameObject reload;
    [SerializeField]
    private GameObject cancel;

    
    public Image FadePanelPrefab;
    public Image AchievementPanelPrefab;
    public Image AchievementPopupPrefab;
    public Image AchieveBackPrefab;

    Image AchievementPanel;
    Image FadePanel;
    Image AchieveBack;

    Button askReloadReload;
    Button askReloadCancel;
    Button askQuitQuit;
    Button askQuitCancel;


    Canvas canvas;
    [SerializeField]
    float fadeTime;
    bool isTrophyOn = false;

    void Awake()
    {
        ColorOfValues = new Color[System.Enum.GetValues(typeof(Define.ChangableValue)).Length];
        ColorOfValues[(int)Define.ChangableValue.Hp] = Color.red;
        ColorOfValues[(int)Define.ChangableValue.Mp] = new Color(0, 0, 0.5f);
        ColorOfValues[(int)Define.ChangableValue.Speed] = new Color(0, 0.5f, 0);
        FadePanel = Instantiate(FadePanelPrefab).GetComponent<Image>();
        AchieveBack = Instantiate(AchieveBackPrefab).GetComponent<Image>();
        FadePanel.gameObject.SetActive(false);
        AchieveBack.gameObject.SetActive(false);
        achievementSprite = new Sprite[GameManager.Instance.isAchieved.Length];
        

    }
    private void Start()
    {
        Initialize();
        canvas = FindObjectOfType<Canvas>();
        FadePanel.transform.SetParent(canvas.transform);
        FadePanel.GetComponent<RectTransform>().anchoredPosition = Vector3.zero;
        AchieveBack.transform.SetParent(canvas.transform);
        AchieveBack.GetComponent<RectTransform>().anchoredPosition = Vector3.zero;





        StartCoroutine(FadeOut());
    }

    private void Initialize()
    {
        DontDestroyOnLoad(gameObject);
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            AchieveBack.gameObject.SetActive(isTrophyOn = !isTrophyOn);
            if(isTrophyOn) AchiveMenuOpen();
        }
    }

    public void SetAchievementPanel(string fillin,int idx)
    {
        StartCoroutine(AchievePanelSpawn(fillin,idx));
    }

    public IEnumerator AchievePanelSpawn(string fillin, int idx)
    {
        float time = 1.5f;
        AchievementPanel = Instantiate(AchievementPanelPrefab, canvas.transform);
        AchievementPanel.GetComponent<RectTransform>().anchoredPosition = new Vector3(-200, -75, 0);
        AchievementPanel.GetComponentInChildren<TextMeshProUGUI>().text = fillin;
        AchievementPanel.transform.GetChild(2).GetComponent<Image>().sprite = achievementSprite[idx];
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

    public void AchiveMenuOpen()
    {
        if (AchieveBack.transform.GetChild(0).childCount == 0)
        {
            for (int i = 0; i < GameManager.Instance.isAchieved.Length; i++)
            {
                {
                    Image temp = Instantiate(AchievementPopupPrefab, AchieveBack.transform.GetChild(0));
                    if (GameManager.Instance.isAchieved[i])
                    {
                        temp.GetComponentsInChildren<Text>(true)[0].text = GameManager.Instance.achieveList[i, 0];
                        temp.GetComponentsInChildren<Text>(true)[1].text = GameManager.Instance.achieveList[i, 1];
                        temp.transform.GetChild(2).GetComponent<Image>().sprite = achievementSprite[i];
                    }
                    else
                    {
                        temp.GetComponentsInChildren<Text>(true)[0].text = "???";
                        temp.GetComponentsInChildren<Text>(true)[1].text = "???";
                        temp.transform.GetChild(2).GetComponent<Image>().sprite = UnrevealedAchievement;
                    }
                }
            }
        }
        else
        {
            for (int i = 0; i < GameManager.Instance.isAchieved.Length; i++)
            {   
                if (GameManager.Instance.isAchieved[i])
                {
                    if (!GameManager.Instance.achieveList[i, 0].Equals(AchieveBack.GetComponentsInChildren<Transform>()[i].GetComponentsInChildren<Text>()[0]))
                    {
                        AchieveBack.transform.GetChild(0).GetChild(i).GetComponentsInChildren<Text>()[0].text = GameManager.Instance.achieveList[i, 0];
                        AchieveBack.transform.GetChild(0).GetChild(i).GetComponentsInChildren<Text>()[1].text = GameManager.Instance.achieveList[i, 1];
                        AchieveBack.transform.GetChild(0).GetChild(i).GetChild(2).GetComponent<Image>().sprite = achievementSprite[i];
                    }   
                }
            }
        }
    }
    public void SettingsButton()
    {
        Time.timeScale = 0;
        settings.SetActive(true);
        if (GameManager.Instance.player != null)
        {
            GameManager.Instance.player.isControllable = false;
        }
    }

    public void CloseSettingsButton()
    {
        Time.timeScale = 1;
        settings.SetActive(false);
        if (GameManager.Instance.player != null)
        {
            GameManager.Instance.player.isControllable = true;
        }
    }

    public void RestartButton()
    {
        settings.SetActive(false);
        reload.SetActive(true);
    }

    public void ToMenuButton()
    {
        cancel.SetActive(true);
        settings.SetActive(false);
    }

    public void ReloadReload()
    {
        Time.timeScale = 1;
        if (GameManager.Instance.player != null)
        {
            GameManager.Instance.player.isControllable = true;
        }
        //SceneManager.LoadScene(sceneToLoad);
    }
    public void ReloadCancel()
    {
        settings.SetActive(true);
        reload.SetActive(false);
    }
    public void QuitQuit()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("MainMenu");
    }
    public void QuitCancel()
    {
        cancel.SetActive(false);
        settings.SetActive(true);
    }
}
