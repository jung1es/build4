using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class lookatcam : MonoBehaviour
{
    public GameObject Go;
    void Update()
    {
        transform.LookAt(transform.position + Go.transform.forward);
    }

}
