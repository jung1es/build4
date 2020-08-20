using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class ShapeProperties : MonoBehaviour
{
    public enum AvilableColors { Blue, Red, Green, Yellow}
    public AvilableColors myColor;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("StackingArea"))
        {

        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("StackingArea"))
        {

        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        
    }

    private void OnCollisionExit(Collision collision)
    {
        
    }

}
