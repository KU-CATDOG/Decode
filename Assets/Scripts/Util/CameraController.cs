using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{

    GameObject temp;

    public BoxCollider2D bounds;
    public Vector3 minBounds;
    public Vector3 maxBounds;

    public float halfHeight;
    public float halfWidth;

    private Camera cam;

    private GameObject player;

    public Vector3 target;
    // Start is called before the first frame update
    void Start()
    {
        //player = GameManager.Instance.player;

        temp = GameObject.Find("Bounds");
        player = GameObject.Find("Player");
        
        if(temp != null)
        {
            bounds = temp.GetComponent<BoxCollider2D>();
            minBounds = bounds.bounds.min;
            maxBounds = bounds.bounds.max;

        }

        cam = GetComponent<Camera>();
        halfHeight = cam.orthographicSize;
        halfWidth = halfHeight * Screen.width / Screen.height;

    }

    // Update is called once per frame
    void Update()
    {
        target = new Vector3(player.transform.position.x, player.transform.position.y, transform.position.z);
        transform.position = target;

        bool clamped = false;

        if (temp != null)
        {
            float clampedX = Mathf.Clamp(transform.position.x, minBounds.x + halfWidth, maxBounds.x - halfWidth);
            float clampedY = Mathf.Clamp(transform.position.y, minBounds.y + halfHeight, maxBounds.y - halfHeight);

            transform.position = new Vector3(clampedX, clampedY, transform.position.z);
        }

    }
}
