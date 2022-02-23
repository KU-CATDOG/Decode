using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HowToAttackPanel : MonoBehaviour
{
    public string enemyName;

    void Update()
    {
        if (null == GameObject.Find(enemyName))
            Destroy(gameObject);
    }
}
