using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LavaEmission : MonoBehaviour {

    // Use this for initialization
    private Color color = Color.red;
    private float f = 5f;
    private float deltaEmission;
    void Start () {
       
        
        GetComponent<Renderer>().material.color = color;
        GetComponent<Renderer>().material.SetColor("_EmissionColor", color * f);
        //GetComponent<Renderer>().material.EnableKeyword("_EMISSION");
        StartCoroutine(emissionLoop());
    }
	
	// Update is called once per frame
	void Update () {
        
    }

    private IEnumerator emissionLoop()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.1f);
            if (f >= 5f)
                deltaEmission = -0.1f;
            if (f <= 0f)
                deltaEmission = 0.1f;
            f += deltaEmission;
            GetComponent<Renderer>().material.SetColor("_EmissionColor", color * f);
        }
    }
}
