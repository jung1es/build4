using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class poc6 : MonoBehaviour
{
    public Vector3 dist;
    public Vector3 range, playerPos;

    bool canControll;
    // Update is called once per frame
    void Update()
    {

        dist = transform.position - playerPos;

        if (Mathf.Abs(dist.x) < range.x && Mathf.Abs(dist.z) < range.z && Mathf.Abs(dist.y) < range.y)
        {
            // if yes then the player is able to move it else you can't
            canControll = true;
        }
        else
        {
            canControll = false;
        }




    }


}
