using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpdateBase : Singleton<UpdateBase> {

    protected CameraManager cameraMngr = null;
    public CameraManager CameraMngr
    {
        get { return cameraMngr; }
    }

    void Awake()
    {
        base.Awake();
    }

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
