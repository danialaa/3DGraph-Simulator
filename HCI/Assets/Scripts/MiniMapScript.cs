using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniMapScript : MonoBehaviour
{
    public float scaleX = 40f;

    public float scaleY = 40f;
    Vector3 start;
    // Start is called before the first frame update
    void Start()
    {
        start = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            transform.localScale = new Vector3(2, 2, 1);
            transform.position = new Vector3(start.x + scaleX , start.y - scaleY, start.z);
        }
        if (Input.GetKeyDown(KeyCode.O))
        {
            transform.position = start;
            transform.localScale = new Vector3(1, 1, 1);
        }
    }
}
