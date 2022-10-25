using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Manager : MonoBehaviour
{
    [SerializeField] private GameObject playerPrefab;
    [SerializeField] private GameObject mainCameraObj;
    [SerializeField] private TMPro.TMP_Text info;
    [SerializeField] private GameObject ghostPrefab;
    bool waitingForPlayers = true;
    public void CreatePlayer() {
        GameObject player = PhotonNetwork.Instantiate(playerPrefab.name, playerPrefab.transform.position 
            + new Vector3(Random.Range(-5, 5), 0), Quaternion.identity);
        
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        foreach (GameObject pl in players) {
            pl.GetComponent<MyPlayer>().SetName(pl.GetComponent<PhotonView>().Owner.NickName);
        }
    }
    private void Awake() {
        info.text = "WAITING FOR OTHER PLAYER";
    }
    IEnumerator Info() {
        info.text = "GHOST INCOMING FROM LEFT SIDE!!! RUN AWAY TOGETHER!!!";
        yield return new WaitForSeconds(5f);
        info.text = "";
    }
    private void Start() {
        Physics2D.IgnoreLayerCollision(3, 3);
        mainCameraObj.SetActive(false);
        CreatePlayer();
    }
    private void Update() {
        if (waitingForPlayers) {
            if (PhotonNetwork.CurrentRoom.PlayerCount > 1) {
                GameObject player = PhotonNetwork.Instantiate(ghostPrefab.name, ghostPrefab.transform.position, Quaternion.identity);
                StartCoroutine(Info());
                waitingForPlayers = false;
            }
        }
    }
}
