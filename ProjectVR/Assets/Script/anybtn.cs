using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class anybtn : MonoBehaviour {

    public GameObject fuki;
    public GameObject modesel;

	// Use this for initialization
	void Start () {

        fuki = GameObject.Find("fukidashi");
        modesel = transform.Find("ModeSelect").gameObject;
	}
	
	// Update is called once per frame
	void Update () {

        if (Input.GetButton("Jump") || Input.GetButton("Cross") || Input.GetButton("Circle") || Input.GetButton("Square") || Input.GetButton("Triangle")
             || Input.GetButton("L1") || Input.GetButton("L2") || Input.GetButton("L3")
             || Input.GetButton("R1") || Input.GetButton("R2") || Input.GetButton("R3"))
        {
            //ボタンが押されたら吹き出しを消す。
            fuki.SetActive(false);
            modesel.SetActive(true);
        }

    }
}
