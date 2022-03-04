using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Thorn : Structure
{
    [SerializeField] private float damage;

    [SerializeField] private float damageDelay;

    [SerializeField] private float hp;

    private bool onThorn;

    private void Start()
    {
        MaxHealth = Health = hp;
        ConnectValue(Define.ChangableValue.Hp, typeof(Structure).GetProperty("MaxHealth"), typeof(Structure).GetProperty("Health"));
        base.Start();
    }

    private void Update()
    {
        if (Health <= 0f)
        {
            for (int i = 0; i < barofChangableValues.Length; i++)
            {
                Destroy(barofChangableValues[i].gameObject);
            }
            Destroy(gameObject);

        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            onThorn = true;
            StartCoroutine(DamageToPlayer());
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            StopCoroutine(DamageToPlayer());
            onThorn = false;
        }
    }

    IEnumerator DamageToPlayer()
    {
        while(onThorn)
        {
            GameManager.Instance.GetDamaged(damage);
            yield return new WaitForSeconds(damageDelay);
        }
    }
}
