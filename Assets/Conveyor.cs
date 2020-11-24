using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Conveyor : MonoBehaviour
{
    [HideInInspector]
    public bool isMoving = true;
    public float Speed;
    public static Conveyor Instance;

    PhotonView myPhotonView;
    Rigidbody myRgd;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    void Start()
    {
        myRgd = GetComponent<Rigidbody>();
        myPhotonView = GetComponent<PhotonView>();
    }
    
    public void StartStopMoving(bool _move)
    {
        isMoving = _move;
        myPhotonView.RPC("MoveSync", RpcTarget.AllBuffered,_move);
    }

    public void SetMovingLocal(bool _move)
    {
        isMoving = _move;
    }
    [PunRPC]
    void MoveSync(bool _move)
    {
        Conveyor.Instance.SetMovingLocal(_move);
    }
   

    private void OnCollisionStay(Collision collision)
    {
        if (collision.transform.CompareTag("Staks"))
        {
            collision.transform.GetComponent<DraggingObs>().isOnConveyor = true;
        }
    }
    private void OnCollisionExit(Collision collision)
    {
        if (collision.transform.CompareTag("Staks"))
        {
            collision.transform.GetComponent<DraggingObs>().isOnConveyor = false;
        }
    }
}
