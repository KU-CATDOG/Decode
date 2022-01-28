using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    public Player player;

    void Start()
    {
        //FIXME
        player = FindObjectOfType<Player>();
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

        }
        if (player.health <= 0)
        {
            //Debug.Log("Player Dead");
            //gameObject.SetActive(false);
        }

    }
}
