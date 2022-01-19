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
    [SerializeField]
    private float rollSpeed = 7f;
    private bool isInvincible = false;
    private bool isPlayerLookRight = true;

    private Weapon weapon;

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
        weapon = GetComponentInChildren<Weapon>();
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<Collider2D>();
        health = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 cursorDir = InputManager.Instance.CursorPos - transform.position;
        float cursorAngle = Mathf.Atan2(cursorDir.y, cursorDir.x) * Mathf.Rad2Deg;
        bool isCursorRight = cursorAngle < 90 && cursorAngle > -90;
        PlayerLookAt(isCursorRight);
        Debug.Log(isPlayerLookRight);


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
        if (isControllable)
        {
            StartCoroutine(RollRoutine(0.5f, horizontal != 0 ? (horizontal > 0) : isPlayerLookRight));
        }
    }

    IEnumerator RollRoutine(float time, bool rollDir)
    {
        SetPlayerControllable(false);
        Debug.Log("Roll");
        isInvincible = true;

        for (float t = 0; t < time; t += Time.deltaTime)
        {
            rb.velocity = new Vector2(rollSpeed * (rollDir ? 1 : -1), rb.velocity.y);
            yield return null;
        }
        SetPlayerControllable(true);
        isInvincible = false;
    }

    private void PlayerLookAt(bool isRight)
    {
        isPlayerLookRight = isRight;
    }


    private void Attack(Define.MouseEvent evt)
    {
        StartCoroutine(weapon.AttackRoutine(evt));
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