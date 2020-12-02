using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

public class PhotonRoomManager : MonoBehaviourPunCallbacks, IInRoomCallbacks
{
    public  delegate void Notification();
    public event Notification OnConnectMaster;
    public event Notification OnCreatedRoomSuccesful;

    public static PhotonRoomManager instance;

    public int      maxPlayerInRoom = 3;
    public bool     roomIsVisable = true;
    public bool     roomIsOpen = true;

    [SerializeField] public Text        roomNameText;
    [SerializeField] private Transform  roomsHolder;
    [SerializeField] private GameObject roomBuutonBrefab;
    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        PhotonNetwork.AutomaticallySyncScene = true;
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster()
    {
        if (Screen.fullScreen) Screen.fullScreen = false;
        PhotonNetwork.GameVersion = "0.0.0";
        PhotonNetwork.JoinLobby();
        OnConnectMaster.Invoke();
    }


    public void CreateRoom()
    {
        if(roomNameText.text == "") return;
        
        RoomOptions roomOps = new RoomOptions() { IsVisible = roomIsVisable, IsOpen = roomIsOpen, MaxPlayers = (byte)maxPlayerInRoom };
        PhotonNetwork.CreateRoom(roomNameText.text , roomOps);
    }

    public override void OnCreatedRoom()
    {
        OnCreatedRoomSuccesful.Invoke();
    }

    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();
        Debug.Log("Joined To Room : " + PhotonNetwork.CurrentRoom.Name );
    }



    private List<RoomInfo> rooms = new List<RoomInfo>();

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
       foreach(var ri in roomList)
        {
            if(ri.RemovedFromList)
            {
                rooms.Remove(ri);
                for(int i = 0 ; i < roomsHolder.childCount ; i++)
                {
                    if(ri.Name == roomsHolder.GetChild(i).GetComponent<RoomButton>().roomName)
                    {
                        Destroy(roomsHolder.GetChild(i).gameObject);
                        break;
                    }
                }
            }
            else if(!rooms.Contains(ri))
            {
                rooms.Add(ri);
                GameObject go  = Instantiate(roomBuutonBrefab,roomsHolder);
                go.GetComponent<RoomButton>().SetRoom(ri.Name);
            }
        }
    }


    public void JoinRoom(string roomName)
    {
        PhotonNetwork.JoinRoom(roomName);
        
    }

}
