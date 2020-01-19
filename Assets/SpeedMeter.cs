using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedMeter : MonoBehaviour
{
    public Vector3 lastPos;
    public Vector3 velocity = Vector3.zero;
    // Start is called before the first frame update
    void Start()
    {
        lastPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        velocity *= 0.2f;
        velocity += ((transform.position - lastPos)/Time.deltaTime) * 0.8f;
        lastPos = transform.position;
    }
}
