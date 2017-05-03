using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpdateStage000 : Singleton<UpdateStage000> {

    private CameraManager cameraMngr = null;
    public CameraManager CameraMngr
    {
        get { return cameraMngr; }
    }

    void Awake()
    {
        base.Awake();
    }

    void OnEnable()
    {
        cameraMngr = new CameraManager();
        cameraMngr.Initialize();
    }

	// Use this for initialization
	void Start () {
	    
	}
	
	// Update is called once per frame
	void Update () {

        cameraMngr.ChangeCameraMode();
	}
}
