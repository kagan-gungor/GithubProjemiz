using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeScale : MonoBehaviour {

    [SerializeField]
    private float kuculmeKatsayisi;
    [SerializeField]
    private float minScale;
	// Use this for initialization
	void Start () {
        
    }
	
	// Update is called once per frame
	void Update () {
	}

    public void kuculmeyiBaslat()
    {
        StartCoroutine(scaleLoop());
    }

    private IEnumerator scaleLoop()
    {
        while (true)
        {
            yield return new WaitForSeconds(1);
            if (transform.localScale.x > minScale)
            {
                transform.localScale -= new Vector3(kuculmeKatsayisi, 0, kuculmeKatsayisi);
            }else
            {
                break;
            }
        }
    } 
}
