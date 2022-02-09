using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActiveConsoleUI : MonoBehaviour
{
    private int numberOfConsole = SaveManager.Instance.activeDataConsole.Length;

    void Start()
    {
        for(int i=0;i<numberOfConsole;i++)
            if (SaveManager.Instance.activeDataConsole[i])
                transform.GetChild(i).gameObject.SetActive(true);
    }
}
