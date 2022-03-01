using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Saw : Structure
{
    private Rigidbody2D rb;

    [SerializeField] private float movementMax;

    [SerializeField] private float speed;
    [SerializeField] private float maxSpeed;
    [SerializeField] private float damage;
    [SerializeField] private float damageDelay;

    private bool onSaw;

    private float direction;

    private Vector3 startPoint;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        startPoint = transform.position;
        MovementSpeed = speed;
        MaxMovementSpeed = maxSpeed;
        direction = 1.0f;
        rb.velocity = new Vector3(MovementSpeed, 0, 0);
        ConnectValue(Define.ChangableValue.Speed, typeof(Structure).GetProperty("MaxMovementSpeed"), typeof(Structure).GetProperty("MovementSpeed"));
        base.Start();
    }

    private void Update()
    {
        if (transform.position.x - startPoint.x >= movementMax)
            direction = -1.0f;
        else if (startPoint.x - transform.position.x >= movementMax)
            direction = 1.0f;
        rb.velocity = new Vector3(MovementSpeed*direction, 0, 0);
    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            StartCoroutine(DamageToPlayer());
            onSaw = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            StopCoroutine(DamageToPlayer());
            onSaw = false;
        }
    }

    IEnumerator DamageToPlayer()
    {
        while (onSaw)
        {
            GameManager.Instance.GetDamaged(damage);
            yield return new WaitForSeconds(damageDelay);
        }
    }
}
