using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NickNameControlScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void setNickName(string nickName)
    {
        GetComponent<Text>().text = nickName;
    }
    public string getNickName()
    {
        return GetComponent<Text>().text;
    }
}
