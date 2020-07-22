using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectHilighting : MonoBehaviour
{
    
    RaycastHit hit;
    private Camera myCamera;
    // Start is called before the first frame update
    void Start()
    {
        myCamera = GetComponent<Camera>();
    }
    GameObject lastLooking;
    // Update is called once per frame
    void Update()
    {
        Ray ray = myCamera.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit))
        {
            if (hit.transform.CompareTag("Staks"))
            {
                if (hit.transform.gameObject != lastLooking)
                {
                    if (lastLooking != null)
                    {
                        lastLooking.GetComponent<Outline>().enabled = false;
                    }
                    lastLooking = hit.transform.gameObject;
                    lastLooking.GetComponent<Outline>().enabled = true;
                }
            }
            else
            {
               // lastLooking.GetComponent<Outline>().enabled = false;
            }
        }
    }

}
