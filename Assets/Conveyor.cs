using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Conveyor : MonoBehaviour
{
    public float Speed;
    Rigidbody myRgd;
    // Start is called before the first frame update
    public static Conveyor Instance;
    PhotonView myPhotonView;
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
    private bool isMoving = true;
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
    // Update is called once per frame
    void FixedUpdate()
    {
        if (isMoving)
        {
            Vector3 pos = myRgd.position;
            myRgd.position += -transform.forward * Speed * Time.fixedDeltaTime;
            myRgd.MovePosition(pos);
        }
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
            collision.transform.GetComponent<DraggingObs>().isOnConveyor = true;
        }
    }
}
