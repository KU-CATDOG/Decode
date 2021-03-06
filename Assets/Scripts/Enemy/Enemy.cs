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
    public float MP { get; protected set; }
    public float MaxMP { get; protected set; }
    public float MovementSpeed { get; protected set; }
    public float MaxMovementSpeed { get; protected set; }
    public float Range { get; protected set; }
    public float Eyesight { get; protected set; }
    public float Interval { get; protected set; }

    protected Player player;
    protected Rigidbody2D rb;

    public bool onHit;

    protected virtual void Start()
    {
        InitializeBars();
        player = FindObjectOfType<Player>();
        rb = GetComponent<Rigidbody2D>();

        //FIXME
        //Physics2D.IgnoreCollision(player.GetComponent<Collider2D>(), GetComponent<Collider2D>());
        Physics2D.IgnoreLayerCollision(8, 8);
        Physics2D.IgnoreLayerCollision(7, 8);

    }
    protected virtual void Update()
    {
        if (CurrentRoutine == null)
        {
            NextRoutine();
        }
    }
    protected void NextRoutine()
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
    }
    protected IEnumerator MoveRoutine(Vector3 destination, float time) // ?????? ?????? ????????
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
    protected IEnumerator MoveTowardPlayer(float speedMultiplier)     // ?????????? ???? ????????
    {
        Vector2 direction = (GetPlayerPos() - GetObjectPos()).normalized;
        rb.MovePosition(rb.position + direction * speedMultiplier * Time.fixedDeltaTime);
        yield return null;
    }
    protected IEnumerator MoveTowardPlayerHorizontal(float speedMultiplier)     // ?????????? ???? ????????
    {
        Vector2 direction = (GetPlayerPos() - GetObjectPos()).normalized;
        direction.y = 0;
        rb.MovePosition(rb.position + direction * speedMultiplier * Time.fixedDeltaTime);
        yield return null;
        //Debug.Log(1);

    }
    protected IEnumerator WaitRoutine(float time)
    {
        yield return new WaitForSeconds(time);
    }

    protected Vector3 GetObjectPos()    // ???????? ????3 ????
    {
        return gameObject.transform.position;
    }
    protected Vector3 GetPlayerPos()    // ???????? ????3 ????; ???? ?????????? ??????????
    {
        return player.transform.position;
    }
    protected float DistToPlayer()
    {
        return Vector2.Distance(gameObject.transform.position, player.transform.position);
        //return Vector3.Distance(GetObjectPos(), GetPlayerPos());
    }
    protected bool CheckPlayer()
    {
        if (FindObjectOfType<Player>() != null) return true;
        else return false;
    }
    
    
}
