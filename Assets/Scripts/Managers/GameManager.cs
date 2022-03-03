using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    public Player player;
    public string prevScene;
    public string[,] achieveList;
    [SerializeField]
    public bool[] isAchieved;

    private void Awake()
    {
        if (GameManager.Instance != this) Destroy(this.gameObject);
        DontDestroyOnLoad(gameObject);
        achieveList = new string[isAchieved.Length,2];
    }
    void Start()
    {
        //player = FindObjectOfType<Player>();
    }

    // Update is called once per frame
    void Update()
    {
        InputManager.Instance.OnUpdate();
    }

    public void GetDamaged(float damage)
    {
        if (!player.isInvincible)
        {
            player.health -= damage;
            Debug.Log("Player dmg taken: " + damage);
            StartCoroutine(player.DamagedRoutine());

        }
        if (player.health <= 0)
        {
            //Debug.Log("Player Dead");
            //player.gameObject.SetActive(false);
            //Load.Instance.LoadSaveData(SaveManager.Instance.lastSaveNumber); // �ֱ� �����ߴ� ������ ������ �̵�
        }

    }
}
