using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.VR;
#if UNITY_PS4
using UnityEngine.PS4;
#endif  //

public class GameSceneManager : Singleton<GameSceneManager> {


    public void Awake()
    {
        Debug.Log("--------------- GameSceneManager Instance - ");

        base.Awake();
        DontDestroyOnLoad(this.gameObject);
    }

	void Start () {
        Debug.Log("------------- GameSceneManager Start \n");
        
	}
	
	void Update ()
    {


	}

    //---------------------------------------------------------------
    /*
        @brief      シーンを遷移する
        @param[in]  GameModeData.GAMEMODE 遷移先シーン
    */
    //---------------------------------------------------------------
    public void ChangeScene(GameModeData.GAMEMODE next)
    {
        Debug.Log("Change Scene Start [ To: " + GameModeData.GameMode + " From: " + next + " ]");

        StartCoroutine(LoadSceneAsync(next));
    }

    IEnumerator LoadSceneAsync(GameModeData.GAMEMODE next)
    {
        // フェードアウトしてから
        GameFadeManager.Instance.StartFade(GameFadeManager.FadeType.FADE_OUT, 30);
        while( GameFadeManager.Instance.IsFade() )
        {
            yield return null;
        }

        // シーンの非同期読み込み
        string sceneName = GameResourcePath.GetSceneName(next);
        AsyncOperation asyncOP = SceneManager.LoadSceneAsync(sceneName);
        while( !asyncOP.isDone )
        {
            yield return null;
        }
        
        // フェードイン
        GameFadeManager.Instance.StartFade(GameFadeManager.FadeType.FADE_IN, 60);
        while( GameFadeManager.Instance.IsFade() )
        {
            yield return null;
        }

    }
}
 
