using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class offsetPoint : MonoBehaviour
{

    public Transform parent;
    public float offset;

    // Update is called once per frame
    void Update()
    {
        transform.localPosition = new Vector3(0,0, offset + ( .6f / (parent.rotation.y) ));
        //transform.Translate(Vector3.forward * (1/parent.rotation.y) * offset);
    }
}
