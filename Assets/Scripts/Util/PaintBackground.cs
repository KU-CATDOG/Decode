using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaintBackground : MonoBehaviour
{
    private Collider2D coll;

    private void OnEnable()
    {
        coll = GetComponent<BoxCollider2D>();
    }


}
