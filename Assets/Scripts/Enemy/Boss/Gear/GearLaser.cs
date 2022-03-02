using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GearLaser : MonoBehaviour
{
    private Gear gear;
    private void Awake()
    {
        gear = transform.parent.GetComponent<Gear>();
    }
    private void OnEnable()
    {
        transform.localScale = new Vector3(transform.localScale.x, 0.1f, transform.localScale.z);
        StartCoroutine(RazerRoutine());
    }
    private IEnumerator RazerRoutine()
    {
        yield return new WaitUntil(() =>
        {
            transform.localScale += new Vector3(0, 0.05f, 0) * (1 + gear.MovementSpeed / 50f);
            return transform.localScale.y >= 3;
        });
        yield return new WaitForSeconds(1.0f / (1 + gear.MovementSpeed / 50f));
        yield return new WaitUntil(() =>
        {
            transform.localScale -= new Vector3(0, 0.05f, 0) * (1 + gear.MovementSpeed / 50f);
            return transform.localScale.y <= 0.1f;
        });
        gameObject.SetActive(false);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            GameManager.Instance.GetDamaged(10);
        }
    }
}
