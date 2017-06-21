using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpdateStage : UpdateBase {

    private GameObject DustStorm;
    public ParticleSystemBase particlBase
    {
        get { return (DustStorm) ? DustStorm.GetComponent<ParticleSystemBase>() : null; }
    }

    private int m_stage;            // ステージ数
    public int Stage
    {
        set { m_stage = value; }
        get { return m_stage;  }
    }

    // ステージステート
    enum StageState
    {
        STATE_PREPARE,
        STATE_READY,
        STATE_INGAME,
        STATE_GOAL,
        STATE_FAILE,
        STATE_GAMEOVER,

        STATE_NUM,
    };
    private StageState m_state;
    private int m_counter;                  // 汎用カウンター
    private float m_frame;

    void Awake()
    {
    }

    void OnEnable()
    {
        cameraMngr = new CameraManager();
        cameraMngr.Initialize();
        GameFadeManager.Instance.SetupVRMode(Camera.main.name);

        m_stage = GameModeData.StageCount;      // ステージ数を設定
        m_counter = 0;
        m_frame = 0.0f;
    }

	// Use this for initialization
	void Start () {

        DustStorm = GameObject.Find("DustStorm");
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

        // ステージの状態を更新
        UpdateStageState();
	}


    public void PlayDustStorm(bool bPlay)
    {
        if( !particlBase ) return;

        if( bPlay )
        {
            particlBase.Play();
        }
        else
        {
            particlBase.Stop();
        }
    }

    public bool IsPlayDustStorm()
    {
        if( !particlBase ) return false;

        return particlBase.IsPlay();
    }

    private void CountOneFrame()
    {
        m_frame += GameDefine.FPSDeltaScale();
        if( m_frame >= 1.0f )
        {
            m_frame = 0.0f;
            m_counter++;
        }
    }
  

    //---------------------------------------------------------------
    /*
        @brief      ステートの変更
    */
    //---------------------------------------------------------------
    private void ChangeState(StageState state)
    {
        m_state = state;
        m_frame = 0.0f;
        m_counter = 0;
    }

    //---------------------------------------------------------------
    /*
        @brief      ステージの状態を更新
    */
    //---------------------------------------------------------------
    void UpdateStageState()
    {
        VRUIManager uiManager = cameraMngr.FpsCameraObj.GetComponent<VRUIManager>();

        switch( m_state )
        {
        case StageState.STATE_PREPARE:
            
            uiManager.ScrCountDown.StartCountDown();
            ChangeState( StageState.STATE_READY );

            break;
        case StageState.STATE_READY:

            if( uiManager.ScrCountDown.IsFinish )
            {
                ChangeState(StageState.STATE_INGAME);
            }

            break;
        case StageState.STATE_INGAME:
            break;

        default:
            break;
        }

        CountOneFrame();
    }


}
