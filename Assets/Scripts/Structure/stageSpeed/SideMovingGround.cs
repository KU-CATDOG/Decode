using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SideMovingGround : Structure
{
    [SerializeField] private float movementMax;

    [SerializeField] private float speed;
    [SerializeField] private float maxSpeed;
    [SerializeField] private float mp;


    private float direction;

    private Vector3 startPoint;

    private void Start()
    {
        startPoint = transform.position;
        MaxMP = MP = mp;
        MovementSpeed = speed;
        MaxMovementSpeed = maxSpeed;
        direction = 1.0f;
        ConnectValue(Define.ChangableValue.Speed, typeof(Structure).GetProperty("MaxMovementSpeed"), typeof(Structure).GetProperty("MovementSpeed"));
        ConnectValue(Define.ChangableValue.Mp, typeof(Structure).GetProperty("MaxMP"), typeof(Structure).GetProperty("MP"));
        base.Start();
    }

    private void Update()
    {
        if (MP > 0)
        {
            Vector3 currentPosition = transform.position;


            if (transform.position.x - startPoint.x >= movementMax)
                direction = -1.0f;
            else if (startPoint.x - transform.position.x >= movementMax)
                direction = 1.0f;
            transform.position = new Vector3 (currentPosition.x + (Time.deltaTime * direction * MovementSpeed), currentPosition.y, currentPosition.z);
        }
    }

}
