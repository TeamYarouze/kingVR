using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpdateBootSequence : MonoBehaviour {

    private int counter;
	// Use this for initialization
	void Start () {
		counter = 0;
	}
	
	// Update is called once per frame
	void Update () {
		
        if( counter < 15 )
        {
            counter++;
            // 適当に15フレーム後
            if( counter == 15 )
            {
                // シーン遷移テスト!!!!!
                GameSceneManager.Instance.ChangeScene(GameModeData.GAMEMODE.GAME_MODE_STAGE);           

            }
        }
	}
}
