using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerObjectsArea : MonoBehaviour
{
    public int LayerIndex;
    public LayerMask playerMask;
    public bool isPOC2 = false;
    private void OnTriggerEnter(Collider collision)
    {
        if (collision.transform.CompareTag("Staks"))
        {
            if (isPOC2)
            {
                collision.gameObject.GetComponent<DraggingObs>().SetPlayerArea(gameObject);
            }
            else
            {
                collision.gameObject.layer = LayerIndex;
            }
        }
        if (collision.transform.CompareTag("Player"))
        {
            collision.transform.GetComponent<Spectator>().cams.GetComponent<Camera>().cullingMask = playerMask;
            if (isPOC2) 
            {
                collision.transform.GetComponent<PlayerController>().SetMyArea(gameObject);
            }
            else
            {
                collision.transform.GetComponent<Spectator>().cams.GetComponent<Camera>().cullingMask = playerMask;
            }
        }
    }

    private void OnTriggerExit(Collider collision)
    {
        if (collision.transform.CompareTag("Staks"))
        {
            collision.gameObject.layer = 0;
            collision.gameObject.GetComponent<DraggingObs>().SetPlayerArea(null);
        }
    }
   
}
