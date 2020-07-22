using UnityEngine;
using Photon.Pun;
using UnityEditor;

public class DraggingObs : MonoBehaviourPunCallbacks
{
    private float mZCoord;
    Vector3 Fb, lastMouseCoordinate;
    Vector3 mousePoint;

    float x, y, z, q;
    float prex, prez;
    public float ObjectMovingSpeed = 0;
    public float rotSen = 100;
    public float min;
    private Camera myCamera;
    private Rigidbody myRigidbody;
    float mX, mY, mZ;
    Color myColor;
    private void Awake()
    {
        myRigidbody = GetComponent<Rigidbody>();
        myRigidbody.drag = 1;
        myRigidbody.angularDrag = 1;
    }
    private void Start()
    {
        myColor = GetComponent<Renderer>().material.color;
        ObjectMovingSpeed = 5;
    }
    bool lerpAlha = false;
    
    private void Update()
    {
        if (lerpAlha)
        {
            myColor.a = Mathf.MoveTowards(0.5f, 1f, Time.deltaTime);
            GetComponent<Renderer>().material.SetColor("_BaseColor", myColor);
        }
        else
        {
            myColor.a = Mathf.MoveTowards(1f, 0.5f, Time.deltaTime);
            GetComponent<Renderer>().material.SetColor("_BaseColor", myColor);
        }
    }
    private void OnMouseDown()
    {
        //  base.photonView.RequestOwnership();
        gameObject.transform.parent = GameObject.FindGameObjectWithTag("Player").transform;
        gameObject.GetComponent<Rigidbody>().useGravity = false;
        gameObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
        lerpAlha = true;


        // Store offset = gameobject world pos - mouse world pos

        Cursor.visible = false;
        // Cursor.lockState = CursorLockMode.Locked;

        x = transform.eulerAngles.x;
        y = transform.eulerAngles.y;
        z = transform.eulerAngles.z;
    }
    private void OnMouseDrag()
    {
        if (myCamera == null)
        {
            myCamera = FindObjectOfType<Camera>();
        }
        if (Input.GetMouseButtonUp(1))
        {
            mouseRotationMode = !mouseRotationMode;
        }
        if (!mouseRotationMode)
        {
            Vector3 pas = transform.position -GetComponentInParent<Transform>().position;
            pas.Normalize();
            mX = Input.GetAxis("Mouse X")/4;
            mY = Input.GetAxis("Mouse Y")/4;
            
            if (Input.GetKey(KeyCode.Q))
            {
                myRigidbody.MovePosition(transform.position + myCamera.transform.forward * Time.fixedDeltaTime*ObjectMovingSpeed);
            }
            else if (Input.GetKey(KeyCode.E))
            {
                myRigidbody.MovePosition(transform.position - myCamera.transform.forward*Time.fixedDeltaTime*ObjectMovingSpeed);
            }
            else
            {
                myRigidbody.MovePosition(transform.position + new Vector3(mX, mY));
            }

            gameObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked;
            //transform.Rotate((Input.GetAxis("Mouse X") * 100 * Time.deltaTime), (Input.GetAxis("Mouse Y") * 100 * Time.deltaTime), 0, Space.Self);
            float rotX = Input.GetAxis("Mouse X") * 100 * Mathf.Deg2Rad;
            float rotY = Input.GetAxis("Mouse Y") * 100 * Mathf.Deg2Rad;
            transform.Rotate(Vector3.up, -rotX);
            transform.Rotate(Vector3.right, rotY);
        }
    }
    private bool mouseRotationMode = false;
    private void OnMouseUp()
    {
        lerpAlha = false;
        Cursor.lockState = CursorLockMode.None;
        mouseRotationMode = false;
        gameObject.transform.parent = null;
        gameObject.GetComponent<Rigidbody>().useGravity = true;
        Cursor.visible = true;
    }

    void OnCollisionEnter(Collision collision)
    {

        if (collision.transform.CompareTag("Floor"))
        {
            min = transform.position.y;
        }
        else min = -50;
    }

}
