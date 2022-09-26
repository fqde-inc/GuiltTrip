using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PivotScreen : MonoBehaviour
{

    public Transform target;
    public float speed; 
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.LookAt(target, Vector3.up);
        transform.rotation = new Quaternion(0, transform.rotation.y, 0, transform.rotation.w);
    }
}
