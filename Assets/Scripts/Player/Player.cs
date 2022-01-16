using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

    private float horizontal;
    private bool isJumpKeyDown = false;
    private bool isControllable = true;

    [SerializeField]
    private LayerMask jumpable;
    private Rigidbody2D rb;
    private Collider2D col;

    [SerializeField]
    private float speed = 5.0f;
    [SerializeField]
    private float jumpSpeed = 7f;
    public float health;
    private float maxHealth = 10f;
    private bool isInvincible = false;

    private float _coolTime = 1f;
    private bool coolDown = false;

    private void OnEnable()
    {
        InputManager.Instance.OnLeftKey += Move;
        InputManager.Instance.OnRightKey += Move;
        InputManager.Instance.OnJumpKeyDown += () => { Jump(); isJumpKeyDown = true; } ;
        InputManager.Instance.OnSpaceKeyDown += Roll;

        InputManager.Instance.OnLeftKeyUp += () => { horizontal = 0; };
        InputManager.Instance.OnRightKeyUp += () => { horizontal = 0; };
        InputManager.Instance.OnJumpKeyUp += () => { isJumpKeyDown = false; };

        InputManager.Instance.MouseAction += Attack;

    }

    private void OnDisable()
    {
        InputManager.Instance.OnLeftKey -= Move;
        InputManager.Instance.OnRightKey -= Move;
        InputManager.Instance.OnJumpKeyDown -= () => { Jump(); isJumpKeyDown = true; };
        InputManager.Instance.OnSpaceKeyDown -= Roll;

        InputManager.Instance.OnLeftKeyUp -= () => { horizontal = 0; };
        InputManager.Instance.OnRightKeyUp -= () => { horizontal = 0; };
        InputManager.Instance.OnJumpKeyUp -= () => { isJumpKeyDown = false; };

        InputManager.Instance.MouseAction -= Attack;
    }


    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<Collider2D>();
        health = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        if (isControllable)
        {
            rb.velocity = new Vector2(horizontal * speed, rb.velocity.y);
        }
        if (rb.velocity.y < 0)
        {
            rb.velocity += Vector2.up * Physics2D.gravity.y * 1.5f * Time.deltaTime;
        }
        else if (rb.velocity.y > 0 && !isJumpKeyDown)
        {
            rb.velocity += Vector2.up * Physics2D.gravity.y * 0.5f * Time.deltaTime;
        }
    }

    public void SetPlayerControllable(bool _isControllable)
    {
        isControllable = _isControllable;

        if (!isControllable)
        {
            horizontal = 0;
        }
    }

    private bool IsGrounded()
    {
        Vector3 bottom = col.bounds.center - new Vector3(0, col.bounds.size.y / 2);
        bool isGrounded = Physics2D.Raycast(bottom, Vector2.down, 0.1f, jumpable);
        return rb.velocity.y < 0.01f && isGrounded;
    }

    private void Move(Vector2 direction)
    {
        if (isControllable)
        {
            horizontal = direction.x;
        }
    }

    private void Jump()
    {
        if(isControllable && IsGrounded())
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpSpeed);
        }
    }

    private void Roll()
    {
        
    }

    private void Attack(Define.MouseEvent evt)
    {
        if(evt == Define.MouseEvent.LClick && !coolDown)    // 마우스 좌클릭
        {
            Debug.Log("LClick");
            StartCoroutine(coolTime(_coolTime));
        }
        else if(evt == Define.MouseEvent.RClick)            // 마우스 우클릭
        {
            Debug.Log("RClick");

        }


    }

    IEnumerator coolTime(float time)
    {
        //InputManager.Instance.MouseAction -= Attack;
        coolDown = true;
        yield return new WaitForSeconds(time);
        //InputManager.Instance.MouseAction += Attack;
        coolDown = false;
    }

    public void GetDamaged(float damage)
    {
        if (!isInvincible)
        {
            health -= damage;
            Debug.Log("Player dmg taken: " + damage);

        }
        if (health <= 0)
        {
            Debug.Log("Player Dead");
            //gameObject.SetActive(false);
        }

    }

}
