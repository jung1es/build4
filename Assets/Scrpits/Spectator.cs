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

        if (GameObject.FindGameObjectWithTag("Staks"))
            selection = null;
        rotX = transform.rotation.x;
        rotY = transform.rotation.y;
    }

    public bool m_MoveCamera = true;
    // Update is called once per frame
    void Update()
    {
        /*if (!photonView.IsMine)
        {
            return;
        }*/
        if (Manager.Instance.IsSharedControl) {
            if (GameObject.FindGameObjectWithTag("Staks"))
            {
                photonView.RPC("objsd", RpcTarget.All);
                if (objects != null)
                {
                    foreach (GameObject obj in objects)
                    {
                        if (obj == selection) obj.GetComponent<poc2DRag>().selected = true;
                        else obj.GetComponent<poc2DRag>().selected = false;
                    }
                }
                if (Input.GetKeyDown(KeyCode.Tab)) photonView.RPC("nextObj", RpcTarget.All);

                if (m_MoveCamera)
                {
                    speed = 1;
                    rotX += Input.GetAxis("Horizontal") * speed;
                    rotY -= Input.GetAxis("Vertical") * speed;
                    transform.rotation = Quaternion.Euler(rotY, rotX, 0);
                }
            }
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
                
                if (Input.GetKey(KeyCode.E)&&!Input.GetMouseButton(0))
                {
                    transform.Rotate(Vector3.up, 40 * Time.deltaTime, Space.World);
                }

                if (Input.GetKey(KeyCode.Q) && !Input.GetMouseButton(0))
                {
                    transform.Rotate(Vector3.up, -40 * Time.deltaTime, Space.World);
                }
                if (Input.GetKey(KeyCode.C))
                {
                    transform.Rotate(Vector3.left, 40 * Time.deltaTime, Space.Self);
                }

                if (Input.GetKey(KeyCode.Z))
                {
                    transform.Rotate(Vector3.left, -40 * Time.deltaTime, Space.Self);
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
        objects = GameObject.FindGameObjectsWithTag("Staks");

    }
}


