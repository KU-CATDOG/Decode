using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Press : MonoBehaviour
{
    public Rigidbody2D rb;

    public float timeDelay;
    public float upDownTime;

    private bool checkWall=true;
    public bool updown=true;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        StartCoroutine(MovePress());
    }


    IEnumerator MovePress()
    {
        while(updown)
        { 
        yield return new WaitForSeconds(timeDelay);
        rb.gravityScale = 1.0f;
        yield return new WaitForSeconds(upDownTime);
        rb.gravityScale = -1.0f;
        yield return new WaitForSeconds(upDownTime);
        }
    }

}
