using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class loading_left : MonoBehaviour {

	// Use this for initialization
	void Start () {

        iTween.RotateBy(this.gameObject, iTween.Hash("z", -1, "easeType", "linear", "loopType", "loop", "time", 5));

    }

    // Update is called once per frame
    void Update () {
		
	}
}
