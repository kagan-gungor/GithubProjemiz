using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityStandardAssets.Effects;

public class BuyuYapma : Photon.MonoBehaviour
{
    public GameObject olusturalacakNesne;
    public GameObject namlu;
    public GameObject buyucu;
    public float atesDolumSuresi;
    public float blinkDolumSuresi;
    public float blastDolumSuresi;
    public Image fireBallSkill;
    public Image blinkSkill;
    public Image blastSkill;
    public Camera myCam;


    public float dagitmaRadius;
    public float dagitmaGucu;

    private bool isFireEnable;
    private bool isBlinkEnable;
    private bool isBlastEnable;


    // Use this for initialization
    void Start()
    {
        isFireEnable = true;
        isBlinkEnable = true;
        isBlastEnable = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (photonView.isMine)
        {
            //if (!(atesSuresi < 0))
            //    atesSuresi -= Time.deltaTime;

            if (Input.GetMouseButtonDown(0))
            {
                if (isFireEnable)
                {
                    GameObject go = PhotonNetwork.Instantiate("Firebolt", namlu.transform.position, buyucu.transform.rotation, 0);
                    isFireEnable = false;
                    skillFillProcces(1, fireBallSkill, atesDolumSuresi);
                    StartCoroutine(bekletVeYokEt(go));
                }
            }
            if (Input.GetKeyDown(KeyCode.Space))
            {
                if (isBlinkEnable)
                {
                    Ray ray = myCam.ScreenPointToRay(Input.mousePosition);
                    Debug.DrawRay(ray.origin, ray.direction * 10, Color.yellow);
                    RaycastHit hit;
                    if (Physics.Raycast(ray, out hit, 200f))
                    {
                        transform.parent.GetComponent<PhotonView>().RPC("StartParticleAnnimation", PhotonTargets.All, "blink");
                        isBlinkEnable = false;
                        transform.position = hit.point;
                        skillFillProcces(2, blinkSkill, blinkDolumSuresi);
                    }
                }
            }
            if (Input.GetKeyDown(KeyCode.Q))
            {
                if (isBlastEnable)
                {
                    Vector3 explosionPos = transform.position - new Vector3(0f, 0.5f, 0f);
                    Collider[] colliders = Physics.OverlapSphere(explosionPos, dagitmaRadius);
                    transform.parent.GetComponent<PhotonView>().RPC("StartParticleAnnimation", PhotonTargets.All, "blast");
                    isBlastEnable = false;
                    skillFillProcces(3, blastSkill, blastDolumSuresi);
                    //Debug.Log("Vurdu");
                    foreach (Collider hit in colliders)
                    {
                        Rigidbody rb = hit.GetComponent<Rigidbody>();

                        if (rb != null && !rb.Equals(GetComponent<Rigidbody>()))
                        {
                            //Debug.LogError(hit.GetComponent<PhotonView>().owner.ID);
                            //float lastDagitmaGucu = Mathf.Abs(dagitmaRadius-Vector3.Distance(hit.transform.position, transform.position))*dagitmaGucu;

                            hit.transform.parent.GetComponent<PhotonView>().RPC("GetShot", PhotonTargets.All, 10, GetComponent<PhotonView>().ownerId);
                            hit.transform.parent.GetComponent<PhotonView>().RPC("DagitmaSkill", hit.GetComponent<PhotonView>().owner, dagitmaGucu, explosionPos);
                            //rb.AddExplosionForce(dagitmaGucu, explosionPos, dagitmaRadius);
                            //Debug.Log("Vurdu gol oldu");
                        }

                    }
                }
            }

        }
    }


    private void skillFillProcces(int skillNumber, Image imageSkill, float dolumSuresi)
    {
        imageSkill.fillAmount = 0.0f;
        StartCoroutine(skillUiUpdate(skillNumber, imageSkill, dolumSuresi));
    }

    private IEnumerator skillUiUpdate(int skillNumber, Image imageSkill, float dolumSuresi)
    {
        while (true)
        {
            yield return new WaitForSeconds(dolumSuresi / 100);
            imageSkill.fillAmount += 0.01f;
            if (imageSkill.fillAmount == 1)
            {
                if (skillNumber == 1)
                {
                    isFireEnable = true;
                }
                else if (skillNumber == 2)
                {
                    isBlinkEnable = true;
                }
                else if (skillNumber == 3)
                {
                    isBlastEnable = true;
                }
                break;
            }

        }

    }
    private IEnumerator bekletVeYokEt(GameObject go)
    {
        yield return new WaitForSeconds(2f);
        PhotonNetwork.Destroy(go);
    }
}
