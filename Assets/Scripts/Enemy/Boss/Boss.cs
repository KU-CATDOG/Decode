using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Boss : Enemy
{
    [SerializeField]
    private GameObject Exit;
    [SerializeField]
    private BoxCollider2D clearedBound;
    private GameObject bounds;
    protected override void Start()
    {
        InitializeBars(true);
        player = FindObjectOfType<Player>();
        rb = GetComponent<Rigidbody2D>();
        bounds = GameObject.Find("Bounds");
        //FIXME
        //Physics2D.IgnoreCollision(player.GetComponent<Collider2D>(), GetComponent<Collider2D>());
        Physics2D.IgnoreLayerCollision(8, 8);
        Physics2D.IgnoreLayerCollision(7, 8);
        Vector2 temp = clearedBound.offset;
        clearedBound.offset = temp;
    }

    protected override void Update()
    {
        if (CurrentRoutine == null)
        {
            NextRoutine();
        }

        if (Health <= 0f)
        {
            for (int i = 0; i < barofChangableValues.Length; i++)
            {
                Destroy(barofChangableValues[i].gameObject);
            }
            Exit.SetActive(false);
            BoxCollider2D col = bounds.GetComponent<BoxCollider2D>();
            bounds.transform.position = new Vector3(clearedBound.offset.x, clearedBound.offset.y, 10);
            col.size = clearedBound.size;
            Camera.main.GetComponent<CameraController>().UpdateCollider();
            Destroy(gameObject);
        }
    }

    protected override void Die()
    {
        Exit.SetActive(false);
        GameManager.Instance.player.health = GameManager.Instance.player.maxHealth;
        Camera.main.GetComponent<CameraController>().bounds = clearedBound;
        Camera.main.GetComponent<CameraController>().UpdateCollider();
        GetComponent<AuthorityUnlock>().Alarm();
        base.Die();
    }
}
