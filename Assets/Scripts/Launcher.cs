using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

public class Launcher : MonoBehaviourPunCallbacks {
    [SerializeField] private GameObject connectedScreen;
    [SerializeField] private GameObject disconnectedScreen;
    public void OnClick_ConnectedBtn() {
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster() {
        base.OnConnectedToMaster();
        PhotonNetwork.JoinLobby(TypedLobby.Default);
    }
    public override void OnDisconnected(DisconnectCause cause) {
        disconnectedScreen.SetActive(true);
    }
    public override void OnJoinedLobby() {
        if (disconnectedScreen.activeSelf)
            disconnectedScreen.SetActive(false);
        connectedScreen.SetActive(true);
    }
    public override void OnJoinedRoom() {
        base.OnJoinedRoom();
        PhotonNetwork.LoadLevel(1);
    }
}
