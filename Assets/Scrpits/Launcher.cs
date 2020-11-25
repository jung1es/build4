using UnityEngine;
using Photon.Pun;
using Photon.Realtime;


namespace com.PT.contest
{

    public class Launcher : MonoBehaviour
    {
        public GameObject menu, lvlSelction,roomMenu;
        bool on;

        private void OnEnable()
        {
            PhotonRoomManager.instance.OnConnectMaster          += ConnectToMaster;
            PhotonRoomManager.instance.OnCreatedRoomSuccesful   += RoomCreated;
        }
        private void OnDisable()
        {
            PhotonRoomManager.instance.OnConnectMaster          -= ConnectToMaster;
            PhotonRoomManager.instance.OnCreatedRoomSuccesful   -= RoomCreated;
        }

      

        void ConnectToMaster()
        {
            if (Screen.fullScreen) Screen.fullScreen = false;
            roomMenu.SetActive(true);

        }

       void RoomCreated()
        {
            menu.GetComponent<CanvasGroup>().interactable = true;
            roomMenu.SetActive(false);
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