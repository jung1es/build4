using UnityEngine;
using Photon.Pun;
using UnityEditor;
using com.PT.contest;

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
    public enum AvilableColors { Red,Blue,Green,Yellow,Purple}
    public AvilableColors MyObjectColor;
    PhotonView photonView;
    private void Awake()
    {
        myRigidbody = GetComponent<Rigidbody>();
        myRigidbody.drag = 1;
        myRigidbody.angularDrag = 1;
    }
    private void Start()
    {
        photonView = GetComponent<PhotonView>();
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
    
    private Vector3 screenPoint;
    private Vector3 offset;
    private GameObject PlayerArea;
    public void SetPlayerArea(GameObject playerArea)
    {
        PlayerArea = playerArea;
    }
    private bool canMoveObject = true;
    
    private void OnMouseDown()
    {
        myCamera = Manager.Instance.MyCamRef;
        if (PlayerArea != null)
        {
            if(PlayerArea == Manager.Instance.MyTransformRef.GetComponent<PlayerController>().GetMyArea())
            {
                canMoveObject = true;
            }
            else
            {
                canMoveObject = false;
            }
        }
        else
        {
            canMoveObject = true;
        }
        Debug.Log(myCamera.transform.position);
        if (canMoveObject)
        {
            base.photonView.RequestOwnership();
            gameObject.transform.parent = Manager.Instance.MyTransformRef;
            gameObject.GetComponent<Rigidbody>().useGravity = false;
            photonView.RPC("SetRigidBodyGravity", RpcTarget.AllBuffered, false);
            gameObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
            lerpAlha = true;
            Cursor.visible = false;
            x = transform.eulerAngles.x;
            y = transform.eulerAngles.y;
            z = transform.eulerAngles.z;
        }
    }
    Vector3 pas;
    private void OnMouseDrag()
    {
        if (canMoveObject)
        {
           
            if (Input.GetMouseButtonUp(1))
            {
                mouseRotationMode = !mouseRotationMode;
                screenPoint = myCamera.WorldToScreenPoint(gameObject.transform.position);
                offset = gameObject.transform.position - myCamera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z));
            }

            if (!mouseRotationMode)
            {
                Cursor.lockState = CursorLockMode.None;
                pas = transform.position - Manager.Instance.MyTransformRef.position;
                pas.Normalize();
                mX = Input.GetAxis("Mouse X") / 2;
                mY = Input.GetAxis("Mouse Y") / 2;
                if (Input.mouseScrollDelta.y != 0)
                {
                    transform.position += new Vector3(pas.x, 0, pas.z) * Input.mouseScrollDelta.y;
                }
                else
                {
                    transform.localPosition += new Vector3(mX, mY);
                }
                gameObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
            }
            else
            {
                Cursor.lockState = CursorLockMode.Locked;
                //transform.Rotate((Input.GetAxis("Mouse X") * 100 * Time.deltaTime), (Input.GetAxis("Mouse Y") * 100 * Time.deltaTime), 0, Space.Self);
                float rotX = Input.GetAxis("Mouse X") * 100 * Mathf.Deg2Rad;
                float rotY = Input.GetAxis("Mouse Y") * 100 * Mathf.Deg2Rad;
                transform.Rotate(transform.up, -rotX);
                transform.Rotate(transform.right, rotY);
            }
            if (Input.GetKey(KeyCode.Q))
            {
                transform.position -= new Vector3(pas.x, 0, pas.z) * 0.1f;
            }
            else if (Input.GetKey(KeyCode.E))
            {
                transform.position += new Vector3(pas.x, 0, pas.z) * 0.1f;
            }
        }
    }
    private bool mouseRotationMode = false;
    private void OnMouseUp()
    {
        if (canMoveObject)
        {
            lerpAlha = false;
            Cursor.lockState = CursorLockMode.None;
            mouseRotationMode = false;
            gameObject.transform.parent = null;
            gameObject.GetComponent<Rigidbody>().useGravity = true;
            photonView.RPC("SetRigidBodyGravity", RpcTarget.AllBuffered, true);
            Cursor.visible = true;
        }
    }

    [PunRPC]
    void SetRigidBodyGravity(bool state)
    {
        myRigidbody.useGravity = state;
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
