using UnityEngine;
using Photon.Pun;
using TMPro;
using com.PT.contest;
using System.Timers;
using Photon.Pun.Demo.Procedural;

public class Spectator : MonoBehaviourPun
{

    public float speed, minY, maxY;

    float rotX, rotY, xRotation;
    public GameObject cams;

    public int q = 0;

    public static GameObject selection;

    public GameObject[] objects;

    private void Start()
    {
        //cams.SetActive(photonView.IsMine);
        //        transform.LookAt(GameObject.FindGameObjectWithTag("centre").transform);

        if (GameObject.FindGameObjectWithTag("trapz"))
            selection = null;
        rotX = transform.rotation.x;
        rotY = transform.rotation.y;
    }
    
   
    // Update is called once per frame
    void Update()
    {
        /*if (!photonView.IsMine)
        {
            return;
        }*/

        if (GameObject.FindGameObjectWithTag("trapz"))
        {
            photonView.RPC("objsd", RpcTarget.All);

            foreach (GameObject obj in objects)
            {
                if (obj == selection) obj.GetComponent<poc2DRag>().selected = true;
                else obj.GetComponent<poc2DRag>().selected = false;
            }

            if (Input.GetKeyDown(KeyCode.Tab)) photonView.RPC("nextObj", RpcTarget.All);

            speed = 1;
            rotX += Input.GetAxis("Horizontal") * speed;
            rotY -= Input.GetAxis("Vertical") * speed;
            //rotY = Mathf.Clamp(rotY, minY, maxY);
            transform.rotation = Quaternion.Euler(rotY, rotX, 0);

            /*
            speed = 300;
            rotX += Input.GetAxis("Mouse X") * Time.deltaTime * speed;
            rotY -= Input.GetAxis("Mouse Y") * Time.deltaTime * speed;

            //rotY = Mathf.Clamp(rotY, minY, maxY);
            transform.rotation = Quaternion.Euler(rotY, rotX, 0);*/

        }
        else
        {
            speed = 20;
            if (!Manager.Instance.EnableMovementOnly)
            {
                if (Input.GetKey(KeyCode.A))
                    transform.Rotate(-Vector3.up * speed * Time.deltaTime,Space.World);

                if (Input.GetKey(KeyCode.D))
                    transform.Rotate(Vector3.up * speed * Time.deltaTime, Space.World);

                if (Input.GetKey(KeyCode.W))
                    transform.Rotate(Vector3.left * speed * Time.deltaTime);
                if (Input.GetKey(KeyCode.S))
                    transform.Rotate(-Vector3.left * speed * Time.deltaTime);
            }
            else
            {
                //if (Input.GetKey(KeyCode.A))
                //    transform.Translate(Vector3.left * speed /2* Time.deltaTime);

                //if (Input.GetKey(KeyCode.D))
                //    transform.Translate(-Vector3.left * speed /2* Time.deltaTime);

                if (Input.GetKey(KeyCode.E))
                {
                    transform.Rotate(Vector3.up, 40 * Time.deltaTime, Space.World);
                    // transform.RotateAround(Manager.Instance.transform.position, Vector3.up, 20 * Time.deltaTime);
                }

                if (Input.GetKey(KeyCode.Q))
                {
                    transform.Rotate(Vector3.up, -40 * Time.deltaTime, Space.World);
                    //transform.RotateAround(Manager.Instance.transform.position, Vector3.up, -20 * Time.deltaTime);
                }
                if (Input.GetKey(KeyCode.C))
                {
                    transform.Rotate(Vector3.forward, 40 * Time.deltaTime, Space.World);
                    // transform.RotateAround(Manager.Instance.transform.position, Vector3.up, 20 * Time.deltaTime);
                }

                if (Input.GetKey(KeyCode.Z))
                {
                    transform.Rotate(Vector3.forward, -40 * Time.deltaTime, Space.World);
                    //transform.RotateAround(Manager.Instance.transform.position, Vector3.up, -20 * Time.deltaTime);
                }
            }
        }


    }
    
    [PunRPC]
    public void nextObj()
    {
        if (q > objects.Length - 2)
        {
            q = -1;
            selection = null;
        }
        else
        {
            q++;
            selection = objects[q];
        }
    }

    [PunRPC]
    void objsd()
    {
        objects = GameObject.FindGameObjectsWithTag("trapz");

        objects[0] = GameObject.Find("Trapeziod (1)");
        objects[1] = GameObject.Find("Trapeziod (2)");
        objects[2] = GameObject.Find("Trapeziod (3)");
        objects[3] = GameObject.Find("Trapeziod (4)");
        objects[4] = GameObject.Find("Trapeziod (5)");
        objects[5] = GameObject.Find("Trapeziod (6)");
    }
}


