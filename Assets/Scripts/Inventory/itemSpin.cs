using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class itemSpin : MonoBehaviour
{
    // Start is called before the first frame update
    public float spinSpeed = 6f;
    public float bob_speed = 0.001f;

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(new Vector3(0, spinSpeed*Time.deltaTime ,0));
        transform.position += new Vector3(0, bob_speed* Convert.ToSingle(Math.Sin(Time.time*1f)),0);
    }
}
