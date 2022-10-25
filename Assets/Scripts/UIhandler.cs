using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;
using TMPro;
public class UIhandler : MonoBehaviourPunCallbacks {
    public TMP_InputField createRoomTF;
    public TMP_InputField joinRoomTF;
    public void OnClick_JoinRoom() {
        PhotonNetwork.JoinRoom(joinRoomTF.text, null);
    }
    public void OnClick_CreateRoom() {
        PhotonNetwork.CreateRoom(createRoomTF.text, new RoomOptions { MaxPlayers = 4 }, null);
    }
    public override void OnJoinedRoom() {
        print("Room Joined Sucess");
    }
    public override void OnJoinRandomFailed(short returnCode,
    string message) {
        print("RoomFaild" + returnCode + "Message" + message);
    }
}
