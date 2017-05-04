﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VR;

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

        if( Input.GetButtonDown("R1") )
        {
            VRManager.Instance.ChangeVRMode();       
        }

        cameraMngr.ChangeCameraMode();

        if( Input.GetButtonDown("Option") )
        {
            GameSceneManager.Instance.ChangeScene(GameModeData.GAMEMODE.GAME_MODE_BOOT);
        }
	}
}
