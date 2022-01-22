using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Range : Enemy
{
    [SerializeField]
    private GameObject bullet;
    private float projectileSpeed = 3.0f;
    protected override void Start()
    {
        base.Start();
        MaxHealth = Health = 5f;
        AttackDamage = 7f;
        MovementSpeed = 0.7f;
        Range = 15f;
        Interval = 3.0f;
    }

    protected override Queue<IEnumerator> DecideNextRoutine()
    {
        Queue<IEnumerator> nextRoutines = new Queue<IEnumerator>();

        if (CheckPlayer())
        {
            if (DistToPlayer() < Range)
            {
                nextRoutines.Enqueue(NewActionRoutine(ShootRoutine(AttackDamage)));
            }
            else
            {
                nextRoutines.Enqueue(NewActionRoutine(MoveTowardPlayer(3.0f)));
            }
        }
        else nextRoutines.Enqueue(NewActionRoutine(WaitRoutine(1f)));

        return nextRoutines;
    }

    private IEnumerator ShootRoutine(float dmg)
    {
        if (CheckPlayer())
        {
            var bul = Instantiate(bullet, transform.position, Quaternion.identity);
            Bullet temp = bul.GetComponent<Bullet>();
            temp.host = gameObject;
            temp.dmg = AttackDamage;
            bul.GetComponent<Rigidbody2D>().velocity = (GetPlayerPos() - GetObjectPos()).normalized * projectileSpeed;
            yield return new WaitForSeconds(Interval);
        }
        else yield return null;
    }
    

}
