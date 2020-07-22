using UnityEngine;
using Photon.Pun;
using Photon.Realtime;


namespace com.PT.contest
{

    public class Launcher : MonoBehaviourPunCallbacks
    {
        public GameObject menu, lvlSelction;
        bool on;

        public void Awake()
        {
            if (Screen.fullScreen) Screen.fullScreen = false;
            PhotonNetwork.AutomaticallySyncScene = true;
            Connect();
        }

        public override void OnConnectedToMaster()
        {
            base.OnConnectedToMaster();
            Join();
        }

        public override void OnJoinedRoom()
        {
            base.OnJoinedRoom();
        }

        public override void OnJoinRandomFailed(short returnCode, string message)
        {
            base.OnJoinRandomFailed(returnCode, message);
            Create();
        }

        public void Create()
        {
            PhotonNetwork.CreateRoom("", new RoomOptions { MaxPlayers = 3 });
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
            if (PhotonNetwork.CurrentRoom.PlayerCount == 1)
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