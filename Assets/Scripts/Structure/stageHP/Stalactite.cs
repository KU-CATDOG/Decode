using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stalactite : MonoBehaviour
{
    public Rigidbody2D rb;

    [SerializeField] private float damage;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player"))
            GameManager.Instance.GetDamaged(damage/2);
        Destroy(gameObject);
    }
}
