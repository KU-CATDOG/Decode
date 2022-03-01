using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaintBackground : MonoBehaviour
{
    public SpriteRenderer spRenderer;
    private Collider2D coll;

    private void Start()
    {
        coll = GetComponent<BoxCollider2D>();
        spRenderer = GetComponent<SpriteRenderer>();

        transform.position = new Vector3(coll.offset.x, coll.offset.y,10);
        coll.offset = new Vector3(0, 0, 0);
        spRenderer.drawMode = SpriteDrawMode.Tiled;
        spRenderer.size = new Vector2(coll.bounds.extents.x*2,coll.bounds.extents.y*2);

    }
}
