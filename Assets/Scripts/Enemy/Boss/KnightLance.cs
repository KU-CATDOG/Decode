using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnightLance : MonoBehaviour
{
    Rigidbody2D rb;
    bool drop = false;
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!drop && collision.tag == "Wall" || collision.tag == "Floor")
        {
            drop = true;
            rb.velocity = Vector2.zero;
        }
        if(!drop && collision.tag == "Player")
        {
            GameManager.Instance.GetDamaged(20);
        }
        Knight knight;
        if(drop && (knight = collision.GetComponent<Knight>()))
        {
            knight.animator.SetBool("HaveWeapon", true);
            knight.animator.SetTrigger("EndAction");
            transform.parent = collision.transform;
            knight.HaveLance = true;
            drop = false;
            gameObject.SetActive(false);
        }
    }
}
