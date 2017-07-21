using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleport : MonoBehaviour {

    public GameObject ui;
    public GameObject objToTP;     
    Camera cam;
    int floorMask;
    void Start()
    {
        cam = Camera.main;
        floorMask = LayerMask.GetMask("Floor");
    }

    void Update()
    {
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        Debug.DrawRay(ray.origin, ray.direction * 10, Color.yellow);
        RaycastHit hit;
        if (Physics.Raycast(ray,out hit,200f, floorMask)) {
           

            if (Input.GetKeyDown(KeyCode.E))
            {
                objToTP.transform.position = hit.point;
               // ParticleSystem.Play();
            }
        }

       
    }
    private void OnCollisionStay(Collision collision)
    {
        if(collision.transform.tag == "arena")
        {
            floorMask = LayerMask.GetMask("BlinkAble");
        }
    }

}
