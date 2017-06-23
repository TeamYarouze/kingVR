using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

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

    private GameObject playerObj;

    // ステージステート
    enum StageState
    {
        STATE_PREPARE,
        STATE_READY,
        STATE_INGAME,
        STATE_GOAL,
        STATE_FAILE,
        STATE_GAMEOVER,
        STATE_RESET,

        STATE_NUM,
    };
    private StageState m_state;
    private int m_counterSec;                  // 汎用カウンター
    private float m_frame;

    private int m_subStep;

    void Awake()
    {
    }

    void OnEnable()
    {
        cameraMngr = new CameraManager();
        cameraMngr.Initialize();
        GameFadeManager.Instance.SetupVRMode(Camera.main.name);

        m_stage = GameModeData.StageCount;      // ステージ数を設定
        m_counterSec = 0;
        m_frame = 0.0f;
    }

	// Use this for initialization
	void Start () {
        playerObj = GameObject.Find("kings");
        DustStorm = GameObject.Find("DustStorm");

        Assert.IsNotNull(playerObj, "Player Object Is Null!!!!!!!!!!!!!!!!!!!!");    
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
        m_frame += Time.deltaTime;
        if( m_frame >= 1.0f )
        {
            m_frame = 0.0f;
            m_counterSec++;
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
        m_subStep = 0;
        m_frame = 0.0f;
        m_counterSec = 0;
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
                // 最初は自動で吹っ飛ぶ
                playerObj.GetComponent<CharAction>().FirstBlowoff();

                ChangeState(StageState.STATE_INGAME);
            }

            break;
        case StageState.STATE_INGAME:

            if( playerObj.GetComponent<CharAction>().IsGoal )
            {
                uiManager.ScrGoalUI.StartGoalUI();

                ChangeState(StageState.STATE_GOAL);
            }
            else if( playerObj.GetComponent<CharAction>().IsGameOver )
            {
                ChangeState(StageState.STATE_GAMEOVER);
            }

            break;

        case StageState.STATE_GOAL:

            // とりあえず10秒ほど経過したら
            if( m_counterSec > 10 )
            {
                ChangeState(StageState.STATE_RESET);
            }

            break;

        case StageState.STATE_RESET:

            bool resetDone = OnEasyReset();
            if( resetDone )
            {
                ChangeState(StageState.STATE_PREPARE);
            }
            break;

        default:
            break;
        }

        CountOneFrame();
    }

    //---------------------------------------------------------------
    /*
        @brief      リセット処理
    */
    //---------------------------------------------------------------
    private bool OnEasyReset()
    {
        bool ret = false;

        const int RESET_FADEOUTSTART = 0;
        const int RESET_FADEOUTWAIT = 1;
        const int RESET_FADEINWAIT = 3;

        VRUIManager uiManager = cameraMngr.FpsCameraObj.GetComponent<VRUIManager>();

        switch( m_subStep )
        {
        // フェードアウト開始
        case RESET_FADEOUTSTART:
            GameSceneManager.Instance.StartFadeOut(24);
            m_subStep = RESET_FADEOUTWAIT;
            break;
        case RESET_FADEOUTWAIT:
            if( !GameSceneManager.Instance.IsFade )
            {
                /*
                    ToDo リセット処理
                */
                playerObj.GetComponent<CharAction>().ResetPosition(true);
                uiManager.ScrGoalUI.StopGoalUI();

                GameSceneManager.Instance.StartFadeIn(24);
                m_subStep = RESET_FADEINWAIT;
            }
            break;
        case RESET_FADEINWAIT:

            if( !GameSceneManager.Instance.IsFade )
            {
                ret = true;
            }

            break;
        }

        return ret;
    }


}
