using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RoomButton : MonoBehaviour
{
    public Text roomNameText;

    [HideInInspector]
    public string roomName;




    public void SetRoom(string n)
    {
        roomName = n;
        roomNameText.text = roomName;
    }


    public void JoinRoom()
    {
        if(roomNameText.text == "") return;
        PhotonRoomManager.instance.JoinRoom(roomNameText.text);
    }

}
