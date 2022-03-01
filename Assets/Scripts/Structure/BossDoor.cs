using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossDoor : MonoBehaviour
{
    [SerializeField] float stoneSpeed;

    private Stone stone;
    
    void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log(collision.gameObject.GetComponent<Rigidbody2D>().velocity.magnitude);
        if (collision.gameObject.name == "Stone")
        {
            stone = collision.gameObject.GetComponent<Stone>();
            if (stone.MovementSpeed >= stoneSpeed)
                Destroy(gameObject);
        }
    }
}
