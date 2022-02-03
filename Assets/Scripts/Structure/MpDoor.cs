using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MpDoor : Enemy
{
    protected override void Start()
    {
        MaxHealth = Health = 1000f;
        MaxMP = MP = 10f;
        changableValues = new Define.ChangableValue[2] { Define.ChangableValue.Hp, Define.ChangableValue.Mp };
        dict[Define.ChangableValue.Hp] = typeof(Enemy).GetProperty("Health");
        maxDict[Define.ChangableValue.Hp] = typeof(Enemy).GetProperty("MaxHealth");
        dict[Define.ChangableValue.Mp] = typeof(Enemy).GetProperty("MP");
        maxDict[Define.ChangableValue.Mp] = typeof(Enemy).GetProperty("MaxMP");
        base.Start();
    }

    protected override Queue<IEnumerator> DecideNextRoutine()
    {
        Queue<IEnumerator> nextRoutines = new Queue<IEnumerator>();

        return nextRoutines;
    }

    private void Update()
    {
        if (MP <= 0)
        {
            Destroy(gameObject);
        }
    }
}