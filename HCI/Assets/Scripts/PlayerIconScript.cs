using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerIconScript : MonoBehaviour
{
    public GameObject firstCamera;
    Vector3 firstCameraPosition;
    // Start is called before the first frame update
    void Start()
    {
        firstCamera = Player.Instance.cam1;
        firstCameraPosition = new Vector3(firstCamera.transform.position.x, firstCamera.transform.position.y + 5
            , firstCamera.transform.position.z);
        transform.position = firstCameraPosition;
    }

    // Update is called once per frame
    void Update()
    {
        firstCameraPosition = new Vector3(firstCamera.transform.position.x, transform.position.y, firstCamera.transform.position.z);
        transform.position = firstCameraPosition;
    }
}
