using UnityEngine;
using Photon.Pun;
using UnityEditor;
using com.PT.contest;
using System.Collections.Generic;
using System.Collections;

public class DraggingObs : MonoBehaviourPunCallbacks
{
    float x, y, z;
    public float ObjectMovingSpeed = 0;
    public float rotSen = 100;
    
    private Camera myCamera;
    private Rigidbody myRigidbody;
    float mX, mY;
    Color myColor;
    
    PhotonView photonView;
    private void Awake()
    {
        myRigidbody = GetComponent<Rigidbody>();
        myRigidbody.drag = 1;
        myRigidbody.angularDrag = 15;
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
        if (numOfObjects == 1&&!isMoving)
        {
            LockedObjects.Clear();
        }
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
        
        if (canMoveObject)
        {
            base.photonView.RequestOwnership();
            gameObject.transform.parent = Manager.Instance.MyTransformRef;
            gameObject.GetComponent<Rigidbody>().useGravity = false;
            if (Manager.Instance.LockObjects)
            {
                
                for (int i = 0; i < LockedObjects.Count; i++)
                {
                    if (LockedObjects[i].GetComponent<DraggingObs>() != null)
                    {
                        otherObj = LockedObjects[i].GetComponent<DraggingObs>();
                        for (int j = 0; j < otherObj.LockedObjects.Count; j++)
                        {
                            if (!LockedObjects.Contains(otherObj.LockedObjects[j]))
                            {
                                LockedObjects.Add(otherObj.LockedObjects[j]);
                            }
                            
                        }
                    }
                }
                for (int i = 0; i < LockedObjects.Count; i++)
                {
                    LockedObjects[i].GetComponent<Rigidbody>().useGravity = false;
                    LockedObjects[i].GetComponent<Rigidbody>().isKinematic = true;
                    LockedObjects[i].SetParent(transform);
                    LockedObjects[i].GetComponent<Rigidbody>().velocity = Vector3.zero;
                    //LockedObjects[i].GetComponent<DraggingObs>().SetRPCGravity(false);
                }
            }
            photonView.RPC("SetRigidBodyGravity", RpcTarget.AllBuffered, false);
            gameObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
            lerpAlha = true;
            Cursor.visible = false;
            x = transform.eulerAngles.x;
            y = transform.eulerAngles.y;
            z = transform.eulerAngles.z;
        }
    }

    float minY = 0.7f;
    Vector3 pas;
    public bool isMoving = false;
    private void OnMouseDrag()
    {
        if (canMoveObject)
        {
            isMoving = true;
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
                gameObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
                if (transform.position.y< (minY-0.1f))
                {
                    transform.position = new Vector3(transform.position.x, minY, transform.position.z);
                }
                if (Input.mouseScrollDelta.y != 0)
                {
                    transform.position += new Vector3(pas.x, 0, pas.z) * Input.mouseScrollDelta.y;
                }
                else
                {
                    transform.localPosition += new Vector3(mX, mY);
                }
                
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
    private DraggingObs otherObj;
    private void OnMouseUp()
    {
       
        if (canMoveObject)
        {
            isMoving = false;
            lerpAlha = false;
            Cursor.lockState = CursorLockMode.None;
            mouseRotationMode = false;
            gameObject.transform.parent = null;
            gameObject.GetComponent<Rigidbody>().useGravity = true;
            photonView.RPC("SetRigidBodyGravity", RpcTarget.AllBuffered, true);
            Cursor.visible = true;
            if (Manager.Instance.LockObjects)
            {
                for (int i = 0; i < LockedObjects.Count; i++)
                {
                    LockedObjects[i].GetComponent<Rigidbody>().useGravity = true;
                    LockedObjects[i].GetComponent<Rigidbody>().isKinematic = false;
                    LockedObjects[i].parent = null;
                    //LockedObjects[i].GetComponent<DraggingObs>().SetRPCGravity(true);
                }
                for (int i = 0; i < LockedObjects.Count; i++)
                {
                    if (LockedObjects[i].GetComponent<DraggingObs>() != null)
                    {
                        otherObj = LockedObjects[i].GetComponent<DraggingObs>();
                        for (int j = 0; j < otherObj.LockedObjects.Count; j++)
                        {
                            LockedObjects.Remove(otherObj.LockedObjects[j]);
                        }
                    }
                }
            }
        }
    }

    [PunRPC]
    void SetRigidBodyGravity(bool state)
    {
        myRigidbody.useGravity = state;
    }
    private List<Transform> LockedObjects = new List<Transform>();
    public void SetRPCGravity(bool _state)
    {
        if (_state)
        {
            photonView.RPC("SetRigidBodyGravity", RpcTarget.AllBuffered, true);
        }
        else
        {
            photonView.RPC("SetRigidBodyGravity", RpcTarget.AllBuffered, false);
        }
    }
    int numOfObjects = 0;
    void OnCollisionEnter(Collision collision)
    {
        if (Manager.Instance.LockObjects)
        {
            if (isMoving && !collision.transform.CompareTag("centre"))
            {
                minY = transform.position.y;
            }
        }
        numOfObjects++;
        if (collision.transform.CompareTag("Floor"))
        {
            
            foreach (GameObject obj in GameObject.FindGameObjectsWithTag("Staks"))
            {
                obj.GetComponent<DraggingObs>().RemoveFromList(transform);
            }
           
            isTouchingFloor = true;
            for(int i = 0; i < LockedObjects.Count; i++)
            {
                LockedObjects[i].GetComponent<DraggingObs>().LockedObjects.Clear();
            }
        }
        
    }
    private void OnCollisionStay(Collision collision)
    {
        if (collision.transform.CompareTag("Staks"))
        {
            if (transform.position.y + 0.4f < collision.transform.position.y)
            {
                if (!LockedObjects.Contains(collision.transform))
                {
                    LockedObjects.Add(collision.transform);
                }
            }
        }
       
    }
    
    private void OnCollisionExit(Collision collision)
    {
        numOfObjects--;
        if (collision.transform.CompareTag("Floor"))
        {
            isTouchingFloor = false;
            
        }
        if (collision.transform.CompareTag("Staks"))
        {
            if (LockedObjects.Contains(collision.transform))
            {
                LockedObjects.Remove(collision.transform);
            }
        }
    }
    public bool isTouchingFloor = false;
    public void RemoveFromList(Transform _object)
    {
        if (LockedObjects.Contains(_object))
        {
            LockedObjects.Remove(_object);
        }
    }
}
