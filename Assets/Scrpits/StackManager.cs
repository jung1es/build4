using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class StackManager : MonoBehaviour
{
    public GameObject[] stackingBlocks;
    public float[] rang;
    public TextMeshProUGUI txt;
    public int correctlyStacked;
    void Update()
    {

        for (int i = 0; i < stackingBlocks.Length; i++)
        {

            RaycastHit hit;
            //check down
            if (Physics.Raycast(stackingBlocks[i].transform.position, transform.TransformDirection(Vector3.up), out hit, rang[i]))
            {
                Debug.DrawRay(stackingBlocks[i].transform.position, transform.TransformDirection(Vector3.up) * hit.distance, Color.yellow);

                if (i < stackingBlocks.Length - 1)
                {
                    if (stackingBlocks[i + 1] != null)
                    {
                        if (hit.transform.gameObject == stackingBlocks[i + 1])
                        {
                            correctlyStacked++;
                        }
                    }
                }


            }
            else if (Physics.Raycast(stackingBlocks[i].transform.position, transform.TransformDirection(Vector3.down), out hit, rang[i]))
            {
                Debug.DrawRay(stackingBlocks[i].transform.position, transform.TransformDirection(Vector3.down) * hit.distance, Color.yellow);
                if (i > 0)
                {
                    if (stackingBlocks[i - 1] != null)
                    {
                        if (hit.transform.gameObject == stackingBlocks[i - 1])
                        {
                            correctlyStacked++;
                        }

                    }
                }


            }
            else
            {
                correctlyStacked = 0;
            }
        }

        if (correctlyStacked > stackingBlocks.Length)
        {
            txt.text = "Correct Placement";
        }
        else
        {
            txt.text = "Incorrect Placement";
        }




    }
}
