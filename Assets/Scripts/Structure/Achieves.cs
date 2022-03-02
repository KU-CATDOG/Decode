using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Achieves : MonoBehaviour
{
    public int idx;
    public string achieveTitle;
    public string achieveDescript;
    
    float yPos;
    float time;

    private void Start()
    {
        yPos = transform.position.y;    
    }

    private void Update()
    {
        time += Time.deltaTime + Random.Range(0f,0.02f);
        transform.position = new Vector3(transform.position.x, yPos + Mathf.Sin(time)/5f, 0);
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player" && Input.GetKey(KeyCode.G))
        {
            GameManager.Instance.isAchieved[idx] = true;
            GameManager.Instance.achieveList[idx,0] = achieveTitle;
            GameManager.Instance.achieveList[idx,1] = achieveDescript;
            UIManager.Instance.SetAchievementPanel(achieveTitle,idx);
            Destroy(gameObject);
        }
    }
}
