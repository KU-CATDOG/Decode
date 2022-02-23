using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : Enemy
{
    [SerializeField]
    private GameObject laserCollider;
    [SerializeField]
    private LineRenderer lr;
    const float laserReadyWidth = 0.1f;
    const float laserShootWidth = 1.0f;

    private bool chargeMP = false;
    private float timer = 0f;
    private bool trigger = false;
    private bool shooting = false;
    protected override void Start()
    {
        MaxHealth = Health = 5f;
        MP = 0f;
        MaxMP = 10f;
        AttackDamage = 7f;
        MovementSpeed = 0.7f;
        Range = 15f;        // 공격 범위
        Interval = 3.0f;
        Eyesight = 20f;

        ConnectValue(Define.ChangableValue.Mp, typeof(Enemy).GetProperty("MaxMP"), typeof(Enemy).GetProperty("MP"));
        ConnectValue(Define.ChangableValue.Hp, typeof(Enemy).GetProperty("MaxHealth"), typeof(Enemy).GetProperty("Health"));

        base.Start();

    }
    private void FixedUpdate()  //FIXME
    {
        if (chargeMP)
        {
            timer += Time.deltaTime;
            if (timer >= 1.0f && MP < MaxMP)
            {
                MP += 10f;
                timer = 0f;
                //Debug.Log(MP);
            }
            if (MP >= MaxMP) trigger = true;
        }
    }

    protected override Queue<IEnumerator> DecideNextRoutine()
    {
        Queue<IEnumerator> nextRoutines = new Queue<IEnumerator>();

        if (CheckPlayer())
        {

            if (trigger)
            {
                trigger = false;
                nextRoutines.Enqueue(NewActionRoutine(LaserRoutine(Interval)));
                MP = 0f;
            }

            if (DistToPlayer() < Eyesight)
            {
 
                Vector3 dir = GetPlayerPos() - transform.position;
                float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
                bool isLookRight = angle < 90 && angle > -90;
                GetComponent<SpriteRenderer>().flipX = !isLookRight;
                

                if (DistToPlayer() < Range)
                {
                    if (!shooting) chargeMP = true;
                    nextRoutines.Enqueue(NewActionRoutine(WaitRoutine(1f)));
                }
                else
                {
                    chargeMP = false;
                    if (GetComponent<Rigidbody2D>().velocity.y >= 0)
                        nextRoutines.Enqueue(NewActionRoutine(MoveTowardPlayer(MovementSpeed)));
                    else nextRoutines.Enqueue(NewActionRoutine(WaitRoutine(1f)));
                }

            }
            else nextRoutines.Enqueue(NewActionRoutine(WaitRoutine(1f)));



        }
        else nextRoutines.Enqueue(NewActionRoutine(WaitRoutine(1f)));

        return nextRoutines;
    }

    private IEnumerator LaserRoutine(float chargeTime)
    {
        shooting = true;
        chargeMP = false;

        lr.enabled = true;

        Vector3 playerPos = GetPlayerPos();
        Vector3 shootPos = transform.position;
        shootPos.y += 0.25f;

        lr.startColor = Color.red;
        lr.endColor = Color.red;

        lr.SetPosition(0, shootPos);
        lr.SetPosition(1, playerPos);

        for(float t = 0; t < chargeTime; t += Time.deltaTime)
        {
            Vector3 dir = GetPlayerPos() - transform.position;
            float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            bool isLookRight = angle < 90 && angle > -90;
            GetComponent<SpriteRenderer>().flipX = !isLookRight;

            playerPos = GetPlayerPos();
            lr.SetPosition(0, shootPos);
            lr.SetPosition(1, playerPos);
            yield return null;
        }

        laserCollider.transform.position = (new Vector3(playerPos.x, playerPos.y, 0) + shootPos) / 2;
        Vector3 diff = new Vector3(playerPos.x, playerPos.y, 0) - shootPos;
        laserCollider.transform.rotation = Quaternion.Euler(0, 0, Mathf.Atan2(diff.y, diff.x) * Mathf.Rad2Deg);
        laserCollider.transform.localScale = new Vector2(diff.magnitude, laserShootWidth);
        laserCollider.SetActive(true);

        lr.startColor = Color.yellow;
        lr.endColor = Color.yellow;

        lr.startWidth = laserShootWidth;
        lr.endWidth = laserShootWidth;

        yield return new WaitForSeconds(0.5f);

        laserCollider.SetActive(false);

        for (float t = 0; t < 0.2f; t += Time.deltaTime)
        {
            float size = Mathf.Lerp(laserShootWidth, laserReadyWidth, t / 0.2f);
            lr.startWidth = size;
            lr.endWidth = size;
            yield return null;
        }

        lr.startWidth = laserReadyWidth;
        lr.endWidth = laserReadyWidth;
        lr.enabled = false;

        shooting = false;

        yield return null;


    }


}

