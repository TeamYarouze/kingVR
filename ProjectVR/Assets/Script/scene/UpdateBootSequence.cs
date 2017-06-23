using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VR;

public class UpdateBootSequence : UpdateBase {

    private float counter;
    private int m_step;

    void OnEnable()
    {
        GameFadeManager.Instance.SetupVRMode(Camera.main.name);
    }

	// Use this for initialization
	void Start () {
		counter = 0;
        m_step = 0;
	}

	// Update is called once per frame
	void Update () {

        if( GameFadeManager.Instance.IsFade() )
        {
            return;
        }

        switch( m_step )
        {
        case 0:

            counter += Time.deltaTime;
		
            if( counter >= 1.0f )
            {
                // シーン遷移テスト!!!!!
                GameSceneManager.Instance.ChangeScene(GameModeData.GAMEMODE.GAME_MODE_TITLE);
                m_step = 1;
            }
            break;
        case 1:
        default:
            break;
        }

	}
}
