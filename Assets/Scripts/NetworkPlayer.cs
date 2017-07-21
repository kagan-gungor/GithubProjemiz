using Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class NetworkPlayer : Photon.MonoBehaviour
{
    [SerializeField]
    private GameObject PlayerCamera;
    [SerializeField]
    private GameObject CanvasUI;
    [SerializeField]
    private GameObject Buyucu;
    [SerializeField]
    private GameObject ParticleController;
    [SerializeField]
    public Text PlayerStateInfo;
    [SerializeField]
    private float health;
    [SerializeField]
    private string NickName;
    [SerializeField]
    public bool isDeath;

    private bool receiveIsDeath;
    public delegate void SendDeathNews(PhotonPlayer DeathPlayer);
    public event SendDeathNews sendNews;

    private HealthBarScript myHealthBarScript;
    private float ReceiveHealth;
    
    //private OyuncuHealth myOyuncuHealth;


    // Use this for initialization
    void Start()
    {
        PhotonNetwork.sendRate = 20;
        PhotonNetwork.sendRateOnSerialize = 10;
        myHealthBarScript = GetComponentInChildren<HealthBarScript>();
        myHealthBarScript.UpdateHealth(health);
        isDeath = false;
        if (photonView.isMine)
        {
            GetComponentInChildren<Rigidbody>().useGravity = true;
            GetComponentInChildren<Rigidbody>().isKinematic = false;
            GetComponentInChildren<BuyuYapma>().enabled = true;
            GetComponentInChildren<PlayerMovement>().enabled = true;
            PlayerCamera.SetActive(true);
            CanvasUI.SetActive(true);
        }
        else
        {
            StartCoroutine(UpdateLoop());
        }
    }


    IEnumerator UpdateLoop()
    {
        while (true)
        {
            yield return null;
            myHealthBarScript.UpdateHealth(ReceiveHealth);
            isDeath = receiveIsDeath;
        }
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.isWriting)
        {
            stream.SendNext(health);
            stream.SendNext(isDeath);
        }
        else
        {
            ReceiveHealth = (float)stream.ReceiveNext();  // pos gets filled-in. must be used somewhere
            isDeath = (bool)stream.ReceiveNext();
        }
    }


    public void GetBurn(float damage)
    {
        health -= damage;
        if (health <= 0 && photonView.isMine)
        {
            Buyucu.GetComponent<CapsuleCollider>().enabled = false;
            Buyucu.GetComponent<BuyuYapma>().enabled = false;
            Buyucu.GetComponent<PlayerMovement>().enabled = false;
            PlayerCamera.GetComponent<CameraTakip>().target = null;
            isDeath = true;
            PlayerStateInfo.text = "Yandın sanırım";
        }
        myHealthBarScript.UpdateHealth(health);
    }

    public void ResetPlayerData()
    {
        if (photonView.isMine)
        {
            Buyucu.transform.localPosition = Vector3.zero;
            Buyucu.transform.localRotation = Quaternion.identity;
            isDeath = false;
            Buyucu.GetComponent<CapsuleCollider>().enabled = true;
            Buyucu.GetComponent<BuyuYapma>().enabled = true;
            Buyucu.GetComponent<PlayerMovement>().enabled = true;
            PlayerCamera.GetComponent<CameraTakip>().target = Buyucu.transform;
            //GameObject.Find("KaraParcasi").transform.localScale = new Vector3(50, 1, 50);
            health = 100;
            myHealthBarScript.UpdateHealth(health);
        }
    }

    [PunRPC]
    public void GetShot(int damage, int ownerID)
    {
        health -= damage;

        if (health <= 0 && photonView.isMine)
        {
            //if (SendNetworkMessage != null)
            //    SendNetworkMessage(PhotonNetwork.player.name + " was killed by " + enemyName);
            //if (RespawnMe != null)
            //    RespawnMe(3f);
            if (sendNews != null)
                sendNews(PhotonNetwork.player);
            Buyucu.GetComponent<CapsuleCollider>().enabled = false;
            Buyucu.GetComponent<BuyuYapma>().enabled = false;
            Buyucu.GetComponent<PlayerMovement>().enabled = false;
            PlayerCamera.GetComponent<CameraTakip>().target = null;
            isDeath = true;
            //PhotonNetwork.Destroy(Buyucu);
            //PhotonNetwork.player.CustomProperties["isDeath"] = true;
            PlayerStateInfo.text = "Öldün sanırım";
        }
        myHealthBarScript.UpdateHealth(health);
    }

    [PunRPC]
    public void StartParticleAnnimation(string animationName)
    {
        if(animationName.Equals("blink"))
            ParticleController.GetComponent<ParticleController>().startBlinkAnimation();
        else if (animationName.Equals("blast"))
            ParticleController.GetComponent<ParticleController>().startBlastAnimation();
    }
    [PunRPC]
    public void DagitmaSkill(float dagitmaGucu,Vector3 explosionPos)
    {
        //Debug.LogError("Kullanıldı"+photonView.owner.ID);
        if(Buyucu.GetComponent<PhotonView>().isMine)
        {
            //Debug.LogError("wtf");
            Buyucu.GetComponent<Rigidbody>().AddForce((Buyucu.transform.position-explosionPos)*dagitmaGucu);
        }
    }

    public void setNickName(string nickName) {
        GetComponentInChildren<NickNameControlScript>().setNickName(nickName);
    }
}
// relay server
// host migration
