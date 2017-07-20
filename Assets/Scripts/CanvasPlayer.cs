using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasPlayer : MonoBehaviour {

    // Use this for initialization
    public Transform target;            // The position that that camera will be following.
    //public float smoothing = 5f;        // The speed with which the camera will be following.


    Vector3 offset;                     // The initial offset from the target.


    void Start()
    {
        // Calculate the initial offset.

    }

    void FixedUpdate()
    {
        if (target != null)
        {
            // Create a postion the camera is aiming for based on the offset from the target.
            //Vector3 targetCamPos = target.position + new Vector3(-17f, 21f, 0f);

            // Smoothly interpolate between the camera's current position and it's target position.
            //transform.position = Vector3.Lerp(transform.position, targetCamPos, smoothing * Time.deltaTime);
            transform.position = target.position+ new Vector3(0f, 4f, 0f);
        }

    }
}
