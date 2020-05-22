using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateObj : MonoBehaviour
{
    public float speed = 1;
    public int direction = 0;

    private Vector3 axis;

    void Start()
    {
        if (direction == 0)
        {
            axis = Vector3.right;
        } else
        {
            axis = Vector3.up;
        }
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(axis * (speed * Time.deltaTime));
    }

}
