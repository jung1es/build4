using UnityEngine;
using Photon.Pun;
using Photon.Realtime;


namespace com.PT.contest
{

    public class Launcher : MonoBehaviourPunCallbacks
    {
        public GameObject menu, lvlSelction;
        bool on;

      
        private void Start()
        {
            PhotonNetwork.AutomaticallySyncScene = true;
            PhotonNetwork.ConnectUsingSettings();
        }

        public override void OnConnectedToMaster()
        {
            if (Screen.fullScreen) Screen.fullScreen = false;
            Join();
        }

        public override void OnJoinedRoom()
        {
            base.OnJoinedRoom();
        }
        public override void OnCreateRoomFailed(short returnCode, string message)
        {
            base.OnCreateRoomFailed(returnCode, message);
            Debug.Log("FAILED TO CREATE ROOM");
        }
        public override void OnJoinRandomFailed(short returnCode, string message)
        {
            Debug.Log("Failed to join room");
            Create();
        }

        public void Create()
        {
            RoomOptions roomOps = new RoomOptions() { IsVisible = true, IsOpen = true, MaxPlayers = (byte)3 };
            PhotonNetwork.CreateRoom("Room0", roomOps);
        }

        public override void OnCreatedRoom()
        {
            base.OnCreatedRoom();
            Join();
        }
        public void Connect()
        {
            PhotonNetwork.GameVersion = "0.0.0";
            PhotonNetwork.ConnectUsingSettings();
        }

        public void Join()
        {
            PhotonNetwork.JoinRandomRoom();
        }
         
        public void Switch()
        {
            menu.SetActive(on);
            lvlSelction.SetActive(!on);
            on = !on;
        }

        public void StartGame_POC_1()
        {
            if (PhotonNetwork.IsMasterClient)
            {
                PhotonNetwork.LoadLevel(1);
            }
        }

        public void StartGame_POC_2()
        {
            if (PhotonNetwork.CurrentRoom.PlayerCount == 1)
            {
                PhotonNetwork.LoadLevel(2);
            }
        }

        public void StartGame_POC_3()
        {
            if (PhotonNetwork.CurrentRoom.PlayerCount == 1)
            {
                PhotonNetwork.LoadLevel(3);
            }
        }

        public void StartGame_POC_4()
        {
            if (PhotonNetwork.CurrentRoom.PlayerCount == 1)
            {
                PhotonNetwork.LoadLevel(4);
            }
        }

        public void StartGame_POC_5()
        {
            if (PhotonNetwork.CurrentRoom.PlayerCount == 1)
            {
                PhotonNetwork.LoadLevel(5);
            }
        }

        public void StartGame_POC_6()
        {
            if (PhotonNetwork.CurrentRoom.PlayerCount == 1)
            {
                PhotonNetwork.LoadLevel(6);
            }
        }

        public void StartGame_POC_7()
        {
            if (PhotonNetwork.CurrentRoom.PlayerCount == 1)
            {
                PhotonNetwork.LoadLevel(7);
            }
        }
    }


}