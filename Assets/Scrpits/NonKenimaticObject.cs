using UnityEngine;
using Photon.Pun;
using com.PT.contest;
using System.Collections.Generic;
using System.Collections;
using ExitGames.Client.Photon;


public class NonKenimaticObject : MonoBehaviour,IConvayable
{
  
    private PhotonView      _pv;
    private Rigidbody       _rb;


    private bool            _isMouseOn;
    private Vector3         _followPos;
    private Vector2         _mousePos;
    private float           _mouseScreenSensitvity = 0.1f;
    [SerializeField]
    private bool            _isfollowingPos;


    private bool            isFollowingParnt, firstIntializationFollowinParent;
    private Vector3         parentLastPosition;
    private Vector3         deltaFromParent;

    private bool            isOnConveyor;
    private bool            movedByConvery;

    private void Awake()
    {
        _pv = GetComponent<PhotonView>();
        _rb = GetComponent<Rigidbody>();
        _followPos = transform.position;
    }


    private void OnMouseDown()
    {
        Cursor.visible = false;
        isFollowingParnt = true;
        _isMouseOn = true;
        _followPos = transform.position;
        _mousePos  = Input.mousePosition;
        _rb.freezeRotation = true;
        _pv.RequestOwnership();
        _isfollowingPos = true;
        _pv.RPC("ConstrainRotation", RpcTarget.AllBuffered);
        _pv.RPC("UpdateIsFolloingPos", RpcTarget.AllBuffered, _isfollowingPos);
    }



    private void OnMouseUp()
    {
        Cursor.visible = true;
        isFollowingParnt = false;
        firstIntializationFollowinParent = false;
        _isfollowingPos =false; 
        _isMouseOn = false;
        _followPos = transform.position;
        _rb.freezeRotation = false;
        _pv.RPC("UnConstrainRotation", RpcTarget.AllBuffered);
        _pv.RPC("UpdateIsFolloingPos", RpcTarget.AllBuffered, _isfollowingPos);
    }



    private void Update()
    {
        if(_isMouseOn)
        {
            FollowParent();
            MoveFollowPosition();
            if (_pv.IsMine)
                _pv.RPC("UpdateFollowPostion",RpcTarget.AllBuffered,_followPos);

        }
    }


    

    void MoveFollowPosition()
    {

        Vector3 cameraRight = Manager.Instance.MyCamRef.transform.right;
        cameraRight.y = 0;

        Vector2 mPos = Input.mousePosition;
        Vector2 delta = (mPos - _mousePos) * _mouseScreenSensitvity;
        _mousePos = mPos;
        _followPos += new Vector3(0, delta.y,0) + cameraRight * delta.x;

        if (Input.GetKey(KeyCode.Q))
        {
            Vector3 cameraForward = Manager.Instance.MyCamRef.transform.forward;
            cameraForward.y = 0;
            _followPos -= cameraForward * 0.1f;
        }
        else if (Input.GetKey(KeyCode.E))
        {
            Vector3 cameraForward = Manager.Instance.MyCamRef.transform.forward;
            cameraForward.y = 0;
            _followPos += cameraForward * 0.1f ;
        }

    }

    void FollowParent()
    {
        if (isFollowingParnt && !firstIntializationFollowinParent)
        {
            firstIntializationFollowinParent = true;
            parentLastPosition = Manager.Instance.MyTransformRef.position;
        }

        if (isFollowingParnt && firstIntializationFollowinParent)
        {
            deltaFromParent = Manager.Instance.MyTransformRef.position - parentLastPosition;
            parentLastPosition = Manager.Instance.MyTransformRef.position;
            _followPos += deltaFromParent;
        }
    }



    private void FixedUpdate()
    {
        if(_isfollowingPos)
        {
            Vector3 vel = (_followPos - transform.position) * 5;
            _rb.velocity = vel;
        }


        if (isOnConveyor && Conveyor.Instance.isMoving)
        {
            Vector3 vel = _rb.velocity;
            vel.z = Conveyor.Instance.Speed;
            _rb.velocity = vel;
            movedByConvery = true;
        }
        if (!isOnConveyor && movedByConvery)
        {

            Vector3 vel = Vector3.zero;
            _rb.velocity = vel;
            movedByConvery = false;
        }

    }

    [PunRPC]
    void UpdateFollowPostion(Vector3 pos)
    {
        _followPos = pos;
    }

    [PunRPC]
    void UpdateIsFolloingPos(bool follow)
    {
        _isfollowingPos = follow;
    }

    [PunRPC]
    void ConstrainRotation()
    {
        _rb.freezeRotation = true;
    }

    [PunRPC]
    void UnConstrainRotation()
    {
        _rb.freezeRotation = false;
    }

    public void SetObjectOnConvey(bool isOn)
    {
        isOnConveyor = isOn;
    }
}
