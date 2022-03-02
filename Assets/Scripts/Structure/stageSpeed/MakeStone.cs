using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MakeStone : MonoBehaviour
{
    [SerializeField] private GameObject obj;
    [SerializeField] private float timeDelay;
    private GameObject stone;
    

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(MakingStone());
    }

    IEnumerator MakingStone()
    {
        Vector3 whereMakeStone = transform.position;
        while (true)
        {
            stone = Instantiate(obj, whereMakeStone, transform.rotation);
            yield return new WaitForSeconds(timeDelay);
            stone.GetComponent<Stone>().DestroyStone();
        }
    }
}
