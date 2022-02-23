using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class HpCanvas : MonoBehaviour
{
    [SerializeField] private GameObject obj;
    private Canvas canvas;

    // Start is called before the first frame update
    void Start()
    {
        canvas = FindObjectOfType<Canvas>();
        Instantiate(obj, canvas.transform);
    }
}
