using UnityEngine;
using Photon.Pun;
using UnityEditor;
using com.PT.contest;
using System.Collections.Generic;
using System.Collections;
using System.Runtime.InteropServices;
using ExitGames.Client.Photon;

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
    public bool isOnConveyor = false;
    private void OnMouseDown()
    {
        Manager.Instance.SetKinematic(true);
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
            if (Manager.Instance.LockObjects)
            {
                photonView.RPC("MakeObjectsKinematic", RpcTarget.AllBuffered, true);
            }
            base.photonView.RequestOwnership();
            for(int i = 0; i < LockedObjects.Count; i++)
            {
                LockedObjects[i].GetComponent<PhotonView>().RequestOwnership();
                if (LockedObjects[i].GetComponent<DraggingObs>().LockedObjects.Count != 0)
                {
                    for(int j=0;j< LockedObjects[i].GetComponent<DraggingObs>().LockedObjects.Count; j++)
                    {
                        LockedObjects[i].GetComponent<DraggingObs>().LockedObjects[j].GetComponent<PhotonView>().RequestOwnership();
                    }
                }
            }
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
                               photonView.RPC("AddObjectToList", RpcTarget.AllBuffered, otherObj.LockedObjects[j].GetComponent<PhotonView>().ViewID);
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

               transform.localPosition += new Vector3(mX, mY);
               // Vector3 vToMove = transform.localPosition + new Vector3(mX, mY);
               // transform.GetComponent<Rigidbody>().MovePosition(transform.position+transform.up*Time.fixedDeltaTime*mY);

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
            StartCoroutine(OnUpDelay());
        }
    }

    private IEnumerator OnUpDelay()
    {
        yield return new WaitForRealSeconds(0.05f);
        Manager.Instance.SetKinematic(false);
        if (Manager.Instance.LockObjects)
        {
            photonView.RPC("MakeObjectsKinematic", RpcTarget.AllBuffered, false);
        }
       
        isMoving = false;
        lerpAlha = false;
        Cursor.lockState = CursorLockMode.None;
        mouseRotationMode = false;
        gameObject.transform.parent = null;
       
       
        photonView.RPC("SetRigidBodyGravity", RpcTarget.AllBuffered, true);
        Cursor.visible = true;
        if (Manager.Instance.LockObjects)
        {
            for (int i = 0; i < LockedObjects.Count; i++)
            {
                LockedObjects[i].GetComponent<Rigidbody>().useGravity = true;
                LockedObjects[i].parent = null;
                LockedObjects[i].GetComponent<DraggingObs>().SetRPCGravity(true);
            }
            for (int i = 0; i < LockedObjects.Count; i++)
            {
                if (LockedObjects[i].GetComponent<DraggingObs>() != null)
                {
                    otherObj = LockedObjects[i].GetComponent<DraggingObs>();
                    for (int j = 0; j < otherObj.LockedObjects.Count; j++)
                    {
                        photonView.RPC("RemoveObjetFromList", RpcTarget.AllBuffered, otherObj.LockedObjects[j].GetComponent<PhotonView>().ViewID);
                    }
                }
            }
        }
    }

    [PunRPC]
    void MakeObjectsKinematic(bool state)
    {
        if (state)
        {
            foreach (Rigidbody rg in FindObjectsOfType<Rigidbody>())
            {
                if (rg.gameObject != gameObject&&!rg.transform.CompareTag("Floor")&&!rg.GetComponent<DraggingObs>().isOnConveyor)
                {
                    rg.isKinematic = true;
                    rg.useGravity = true;
                }
            }
        }
        else
        {
            foreach (Rigidbody rg in FindObjectsOfType<Rigidbody>())
            {

                if (!rg.transform.CompareTag("Floor")&& !rg.GetComponent<DraggingObs>().isOnConveyor)
                {
                    rg.isKinematic = false;

                    rg.useGravity = true;
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

    [PunRPC]
    void AddObjectToList(int _objectID)
    {
        
        foreach (PhotonView gg in FindObjectsOfType<PhotonView>())
        {
            if (gg.ViewID == _objectID)
            {
                if (!LockedObjects.Contains(gg.transform))
                {
                    LockedObjects.Add(gg.transform);
                   
                }
            }
        }
    }

    [PunRPC]
    void RemoveObjetFromList(int _objectID)
    {
        foreach (PhotonView gg in FindObjectsOfType<PhotonView>())
        {
            if (gg.ViewID == _objectID)
            {
                if (LockedObjects.Contains(gg.transform))
                {
                    
                    LockedObjects.Remove(gg.transform);
                   
                }
            }
        }
    }

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
            if (isMoving && collision.transform.CompareTag("Floor"))
            {
                minY = 1;
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
                    photonView.RPC("AddObjectToList", RpcTarget.AllBuffered, collision.transform.GetComponent<PhotonView>().ViewID);
                }
            }
        }

        //if (collision.transform.CompareTag("Floor"))
        //{
        //    myRigidbody.isKinematic = Manager.Instance.GetKinematic();
        //}
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
                photonView.RPC("RemoveObjetFromList", RpcTarget.AllBuffered, collision.transform.GetComponent<PhotonView>().ViewID);
                SetRPCGravity(false);
            }
        }
    }
    public bool isTouchingFloor = false;
    public void RemoveFromList(Transform _object)
    {
        if (LockedObjects.Contains(_object))
        {
            photonView.RPC("RemoveObjetFromList", RpcTarget.AllBuffered, _object.GetComponent<PhotonView>().ViewID);
        }
    }
}
