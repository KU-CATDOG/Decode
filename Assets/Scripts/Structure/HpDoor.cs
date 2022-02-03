using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HpDoor : Enemy
{
    protected override void Start()
    {
        MaxHealth = Health = 10f;
        changableValues = new Define.ChangableValue[1] { Define.ChangableValue.Hp };
        dict[Define.ChangableValue.Hp] = typeof(Enemy).GetProperty("Health");
        maxDict[Define.ChangableValue.Hp] = typeof(Enemy).GetProperty("MaxHealth");
        base.Start();

    }

    protected override Queue<IEnumerator> DecideNextRoutine()
    {
        Queue<IEnumerator> nextRoutines = new Queue<IEnumerator>();

        return nextRoutines;
    }

    private void Update()
    {
        if (Health <= 0)
        {
            Destroy(gameObject);
        }
    }
}