using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.VR;


public class GameFadeManager : Singleton<GameFadeManager> {

    private Canvas fadeCanvas;
    private Image fadeImage;
    private Camera camera;          // VRモード時用

    private int counter;
    private int FadeTime;
    private float fadeBlend;

    public enum FadeType
    {
        FADE_NONE = 0,
        FADE_IN,
        FADE_OUT,
        FADE_MAX,
    };
    private FadeType fadeType;

    public bool IsFade()
    {
        return fadeType != FadeType.FADE_NONE;
    }

    void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(this.gameObject);

        fadeCanvas = gameObject.GetComponentInChildren<Canvas>();

        if( fadeCanvas != null )
        {
            fadeImage = fadeCanvas.GetComponentInChildren<Image>();
        }
    }

	// Use this for initialization
	void Start () {
		
        counter = 0;
        FadeTime = 0;
        fadeType = FadeType.FADE_NONE;

        Color col = fadeImage.color;
        col.a = 0.0f;
        fadeImage.color = col;

        camera = null;
	}
	
	// Update is called once per frame
	void Update () {

        if( VRSettings.enabled )
        {
            fadeCanvas.worldCamera = Camera.main;
        }
		
        bool bFinish = false;
        switch( fadeType )
        {
        case FadeType.FADE_IN:
            bFinish = FadeIn();
            break;
        case FadeType.FADE_OUT:
            bFinish = FadeOut();
            break;
        }

        // フェードカウンター
        if( IsFade() )
        {
            counter++;
            if( counter > FadeTime )
            {
                counter = FadeTime;
                if( bFinish )
                {
                    Debug.Log("Fade" + fadeType + "Is Finish");
                    fadeType = FadeType.FADE_NONE;
                    counter = 0;
                }
            }
        }
	}

    public void StartFade(FadeType type, int fadeTime)
    {
        fadeType = type;
        FadeTime = fadeTime;
        counter = 0;

        // 現在のα値に合わせて開始時間を設定
        Color col = fadeImage.color;
        if( type == FadeType.FADE_IN )
        {
            counter = (int)((col.a - 1.0f) * FadeTime);
        }
        else if( type == FadeType.FADE_OUT )
        {
            counter = (int)(col.a * FadeTime);
        }        
    }

    private bool FadeIn()
    {
        Color fadeColor = fadeImage.color;

        fadeBlend = Mathf.Min((float)(counter) / (float)(FadeTime), 1.0f);
        float alpha = Mathf.Lerp(1.0f, 0.0f, fadeBlend);

        fadeColor.a = alpha;

        fadeImage.color = fadeColor;

        if( alpha <= 0.0f )
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private bool FadeOut()
    {
        Color fadeColor = fadeImage.color;

        fadeBlend = Mathf.Min((float)(counter) / (float)(FadeTime), 1.0f);
        float alpha = Mathf.Lerp(0.0f, 1.0f, fadeBlend);

        fadeColor.a = alpha;

        fadeImage.color = fadeColor;

        if( alpha >= 1.0f )
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    /**
     * VRモード時の設定
     */
    public void SetupVRMode(Camera vrCamera)
    {
        fadeCanvas.renderMode = RenderMode.ScreenSpaceCamera;
        fadeCanvas.worldCamera = vrCamera;
    }
    /**
     * VRモードの解除
     */
    public void ResetVRMode()
    {
        fadeCanvas.renderMode = RenderMode.ScreenSpaceOverlay;
        fadeCanvas.worldCamera = null;
    }
}
