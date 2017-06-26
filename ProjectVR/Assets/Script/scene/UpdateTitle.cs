using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VR;

public class UpdateTitle : UpdateBase {

    private bool bNext;

    new void Awake()
    {
        base.Awake();
    }

	/**
     *  初期化処理です
     */
	new void Start () {
		// 初期化処理をここに書きます
        // 初期化処理なので実行されるのはシーン開始時の1回だけです

        if( !VRSettings.enabled )
        {
            VRManager.Instance.ChangeVRMode();
        }

        bNext = false;
	}
	
	/**
     * 毎フレームの更新処理です
     */
	new void Update () {
		// 毎フレームの更新処理をここに書きます

        if( !bNext )
        {

            if( Input.GetButtonDown("Circle") || Input.GetButtonDown("Cross")
                || Input.GetButtonDown("Square") || Input.GetButtonDown("Triangle") )
            {
                GameSceneManager.Instance.ChangeScene(GameModeData.GAMEMODE.GAME_MODE_STAGE);
                bNext = true;
            }
        }
	}
}
