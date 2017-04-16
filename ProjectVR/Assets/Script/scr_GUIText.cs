using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scr_GUIText : MonoBehaviour {

    private GameObject textObj = null;
    private bool bDraw = false;

    // シングルトン
    private static scr_GUIText _instance;
    public static scr_GUIText instance
    {
        get
        {
            if( _instance == null )
            {
                _instance = FindObjectOfType<scr_GUIText>();
                DontDestroyOnLoad(_instance.gameObject);
            }
            return _instance;
        }
    }

    public void Awake()
    {
        if( _instance == null )
        {
            _instance = this;
            DontDestroyOnLoad(_instance);
        }
        else if( this != _instance )
        {
            Destroy(gameObject);
        }
    }

	// Use this for initialization
	void Start () {
		textObj = GameObject.Find("Text");
        bDraw = false;
 	}
	
	// Update is called once per frame
	void Update () {

        if( !textObj ) return;

        if( Input.GetButtonDown("L1") )
        {
            bDraw = !bDraw;
        }

        textObj.GetComponent<GUIText>().text = "";
	}

    void LateUpdate()
    {
    }
    
    public void SetText(string str)
    {
        if( !textObj ) return;
        if( !bDraw ) return;

        textObj.GetComponent<GUIText>().text = str;
    }

    public void AddText(string str)
    {
        if( !textObj ) return;
        if( !bDraw ) return;

        textObj.GetComponent<GUIText>().text += str;
    }

    public void SetDrawFlag(bool flg)
    {
        bDraw = flg;
    }
}