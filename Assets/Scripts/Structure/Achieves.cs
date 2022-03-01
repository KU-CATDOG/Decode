using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Achieves : MonoBehaviour
{
    public int idx;
    public string fillin;
    float rotz;

    private void Update()
    {
        rotz += 360 / 10 * Time.deltaTime;
        gameObject.transform.rotation = Quaternion.Euler(0, 0, rotz);
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player" && Input.GetKey(KeyCode.G))
        {
            other.GetComponent<Player>().ActivateAchievement(idx);
            UIManager.Instance.SetAchievementPanel(fillin);
            Destroy(gameObject);
        }
    }
}
