using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MpDoor : Enemy
{
    protected override void Start()
    {
        base.Start();
        MaxHealth = Health = 1000f;
        MaxMP = MP = 10f;
        changableValues = new Define.ChangableValue[2] { Define.ChangableValue.Hp, Define.ChangableValue.Mp };
        dict[Define.ChangableValue.Hp] = typeof(Enemy).GetProperty("Health");
        dict[Define.ChangableValue.Mp] = typeof(Enemy).GetProperty("MP");
    }

    protected override Queue<IEnumerator> DecideNextRoutine()
    {
        Queue<IEnumerator> nextRoutines = new Queue<IEnumerator>();

        return nextRoutines;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (MP <= 0)
        {
            Destroy(gameObject);
        }
    }
}