using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class GameFadeManager : Singleton<GameFadeManager> {

    private Image fadeImage;

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

        Canvas canvas = gameObject.GetComponentInChildren<Canvas>();

        if( canvas != null )
        {
            fadeImage = canvas.GetComponentInChildren<Image>();
        }
    }

	// Use this for initialization
	void Start () {
		
        counter = 0;
        FadeTime = 0;
        fadeType = FadeType.FADE_NONE;
	}
	
	// Update is called once per frame
	void Update () {
		
        switch( fadeType )
        {
        case FadeType.FADE_IN:
            FadeIn();
            break;
        case FadeType.FADE_OUT:
            FadeOut();
            break;
        }

        // フェードカウンター
        if( IsFade() )
        {
            counter++;
            if( counter > FadeTime )
            {
                Debug.Log("Fade" + fadeType + "Is Finish");
                fadeType = FadeType.FADE_NONE;
                counter = 0;
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

    private void FadeIn()
    {
        Color fadeColor = fadeImage.color;

        fadeBlend = Mathf.Min((float)(counter) / (float)(FadeTime), 1.0f);
        float alpha = Mathf.Lerp(1.0f, 0.0f, fadeBlend);

        fadeColor.a = alpha;

        fadeImage.color = fadeColor;
    }

    private void FadeOut()
    {
        Color fadeColor = fadeImage.color;

        fadeBlend = Mathf.Min((float)(counter) / (float)(FadeTime), 1.0f);
        float alpha = Mathf.Lerp(0.0f, 1.0f, fadeBlend);

        fadeColor.a = alpha;

        fadeImage.color = fadeColor;
    }
}
