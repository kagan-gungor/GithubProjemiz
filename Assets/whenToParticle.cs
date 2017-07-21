using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class whenToParticle : MonoBehaviour {

    protected bool letPlay = false;

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (!gameObject.GetComponent<ParticleSystem>().isPlaying)
            {
                gameObject.GetComponent<ParticleSystem>().Play();
            }
        }
    }
}
