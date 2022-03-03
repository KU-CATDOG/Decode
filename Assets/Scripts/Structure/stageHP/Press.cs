using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Press : MonoBehaviour
{
    private Rigidbody2D rbMid;
    private Rigidbody2D rbBottom;
    private BoxCollider2D colBottom;
    private float originYPos;
    [SerializeField]
    private float movDistance;
    [SerializeField]
    private float delay;
    [SerializeField]
    private float velocity;
    private void Awake()
    {
        rbMid = transform.GetChild(1).GetComponent<Rigidbody2D>();
        rbBottom = transform.GetChild(2).GetComponent<Rigidbody2D>();
        colBottom = rbBottom.GetComponent<BoxCollider2D>();
        originYPos = rbBottom.position.y;
    }
    private void Start()
    {
        StartCoroutine(PressRoutine());
    }

    private IEnumerator PressRoutine()
    {
        while (true)
        {

            colBottom.enabled = true;
            rbBottom.velocity = Vector2.down * velocity*2;
            rbMid.velocity = Vector2.down * velocity ;
            yield return new WaitUntil(() =>
            {
                rbMid.transform.localScale = new Vector2(1, rbMid.transform.localPosition.y * -4.2f + 1);
                return originYPos - rbBottom.position.y >= movDistance;
            });
            rbBottom.position = new Vector2(rbBottom.position.x, originYPos - movDistance);
            rbBottom.velocity = Vector2.zero;
            rbMid.velocity = Vector2.zero;
            yield return new WaitForSeconds(delay);
            colBottom.enabled = false;
            rbBottom.velocity = Vector2.up * velocity;
            rbMid.velocity = Vector2.up * velocity / 2f;
            yield return new WaitUntil(() =>
            {
                rbMid.transform.localScale = new Vector2(1, rbMid.transform.localPosition.y * -4.2f + 1);
                return originYPos <= rbBottom.position.y;
            });
            rbBottom.position = new Vector2(rbBottom.position.x, originYPos);
            rbBottom.velocity = Vector2.zero;
            rbMid.velocity = Vector2.zero;
            yield return new WaitForSeconds(delay);
        }
    }

}
