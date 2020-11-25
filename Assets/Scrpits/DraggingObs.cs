using UnityEngine;
using Photon.Pun;
using com.PT.contest;
using System.Collections.Generic;
using System.Collections;
using ExitGames.Client.Photon;

public class DraggingObs : MonoBehaviourPunCallbacks
{
    public float rotSen = 100;


    float                   x, y, z;
    private float           ObjectMovingSpeed = 0;
    private Camera          myCamera;
    private Rigidbody       myRigidbody;
    float                   mX, mY;
    Color                   myColor;
    
    private PhotonView      pv;

    public List<Transform>  LockedObjects = new List<Transform>();
    bool                    lerpAlha = false;

    private Vector3         screenPoint;
    private Vector3         offset;
    private GameObject      PlayerArea;


    public  bool            canMoveObject = true;
    public  bool            isOnConveyor = false;

    float                   minY = 0.7f;
    Vector3                 pas;
    public  bool            isMoving = false;

    private bool            mouseRotationMode = false;
    private DraggingObs     otherObj;

    public  int             numOfObjects = 0;

    public  bool            isTouchingFloor = false;
    private bool            onMouse;
    public  bool            smallCube;
    private bool            movedByConvery;

    public List<Transform> contactObjects = new List<Transform>();
    private IEnumerator checkParentIE;

    private void Awake()
    {
        myRigidbody = GetComponent<Rigidbody>();
        pv          = GetComponent<PhotonView>();
    }

    private void Start()
    {
        Initalization();
    }
  
    private void Initalization()
    {
        myRigidbody.drag = 1;
        myRigidbody.angularDrag = (smallCube) ? 150 : 15;
        ObjectMovingSpeed = 5;
        myColor = GetComponent<Renderer>().material.color;
    }
    
    private void Update()
    {
        if (numOfObjects == 1 && !isMoving)
        {
            LockedObjects.Clear();
        }

        ChangeMaterialAlphaColor();

        FollowParent();

        
        if (contactObjects.Count == 0)
        {
            countThershold++;

            if(countThershold > 5)
            {
                countThershold = 0;
                transform.parent = null;
            }

        }
        else
        {
            countThershold = 0;
        }
        

    }

   private int countThershold;



    private void FixedUpdate()
    {
      
        if(isOnConveyor && Conveyor.Instance.isMoving)
        {
            Vector3 vel = myRigidbody.velocity;
            vel.z = Conveyor.Instance.Speed;
            myRigidbody.velocity = vel;
            movedByConvery = true;
        }
        if(!isOnConveyor&& movedByConvery)
        {

            Vector3 vel = Vector3.zero;
            myRigidbody.velocity = vel;
            movedByConvery = false;
        }
    }

    private void ChangeMaterialAlphaColor()
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

    public void SetPlayerArea(GameObject playerArea)
    {
        PlayerArea = playerArea;
    }

    private bool isFollowingParnt , firstIntializationFollowinParent;
    private Vector3 parentLastPosition;
    private Vector3 deltaFromParent;
   
    void FollowParent()
    {
        if(isFollowingParnt && !firstIntializationFollowinParent)
        {
            firstIntializationFollowinParent = true;
            parentLastPosition = Manager.Instance.MyTransformRef.position;
        }

        if(isFollowingParnt && firstIntializationFollowinParent)
        {
            deltaFromParent = Manager.Instance.MyTransformRef.position - parentLastPosition;
            parentLastPosition = Manager.Instance.MyTransformRef.position;
        }
    }

