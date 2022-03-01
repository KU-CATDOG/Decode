using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Escalator : Structure
{

    private Rigidbody2D rb;
    private GameObject player;

    [SerializeField] private float speed;
    [SerializeField] private float maxSpeed;
    [SerializeField] private float hp;

    [SerializeField] private bool moveToLeft;

    private float direction;
    public bool underPlayer;

    void Start()
    {
        underPlayer = false;
        player = GameObject.Find("Player");
        rb = GetComponent<Rigidbody2D>();
        MaxHealth = Health = hp;
        MovementSpeed = speed;
        MaxMovementSpeed = maxSpeed;
        rb.velocity = new Vector3(MovementSpeed, 0, 0);
        ConnectValue(Define.ChangableValue.Hp, typeof(Structure).GetProperty("MaxHealth"), typeof(Structure).GetProperty("Health"));
        ConnectValue(Define.ChangableValue.Speed, typeof(Structure).GetProperty("MaxMovementSpeed"), typeof(Structure).GetProperty("MovementSpeed"));
        base.Start();

        if (moveToLeft)
            direction = -1.0f;
        else
            direction = 1.0f;
    }

    // Update is called once per frame
    void Update()
    {
        if (Health <= 0f)
        {
            for (int i = 0; i < barofChangableValues.Length; i++)
            {
                Destroy(barofChangableValues[i].gameObject);
            }
            Destroy(gameObject);
        }

        Vector3 playerCurrentPosition = player.transform.position;

        if(underPlayer)
            player.transform.position = new Vector3(playerCurrentPosition.x + (Time.deltaTime * direction * MovementSpeed * Mathf.Cos((transform.eulerAngles.z) * Mathf.PI / 180)), playerCurrentPosition.y + (Time.deltaTime * direction * MovementSpeed * Mathf.Sin((transform.eulerAngles.z)*Mathf.PI/180)), playerCurrentPosition.z);
    }

}
