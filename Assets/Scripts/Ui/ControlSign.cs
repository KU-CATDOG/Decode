using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ControlSign : MonoBehaviour
{
    [SerializeField]
    Image plus_sprite;
    [SerializeField]
    Image minus_sprite;

    void Update()
    {
        Weapon weaponStatus = GameManager.Instance.player.transform.GetChild(0).GetComponent<Weapon>();
        if(weaponStatus.magnitudeOfChange[0] >= 0)
        {
            plus_sprite.transform.GetComponent<Image>().enabled = true;
            minus_sprite.transform.GetComponent<Image>().enabled = false;
        }
        else
        { 
            plus_sprite.transform.GetComponent<Image>().enabled = false;
            minus_sprite.transform.GetComponent<Image>().enabled = true;
        }
    }
}
