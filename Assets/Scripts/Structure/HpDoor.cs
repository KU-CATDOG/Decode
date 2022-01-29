using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HpDoor : Enemy
{
    protected override void Start()
    {
        base.Start();
        MaxHealth = Health = 10f;
        changableValues = new Define.ChangableValue[1] { Define.ChangableValue.Hp };
        dict[Define.ChangableValue.Hp] = typeof(Enemy).GetProperty("Health");
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