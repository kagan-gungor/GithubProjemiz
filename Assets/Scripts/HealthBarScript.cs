using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBarScript : MonoBehaviour
{
    [SerializeField]
    private RectTransform foregroundImage;
    [SerializeField]
    private RectTransform uiforegroundImage;

    // Use this for initialization
    private GameObject playerCamera;


    void Start()
    {
        playerCamera = GameObject.Find("PlayerCamera");
    }

    //Update is called once per frame
    void Update()
    {
        //if (playerCamera != null)
        //{
        //    transform.LookAt(playerCamera.transform);
        //}



    }

    public void UpdateHealth(float health)
    {
        if (foregroundImage != null)
        {
            foregroundImage.sizeDelta = new Vector2(health * 2, foregroundImage.sizeDelta.y);
            uiforegroundImage.sizeDelta = new Vector2(health * 6, uiforegroundImage.sizeDelta.y);
        }

    }
}
