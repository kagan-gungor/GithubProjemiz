using System;
using System.Collections;
using System.Collections.Generic;
using ExitGames.Client.Photon;
using UnityEngine;
using UnityEngine.UI;

public class _networkControlScript : Photon.MonoBehaviour
{
    [SerializeField]
    private int PlayerSize;
    [SerializeField]
    Text networkStateText;
    [SerializeField]
    Text gameStateText;
    [SerializeField]
    Camera sceneCamera;
    [SerializeField]
    Camera playerCamera;
    [SerializeField]
    Transform[] spawnPoints;
    [SerializeField]
    private bool isRoundEnd;
    [SerializeField]
    GameObject myWizard;
    [SerializeField]
    private Text pingText;
    [SerializeField]
    private int RoundCount;
    [SerializeField]
    private GameObject[] players;


    private bool isRoundStart;
    private Text RoundCountText;
    // Use this for initialization
    void Awake()
    {
        PhotonNetwork.logLevel = PhotonLogLevel.Full;
        PhotonNetwork.ConnectUsingSettings("0.1");
        PhotonNetwork.autoJoinLobby = true;
        PhotonNetwork.automaticallySyncScene = true;
    }
    private void Start()
    {
        isRoundEnd = false;
        isRoundStart = false;
        RoundCount = 1;
        RoundCountText = GameObject.Find("RoundCount").GetComponent<Text>();
    }
    // Update is called once per frame
    void Update()
    {
        networkStateText.text = PhotonNetwork.connectionStateDetailed.ToString();
        pingText.text = "Ping :" + PhotonNetwork.GetPing();

        if (PhotonNetwork.isMasterClient && isRoundStart)
        {
            OyunculariTopla();
            RoundEndHandle();

        }


    }

    private void RoundEndHandle()
    {
        int deathPlayerCount = 0;
        GameObject kazananOyuncu = null;
        foreach (GameObject go in players)
        {
            if (go.GetComponent<NetworkPlayer>().isDeath)
            {
                deathPlayerCount++;
            }
            else
            {
                kazananOyuncu = go;
            }
        }
        //PhotonPlayer livePlayer = null;
        if (players.Length - deathPlayerCount == 1)
        {
            Debug.Log("Bitti" + kazananOyuncu.name);
            photonView.RPC("RoundEnded", PhotonTargets.All);
            isRoundStart = false;
            //foreach (PhotonPlayer pPlayer in PhotonNetwork.playerList)
            //{
            //    if (pPlayer.ID == players[0].GetComponent<PhotonView>().ownerId)
            //    {
            //        livePlayer = pPlayer;
            //    }
            //}
            //Debug.Log(livePlayer.NickName + " kazanan oyuncumuz");
            //if (kazananOyuncu != null)
            //    kazananOyuncu.GetComponent<NetworkPlayer>().PlayerStateInfo.text = "Hacı kazandın sen";
        }



        //if (deathPlayerCount == PhotonNetwork.playerList.Length - 1)
        //{
        //    isRoundEnd = true;
        //    isRoundStart = false;

        //    if (livePlayer != null)
        //
        //    //GameEnded();
        //}
    }

    [PunRPC]
    void RoundEnded()
    {
        if (myWizard != null)
        {
            if (myWizard.GetComponent<NetworkPlayer>().isDeath)
            {
                myWizard.GetComponent<NetworkPlayer>().PlayerStateInfo.text = "Kaybettin Dostum";
            }
            else
            {
                myWizard.GetComponent<NetworkPlayer>().PlayerStateInfo.text = "Kazandın kardeşş :)";
            }
        }
        Invoke("RoundReset", 5f);
        if (PhotonNetwork.isMasterClient)
            RoundCount++;
    }

    void OnJoinedLobby()
    {
        RoomOptions ro = new RoomOptions() { IsVisible = true, MaxPlayers = (byte)PlayerSize };
        PhotonNetwork.JoinOrCreateRoom("LobbyDeneme", ro, TypedLobby.Default);
    }
    void OnJoinedRoom()
    {
        ExitGames.Client.Photon.Hashtable ht = new ExitGames.Client.Photon.Hashtable();
        ht.Add("isDeath", false);
        PhotonNetwork.player.SetCustomProperties(ht);
        PhotonNetwork.player.NickName = "Sihirbaz" + PhotonNetwork.player.ID;
        if (PhotonNetwork.playerList.Length == PlayerSize)
        {
            photonView.RPC("SpawnPlayerProcess", PhotonTargets.All);
        }
        else
        {
            gameStateText.text = "Diğer Oyuncular Bekleniyor";
        }
    }


    [PunRPC]
    void SpawnPlayerProcess()
    {
        playerSpawn(spawnPoints);
        isRoundStart = true;
        //if (PhotonNetwork.isMasterClient)
        //{
        //    GameObject.FindGameObjectWithTag("saveArea").GetComponent<ChangeScale>().kuculmeyiBaslat();
        //}
    }

    void OyunculariTopla()
    {
        players = GameObject.FindGameObjectsWithTag("PlayerPrefab");
    }
    private void playerSpawn(Transform[] spawnPointsTemp)
    {
        GameObject player;
        int indis = PhotonNetwork.player.ID - 1;
        player = PhotonNetwork.Instantiate("MyPlayer",
                                         spawnPointsTemp[indis].position,
                                         spawnPointsTemp[indis].rotation,
                                         0);
        if (player.GetComponent<PhotonView>().isMine)
        {
            myWizard = player;
            myWizard.GetComponent<NetworkPlayer>().setNickName(PhotonNetwork.player.NickName);
        }else
        {
            player.GetComponent<NetworkPlayer>().setNickName(player.GetComponent<PhotonView>().owner.NickName+"---");
        }
        
        gameStateText.text = "";
        RoundCountText.text = RoundCount + ". Round";
        sceneCamera.enabled = false;
    }

    private void RoundReset()
    {
        myWizard.GetComponent<NetworkPlayer>().ResetPlayerData();

        RoundCountText.text = RoundCount + ". Round";
        myWizard.GetComponent<NetworkPlayer>().PlayerStateInfo.text = "";
        isRoundStart = true;
        //sceneCamera.enabled = false;
    }
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.isWriting)
        {
            stream.SendNext(RoundCount);
        }
        else
        {
            RoundCount = (int)stream.ReceiveNext();
        }
    }
}
