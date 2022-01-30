using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HpDoor : Enemy
{
    protected override void Start()
    {
        MaxHealth = Health = 10f;
        ConnectValue(Define.ChangableValue.Hp, typeof(Enemy).GetProperty("MaxHealth"), typeof(Enemy).GetProperty("Health"));
        base.Start();
    }

    protected override Queue<IEnumerator> DecideNextRoutine()
    {
        Queue<IEnumerator> nextRoutines = new Queue<IEnumerator>();

        return nextRoutines;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log(Health);
        if (Health <= 0)
        {
            Destroy(gameObject);
        }
    }
}