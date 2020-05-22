using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSpiral : MonoBehaviour
{

    public float speed = 1;
    public float radius = 1;
    public Transform target;

    float initZ;
    // Start is called before the first frame update
    void Start()
    {
        initZ = transform.position.z;
    }

    // Update is called once per frame
    void Update()
    {
        var x = -Mathf.Sin(Time.time * speed) * radius;
        var y = -Mathf.Cos(Time.time * speed) * radius;
        var z = initZ + Mathf.Cos(Time.time * speed * 0.5f) * radius;
        transform.position = new Vector3(x, y, z);
        transform.LookAt(target);
    }
}
