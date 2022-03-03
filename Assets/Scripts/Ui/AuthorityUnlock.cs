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

    Canvas canvas;

    Text tx;

    private void Start()
    {
        tx = panel.GetComponent<Text>();
        canvas = FindObjectOfType<Canvas>();

    }

    public void Alarm()
    {
        Image image = Instantiate(panel, canvas.transform);
        string str = "Access authorized\n" + authority;
        StartCoroutine(TypeEff(str));
    }

    IEnumerator TypeEff(string str)
    {
        yield return new WaitForSeconds(1f);
        for (int i = 0; i <= str.Length; i++)
        {
            tx.text = str.Substring(0, i);
            yield return new WaitForSeconds(0.1f);
        }
        yield return new WaitForSeconds(2f);

        for (int i = str.Length; i >= 0; i--)
        {
            tx.text = str.Substring(0, i);
            yield return new WaitForSeconds(0.1f);
        }
    }
}
