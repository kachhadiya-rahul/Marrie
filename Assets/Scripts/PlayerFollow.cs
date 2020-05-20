using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFollow : MonoBehaviour
{

    Vector3 cameraOffset;
    public Transform LookAt;
    // Start is called before the first frame update
    void Start()
    {
        float xOff =  (Screen.width * 0.1f) + Camera.main.WorldToScreenPoint(LookAt.position).x;
        float yOff = Screen.height / 2;

        cameraOffset = Camera.main.ScreenToWorldPoint(new Vector3(xOff, yOff));
        cameraOffset.z = -10;

    }

    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector3(LookAt.position.x + Mathf.Abs(cameraOffset.x), 0, -10);
        //LookAt.position + new Vector3(Mathf.Abs(cameraOffset.x),0,-10);
    }
}
