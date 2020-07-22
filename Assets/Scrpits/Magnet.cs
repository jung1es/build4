using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Magnet : MonoBehaviour
{
    bool north, south;
    void Update()
    {
        RaycastHit hit;
        north = Physics.Raycast(transform.position, transform.TransformDirection(Vector3.up), out hit, transform.localScale.x / 2 * 1.5f);
        south = Physics.Raycast(transform.position, transform.TransformDirection(Vector3.down), out hit, transform.localScale.x / 2 * 1.5f);
        //check down
        if (north)
        {
            Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.right) * hit.distance, Color.black);
        }
    }
}
