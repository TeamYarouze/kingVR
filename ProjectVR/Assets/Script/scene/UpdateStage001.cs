using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VR;

public class UpdateStage001 : Singleton<UpdateStage001> {

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
        GameFadeManager.Instance.SetupVRMode(Camera.main.name);
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

        if( Input.GetButtonDown("R3") )
        {
            GameSceneManager.Instance.ChangeScene(GameModeData.GAMEMODE.GAME_MODE_BOOT);
        }
	}

}
