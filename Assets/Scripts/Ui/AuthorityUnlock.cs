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
        StartCoroutine(Indicate());
    }

    IEnumerator Indicate()
    {
        image.gameObject.SetActive(true);
        Debug.Log(image.name + ", " + tx.name);
        tx.text = str1;
        yield return new WaitForSeconds(2f);
        if(AbilityDescription != null)
        {
            tx.text = AbilityDescription;
        }
        yield return new WaitForSeconds(2f);
        image.gameObject.SetActive(false);
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
