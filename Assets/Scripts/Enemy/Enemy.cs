using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy : Changable
{
    protected Coroutine CurrentRoutine { get; private set; }
    private Queue<IEnumerator> nextRoutines = new Queue<IEnumerator>();
    
    public float Health { get; protected set; }
    public float MaxHealth { get; protected set; }
    public float AttackDamage { get; protected set; }
    public float MovementSpeed { get; protected set; }
    public float Range { get; protected set; }
    public float Interval { get; protected set; }

    protected Player player;
    protected Rigidbody2D rb;

    protected virtual void Start()
    {
        changableValues = new Define.ChangableValue[2] { Define.ChangableValue.Hp, Define.ChangableValue.Speed};
        dict[Define.ChangableValue.Hp] = typeof(Enemy).GetProperty("Health");
        dict[Define.ChangableValue.Speed] = typeof(Enemy).GetProperty("MovementSpeed");
        player = FindObjectOfType<Player>();
        rb = GetComponent<Rigidbody2D>();
    }
    private void Update()
    {
        if (CurrentRoutine == null)
        {
            NextRoutine();
        }
    }
    private void NextRoutine()
    {
        if (nextRoutines.Count <= 0)
        {
            nextRoutines = DecideNextRoutine();
        }
        StartCoroutineUnit(nextRoutines.Dequeue());
    }
    protected abstract Queue<IEnumerator> DecideNextRoutine();
    private void StartCoroutineUnit(IEnumerator coroutine)
    {
        if (CurrentRoutine != null)
        {
            StopCoroutine(CurrentRoutine);
        }
        CurrentRoutine = StartCoroutine(coroutine);
    }
    protected IEnumerator NewActionRoutine(IEnumerator action)
    {
        yield return action;
        CurrentRoutine = null;
    }/*
    public virtual void GetDamaged(float damage)
    {
        Health -= damage;
        Debug.Log("Enemy dmg taken: " + damage);
        if (Health <= 0)
        {
            Destroy(gameObject);
        }
    }*/
    protected IEnumerator MoveRoutine(Vector3 destination, float time) // 지정된 좌표로 움직인다
    {
        yield return MoveRoutine(destination, time, AnimationCurve.Linear(0, 0, 1, 1));
    }
    protected IEnumerator MoveRoutine(Vector3 destination, float time, AnimationCurve curve)
    {
        Vector3 startPosition = transform.position;
        for (float t = 0; t <= time; t += Time.deltaTime)
        {
            transform.position =
                Vector3.Lerp(startPosition, destination, curve.Evaluate(t / time));
            yield return null;
        }
        transform.position = destination;
    }
    protected IEnumerator MoveTowardPlayer(float speedMultiplier)     // 플레이어를 향해 움직인다
    {
        Vector2 direction = (GetPlayerPos() - GetObjectPos()).normalized;
        rb.MovePosition(rb.position + direction * speedMultiplier * Time.fixedDeltaTime);
        yield return null;
    }
    protected IEnumerator WaitRoutine(float time)
    {
        yield return new WaitForSeconds(time);
    }

    protected Vector3 GetObjectPos()    // 오브젝트 벡터3 반환
    {
        return gameObject.transform.position;
    }
    protected Vector3 GetPlayerPos()    // 플레이어 벡터3 반환; 먼저 살아있는지 확인해야함
    {
        return player.transform.position;
    }
    protected float DistToPlayer()
    {
        return Vector3.Distance(GetObjectPos(), GetPlayerPos());
    }
    protected bool CheckPlayer()
    {
        if (FindObjectOfType<Player>() != null) return true;
        else return false;
    }
}
