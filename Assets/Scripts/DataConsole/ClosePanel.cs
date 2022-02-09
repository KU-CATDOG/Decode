using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClosePanel : MonoBehaviour
{
    public void OnClickExit()
    {
        Time.timeScale = 1;
        Destroy(this.transform.parent.gameObject);
    }
}
