using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AuthorityUnlock : MonoBehaviour
{
    [SerializeField]
    private Image panel;
    [SerializeField]
    private string authority;
    [SerializeField]
    private string AbilityDescription;

    string str1;
    Image image;
    Canvas canvas;

    Text tx;

    private void Start()
    {
        canvas = FindObjectOfType<Canvas>();
        image = Instantiate(panel, canvas.transform);
        tx = image.GetComponentInChildren<Text>();
        Debug.Log(image.name + ", " + tx.name);
        image.gameObject.SetActive(false);
    }

    public void Alarm()
    {
        str1 = "Access authorized\n" + authority;
        UIManager.Instance.Indication(image, str1, AbilityDescription, tx);
    }
}
