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
            menu.GetComponent<CanvasGroup>().interactable = true;
            PhotonNetwork.JoinRandomRoom();
        }

       
        public override void OnCreateRoomFailed(short returnCode, string message)
        {
            Debug.Log("FAILED TO CREATE ROOM");
        }
        public override void OnJoinRandomFailed(short returnCode, string message)
        {
            Debug.Log("Failed to join room");
            Create();
        }

        public void Create()
        {
            // max player of rooms is set to 3
            RoomOptions roomOps = new RoomOptions() { IsVisible = true, IsOpen = true, MaxPlayers = (byte)3 };
            int roomRandomNumber = Random.Range(0,999);
            PhotonNetwork.CreateRoom("Room " + roomRandomNumber, roomOps);
        }

      
        public void Connect()
        {
            PhotonNetwork.GameVersion = "0.0.0";
            PhotonNetwork.ConnectUsingSettings();
        }

         
        public void Switch()
        {
            menu.SetActive(on);
            lvlSelction.SetActive(!on);
            on = !on;
        }
        public GameObject MenuHolder;
        public void HideOrShowMenu(bool _state)
        {
            MenuHolder.SetActive(_state);
        }
       

        public void StartGame_POC(int value)
        {

            PhotonNetwork.LoadLevel(value);

        }
    }


}