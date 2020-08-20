using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class POC3StackingArea : MonoBehaviourPunCallbacks
{
    List<DraggingObs> Shapes = new List<DraggingObs>();
    public static POC3StackingArea Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(Instance.gameObject);
        }
    }

    [PunRPC]
    void AddShapeToStack(GameObject _object)
    {

    }

    [PunRPC]
    void RemoveShapeFromStack(GameObject _object)
    {

    }

}
