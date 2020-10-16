using UnityEngine;
using System.Collections;
using Photon.Pun;



public class poc2DRag : MonoBehaviourPunCallbacks
{
    private Vector3 mOffset;
    private float mZCoord;
    Vector3 Fb, lastMouseCoordinate;
    Vector3 mousePoint;

    float x, y, z, q, x1, y1, z1;
    float prex, prez;
    private float rotSen = 1;
    public float min;

    Rigidbody rb;

    public bool selected;
    Event key;
    public Material select, notsel;
    private float mX;
    private float mY;
    private Transform objMovingReferece;
    private Color myObjColor;
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        myObjColor = GetComponent<Renderer>().material.color;
    }

    private void Update()
    {
        if (selected)
        {
            for (int i = 0; i < photonView.gameObject.GetComponent<Renderer>().materials.Length; i++)
            {
                photonView.gameObject.GetComponent<Renderer>().materials[i].color = Color.red;
            }
        }
        else {
            for (int i = 0; i < photonView.gameObject.GetComponent<Renderer>().materials.Length; i++)
            {
                photonView.gameObject.GetComponent<Renderer>().materials[i].color = myObjColor;
            }
        }
       


        if (selected)
        {

            // x1 = Input.GetAxis("Horizontal") * 0.2f;
            // y1 = Input.GetAxis("Vertical") * 0.2f;
            Vector3 pas = transform.position - GameObject.FindGameObjectWithTag("Player").transform.position;
            pas.Normalize();
            
            if (Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0 || Input.GetKey(KeyCode.Q) || Input.GetKey(KeyCode.E) || Input.GetKey(KeyCode.Z) && Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.X) && Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.C) && Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.Z) || Input.GetKey(KeyCode.X) || Input.GetKey(KeyCode.C))
            {
                base.photonView.RequestOwnership();
            }
            rb.useGravity = false;
            rb.velocity = Vector3.zero;
            if (Input.GetKey(KeyCode.Q))
            {
                var localDirection = Camera.main.transform.InverseTransformDirection(Camera.main.transform.up);
                transform.Translate(localDirection * 5 * Time.deltaTime, Space.World);
            }
            else if (Input.GetKey(KeyCode.A))
            {
                if (transform.position.y > 0.6f)
                {
                    var localDirection = Camera.main.transform.InverseTransformDirection(Camera.main.transform.up*-1);
                    transform.Translate(localDirection * 5 * Time.deltaTime, Space.World);
                }
            }
            else if (Input.GetKey(KeyCode.S))
            {
                var localDirection = Camera.main.transform.InverseTransformDirection(Camera.main.transform.right);
                transform.Translate(localDirection * 5 * Time.deltaTime, Space.World);
            }
            else if (Input.GetKey(KeyCode.W))
            {
                var localDirection = Camera.main.transform.InverseTransformDirection(Camera.main.transform.right*-1);
                transform.Translate(localDirection * 5 * Time.deltaTime, Space.World);
            }
            else if (Input.mouseScrollDelta.y != 0)
            {
                transform.position += new Vector3(pas.x, 0, pas.z) * Input.mouseScrollDelta.y;
            }
            else if (Input.GetKey(KeyCode.Z) && Input.GetKey(KeyCode.LeftShift))
            {
                z = -1 * rotSen;
                rb.velocity = Vector3.zero;
                rb.angularVelocity = Vector3.zero;
            }
            else if (Input.GetKey(KeyCode.Z))
            {
                z = 1 * rotSen;
                rb.velocity = Vector3.zero;
                rb.angularVelocity = Vector3.zero;
            }
            else
            {
                z = 0;
            }

            if (Input.GetKey(KeyCode.X) && Input.GetKey(KeyCode.LeftShift))
            {
                x = -1 * rotSen;
                rb.velocity = Vector3.zero;
                rb.angularVelocity = Vector3.zero;
            }
            else if (Input.GetKey(KeyCode.X))
            {
                x = 1 * rotSen;
                rb.velocity = Vector3.zero;
                rb.angularVelocity = Vector3.zero;
            }
            else
            {
                x = 0;
            }

            if (Input.GetKey(KeyCode.C) && Input.GetKey(KeyCode.LeftShift))
            {
                y = -1 * rotSen;
                rb.velocity = Vector3.zero;
                rb.angularVelocity = Vector3.zero;
            }
            else if (Input.GetKey(KeyCode.C))
            {
                y = 1 * rotSen;
                rb.velocity = Vector3.zero;
                rb.angularVelocity = Vector3.zero;
            }
            else
            {
                y = 0;
            }

            transform.rotation *= Quaternion.Euler(new Vector3(x, y, z));
        }
        else
        {
            rb.useGravity = true;
            
        }

    }

    private void OnMouseDown()
    {
      
        //gameObject.transform.parent = GameObject.FindGameObjectWithTag("Player").transform;
        //gameObject.GetComponent<Rigidbody>().useGravity = false;
        //gameObject.GetComponent<Rigidbody>().velocity = Vector3.zero;

        //Cursor.visible = false;
        

    }
    private void OnMouseDrag()
    {
        if (selected)
        {
            Vector3 pas = transform.position - GameObject.FindGameObjectWithTag("Player").transform.position;
            pas.Normalize();


            mX = Input.GetAxis("Mouse X");
            mY = Input.GetAxis("Mouse Y");


            transform.localPosition += new Vector3(mX, mY);


            if (transform.position.y < min)
            {
                transform.position = new Vector3(transform.position.x, min, transform.position.z);
            }
            gameObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
        }



    }

    private void OnMouseUp()
    {
        gameObject.transform.parent = null;
        gameObject.GetComponent<Rigidbody>().useGravity = true;
        Cursor.visible = true;
    }

    /*void OnCollisionEnter(Collision collision)
    {

        if (collision.transform.CompareTag("Floor"))
        {
            min = transform.position.y;
        }
        else min = -50;
    }*/
}
