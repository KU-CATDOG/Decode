using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnightLance : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Knight")
        {
            /// â �ݴ� ���, �ִϸ��̼� ������ ���ҽ� ������ �߰��ؾߵ�
            transform.parent = collision.transform;
            collision.GetComponent<Knight>().HaveLance = true;
        }
    }
}
