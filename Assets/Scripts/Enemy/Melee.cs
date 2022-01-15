using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Melee : Enemy
{
    protected override void Start()
    {
        base.Start();
        MaxHealth = Health = 10f;
        AttackDamage = 5f;
        MovementSpeed = 1f;
        Range = 5f;
    }

    protected override Queue<IEnumerator> DecideNextRoutine()
    {
        Queue<IEnumerator> nextRoutines = new Queue<IEnumerator>();

        if (FindObjectOfType<Player>() != null)
        {
            nextRoutines.Enqueue(NewActionRoutine(MoveTowardPlayer(3.0f)));
            //nextRoutines.Enqueue(NewActionRoutine(WaitRoutine(1f)));
        }
        else nextRoutines.Enqueue(NewActionRoutine(WaitRoutine(1f)));

        return nextRoutines;
    }
}
