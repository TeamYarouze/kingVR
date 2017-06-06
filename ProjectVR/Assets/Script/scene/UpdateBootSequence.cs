using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VR;

public class UpdateBootSequence : MonoBehaviour {

    private int counter;

    void OnEnable()
    {
        GameFadeManager.Instance.SetupVRMode(Camera.main.name);
    }

	// Use this for initialization
	void Start () {
		counter = 0;
	}

	// Update is called once per frame
	void Update () {

        if( GameFadeManager.Instance.IsFade() ) return;
		
        if( Input.GetButtonDown("R3") )
        {
            // シーン遷移テスト!!!!!
            GameSceneManager.Instance.ChangeScene(GameModeData.GAMEMODE.GAME_MODE_STAGE);           
        }
	}
}
