using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MpDoor : Enemy
{
    protected override void Start()
    {
        MaxHealth = Health = 1000f;
        MaxMP = MP = 10f;
        ConnectValue(Define.ChangableValue.Hp, typeof(Enemy).GetProperty("MaxHealth"), typeof(Enemy).GetProperty("Health"));
        ConnectValue(Define.ChangableValue.Mp, typeof(Enemy).GetProperty("MaxMP"), typeof(Enemy).GetProperty("MP"));
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