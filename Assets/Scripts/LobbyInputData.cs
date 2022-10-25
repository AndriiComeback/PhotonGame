using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobbyInputData : MonoBehaviour
{
    [SerializeField] TMPro.TMP_InputField playerNameInput;
    private void Start() {
        DontDestroyOnLoad(this);
    }
    public void UpdateName() {
        PhotonNetwork.LocalPlayer.NickName = playerNameInput.text;
    }
}