    private void OnMouseDown()
    {
        onMouse = true;
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

            isFollowingParnt = true;
            // gameObject.transform.parent = Manager.Instance.MyTransformRef;
            
         



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
                               pv.RPC("AddObjectToList", RpcTarget.AllBuffered, otherObj.LockedObjects[j].GetComponent<PhotonView>().ViewID);
                            }
                            
                        }
                    }
                }
                for (int i = 0; i < LockedObjects.Count; i++)
                {
                  
                    LockedObjects[i].GetComponent<Rigidbody>().isKinematic = true;
                    LockedObjects[i].SetParent(transform);
                    LockedObjects[i].GetComponent<Rigidbody>().velocity = Vector3.zero;
                    
                }
            }
          
            gameObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
            lerpAlha = true;
            Cursor.visible = false;
            x = transform.eulerAngles.x;
            y = transform.eulerAngles.y;
            z = transform.eulerAngles.z;
        }

        pv.RPC("MakeObjectKinematic", RpcTarget.AllBuffered);
    }
   
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

                mY = (!CheckContactObjectBelow() && mY < 0) ? 0 : mY;

                deltaFromParent = (!CheckContactObjectBelow() && deltaFromParent.y < 0) ? Vector3.zero : deltaFromParent;

                Vector3 cameraRight = Manager.Instance.MyCamRef.transform.right;
                cameraRight.y = 0;

                transform.localPosition += new Vector3(0, mY) + deltaFromParent + cameraRight * mX;
              
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
                Vector3 cameraForward = Manager.Instance.MyCamRef.transform.forward;
                cameraForward.y = 0;
                transform.localPosition -= cameraForward * 0.1f;
            }
            else if (Input.GetKey(KeyCode.E))
            {
                Vector3 cameraForward = Manager.Instance.MyCamRef.transform.forward;
                cameraForward.y = 0;
                transform.localPosition += cameraForward * 0.1f;
            }
        }
    }
 
    private void OnMouseUp()
    {
       onMouse = false;
        if (canMoveObject)
        {
            StartCoroutine(OnUpDelay());
        }

        pv.RPC("MakeObjectNonKinematic", RpcTarget.AllBuffered);

        isFollowingParnt = false;
        firstIntializationFollowinParent = false;

       for(int i = 0 ; i < transform.childCount ; i++)
        {
            transform.GetChild(i).parent = null;
        }
    }

    private IEnumerator OnUpDelay()
    {
        yield return new WaitForRealSeconds(0.05f);
        Manager.Instance.SetKinematic(false);
        if (Manager.Instance.LockObjects)
        {
            pv.RPC("MakeObjectsKinematic", RpcTarget.AllBuffered, false);
        }
       
        isMoving = false;
        lerpAlha = false;
        Cursor.lockState = CursorLockMode.None;
        mouseRotationMode = false;
        gameObject.transform.parent = null;
       
       
   
        Cursor.visible = true;
        if (Manager.Instance.LockObjects)
        {
            for (int i = 0; i < LockedObjects.Count; i++)
            {
               
                LockedObjects[i].parent = null;

            }
            for (int i = 0; i < LockedObjects.Count; i++)
            {
                if (LockedObjects[i].GetComponent<DraggingObs>() != null)
                {
                    otherObj = LockedObjects[i].GetComponent<DraggingObs>();
                    for (int j = 0; j < otherObj.LockedObjects.Count; j++)
                    {
                        pv.RPC("RemoveObjetFromList", RpcTarget.AllBuffered, otherObj.LockedObjects[j].GetComponent<PhotonView>().ViewID);
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

               
                }
            }
        }
    }

   
   
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

    [PunRPC]
    void MakeObjectKinematic()
    {
        myRigidbody.isKinematic = true;
    }

    [PunRPC]
    void MakeObjectNonKinematic()
    {
        myRigidbody.isKinematic = false;
      
    }

    [PunRPC]
    void ReleaseFromParent()
    {
        transform.parent = null;
    }

  
    void OnCollisionEnter(Collision collision)
    {
        
        DraggingObs dr = collision.transform.root.GetComponent<DraggingObs>();

        if(dr != null)
        {
            if (dr.onMouse)
            {
                float d = transform.position.y - collision.transform.position.y;
                if (dr.transform == collision.transform && d > 0.4f)
                {
                    transform.parent = dr.transform;
        
                }

                    
                else if(collision.transform.root == dr.transform && d > 0.4f)
                {
                    transform.parent = collision.transform;
                }
            }

         
        }

        if(!contactObjects.Contains(collision.transform))
        {
            contactObjects.Add(collision.transform);
        }


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
                    pv.RPC("AddObjectToList", RpcTarget.AllBuffered, collision.transform.GetComponent<PhotonView>().ViewID);
                }
            }
        }

      
    }
    
    
   
    IEnumerator CheckParentOnCollision()
    {
        yield return new WaitForSeconds(0.1f);
        int counter = 0;
        bool check = false;
        while(counter < 3)
        {
            if(transform.parent != null && contactObjects.Contains(transform.parent))
            {
                check = true;
            }
            yield return null;
        }
        if(!check)
        {
            pv.RPC("MakeObjectNonKinematic", RpcTarget.AllBuffered);
            pv.RPC("ReleaseFromParent", RpcTarget.AllBuffered);
        }
        checkParentIE = null;
    }
    private void OnCollisionExit(Collision collision)
    {


        if( collision.transform == transform.parent )
        {
            if(checkParentIE == null)
            {
                checkParentIE = CheckParentOnCollision();
                StartCoroutine(checkParentIE);
            }
           
        }

       

        numOfObjects--;
        if (collision.transform.CompareTag("Floor"))
        {
            isTouchingFloor = false;
        }
        if (collision.transform.CompareTag("Staks"))
        {
            if (LockedObjects.Contains(collision.transform))
            {
                pv.RPC("RemoveObjetFromList", RpcTarget.AllBuffered, collision.transform.GetComponent<PhotonView>().ViewID);
              
            }
        }

        if(!onMouse)
        {
            pv.RPC("MakeObjectNonKinematic", RpcTarget.AllBuffered);
                
        }


        if (contactObjects.Contains(collision.transform))
        {
            contactObjects.Remove(collision.transform);
        }


    }

    public void RemoveFromList(Transform _object)
    {
        if (LockedObjects.Contains(_object))
        {
            pv.RPC("RemoveObjetFromList", RpcTarget.AllBuffered, _object.GetComponent<PhotonView>().ViewID);
        }
    }

  

    bool CheckContactObjectBelow()
    {
        bool isBelowEmpty = true;

        foreach(var t in contactObjects)
        {
            if(transform.position.y > t.position.y)
                isBelowEmpty =false;
        }
        return isBelowEmpty;
    }

}
