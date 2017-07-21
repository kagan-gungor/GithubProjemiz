using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LobbyManager : MonoBehaviour
{

    [SerializeField]
    GameObject userIDPanel;
    [SerializeField]
    GameObject chatPanel;
    [SerializeField]
    GameObject oyunArama;
    [SerializeField]
    GameObject connectingObject;
    [SerializeField]
    GameObject statusLabel;
    [SerializeField]
    GameObject userNameLabel;
    [SerializeField]
    Text userNameText;
    [SerializeField]
    Text lobbyPlayerCount;


    public bool inlobby;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        statusLabel.GetComponent<Text>().text = PhotonNetwork.connectionStateDetailed.ToString();
    }

    public void ConnectClick()
    {
        PhotonNetwork.logLevel = PhotonLogLevel.Full;
        PhotonNetwork.ConnectUsingSettings("0.1");
        PhotonNetwork.autoJoinLobby = true;
        PhotonNetwork.automaticallySyncScene = true;

        userIDPanel.SetActive(false);
        connectingObject.SetActive(true);
        statusLabel.SetActive(true);

    }

    public void OnJoinedLobby()
    {
        inlobby = true;
        connectingObject.SetActive(false);
        oyunArama.SetActive(true);
        userNameLabel.SetActive(true);
        userNameLabel.GetComponent<Text>().text = userNameText.text;
        //lobbyPlayerCount.text = PhotonNetwork.playerList.Length+"";
        PhotonNetwork.player.NickName = userNameText.text;

    }

    public void GameSearchClick()
    {
        if (inlobby)
        {
            if (PhotonNetwork.GetRoomList().Length == 0)
            {
                RoomOptions ro = new RoomOptions() { IsVisible = true, MaxPlayers = 2};
                Debug.Log(PhotonNetwork.GetRoomList().Length);
                PhotonNetwork.JoinOrCreateRoom(PhotonNetwork.player.NickName+"Room", ro, TypedLobby.Default);
            }else
            {
                PhotonNetwork.JoinRoom(PhotonNetwork.GetRoomList()[0].Name);
            }
            

        }
    }

    public IEnumerator oyuncuSay()
    {
        while (true)
        {
            yield return new WaitForSeconds(1f);
            if (PhotonNetwork.playerList.Length == 2)
            {
                PhotonNetwork.room.IsVisible = false;
            }else
            {
                PhotonNetwork.room.IsVisible = true;
            }
        }
        

    }

    void OnJoinedRoom()
    {
        inlobby = false;
        oyunArama.SetActive(false);
        chatPanel.SetActive(true);
        Debug.Log("Player size" + PhotonNetwork.playerList.Length);
        Debug.Log("oda adi" + PhotonNetwork.room.Name);
        if (PhotonNetwork.isMasterClient)
        {
            StartCoroutine(oyuncuSay());
        }
    }

    void OnPhotonJoinRoomFailed()
    {
        Debug.Log("Patladi");
    }

    private void OnApplicationQuit()
    {
        PhotonNetwork.Disconnect();
    }
}
