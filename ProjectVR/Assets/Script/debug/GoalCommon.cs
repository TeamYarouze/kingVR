using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalCommon : MonoBehaviour {

    private MeshRenderer goalRenderer;

	// Use this for initialization
	void Start () {
		goalRenderer = gameObject.GetComponent<MeshRenderer>();
        goalRenderer.enabled = false;
	}
	
	// Update is called once per frame
	void Update () {
		
        if( Input.GetButtonDown("L1") )
        {
            if( goalRenderer.enabled )
            {
                goalRenderer.enabled = false;
            }
            else
            {
                goalRenderer.enabled = true;
            }
        }

	}
}
