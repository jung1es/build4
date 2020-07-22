using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rotate : MonoBehaviour
{
    public Transform obj;
    public Vector3 rot;
    public float t;

    void Update()
    {
        transform.RotateAround(obj.position, rot, t);
    }
}
