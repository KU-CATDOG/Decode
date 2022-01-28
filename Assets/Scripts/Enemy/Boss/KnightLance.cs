using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnightLance : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Knight")
        {
            /// 창 줍는 모션, 애니메이션 적용을 리소스 나오면 추가해야됨
            transform.parent = collision.transform;
            collision.GetComponent<Knight>().HaveLance = true;
        }
    }
}
