using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrbManage : MonoBehaviour
{
    public MpOrb[] Orbs;
    [SerializeField]
    private LineRenderer[] lr;
    private void Start()
    {
        for(int i=0; i < 5; i++)
        {
            lr[i].startWidth = 0.2f;
            lr[i].endWidth = 0.2f;
        }
    }
    private void Update()
    {
        for(int i=0;i < 5; i++)
        {
            if(Orbs[i].isActive && Orbs[(i + 1) % 5].isActive)
            {
                lr[i].SetPosition(0, Orbs[i].transform.position);
                lr[i].SetPosition(1, Orbs[(i + 1) % 5].transform.position);
                lr[i].enabled = true;
            }
            else
            {
                lr[i].enabled = false;
            }
        }
    }
}
