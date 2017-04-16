using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VR;
#if UNITY_PS4
using UnityEngine.PS4;
#endif  //

public class scr_SceneManager : MonoBehaviour {

    // シングルトン
    private static scr_SceneManager _instance;
    public static scr_SceneManager instance
    {
        get
        {
            if( _instance == null )
            {
                _instance = FindObjectOfType<scr_SceneManager>();
                DontDestroyOnLoad(_instance.gameObject);
            }
            return _instance;
        }
    }

    private Camera mainCamera = null;
    private Camera freeLookCamera = null;
    private Camera fpsCamera = null;

    private GameObject targetObj = null;

    private bool bSetupHMDDevice;
    public bool IsSetupHMDDevice
    {
        get {   return bSetupHMDDevice;     }
    }

    public enum UseCameraType
    {
        USE_CAMERA_MAIN = 0,
        USE_CAMERA_FREELOOK,
        USE_CAMERA_FPS,
        USE_CAMERA_NUM,

        USE_CAMERA_INVALID = 0xff,

        USE_CAMERA_MAX,
    };
    private UseCameraType cameraType;
    public UseCameraType CameraType
    {
        set { cameraType = value; }
        get { return cameraType; }
    }
   
    private int m_vrSettintStep;

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
		mainCamera = GameObject.Find("Main Camera").GetComponent<Camera>();
        if( !mainCamera )
        {
            Debug.Log("MainCamera Object Not Find\n");
        }
        freeLookCamera = GameObject.Find("FreeCamera").GetComponent<Camera>(); 
        if( !freeLookCamera )
        {
            Debug.Log("FreeLookCamera Object Not Find\n");
        }
        fpsCamera = GameObject.Find("FPSCamera").GetComponent<Camera>();
        if( !fpsCamera )
        {
            Debug.Log("FPS Camera Object Not Find\n");
        }
        
        bSetupHMDDevice = false;
        m_vrSettintStep = -1;
//        EnableMainCamera();
        EnableFPSCamera();

        if( VRSettings.enabled )
        {
            VRManager.instance.BeginVRSetup();
        }

        /*
        cameraType = UseCameraType.USE_CAMERA_INVALID;
        EnableMainCamera();
        targetObj = GameObject.Find("unitychan");
        if( targetObj == null )
        {
            Debug.Log("unitychan isnot Find\n");
        }
        targetObj.GetComponent<CharAction>().SetMoveMode(CharAction.ACTION_MODE.ACT_MODE_BLOWOFF);
        */
	}
	
	// Update is called once per frame
	void Update () {


        if( Input.GetButtonDown("R1") )
        {
            ProcSetupVRMode();
        }

		if( Input.GetKeyDown(KeyCode.C) )
        {
            cameraType++;
            if( cameraType >= UseCameraType.USE_CAMERA_NUM )
            {
                cameraType = UseCameraType.USE_CAMERA_MAIN;
            }

            if( cameraType == UseCameraType.USE_CAMERA_MAIN )
            {
                EnableMainCamera();
            }
            else if( cameraType == UseCameraType.USE_CAMERA_FREELOOK )
            {
                EnableFreeLookCamera();
            }
            else if( cameraType == UseCameraType.USE_CAMERA_FPS )
            {
                EnableFPSCamera();
            }
        }
   }

    void LateUpdate()
    {

        {
            string cameraStr = "CameraType : ";
            if (cameraType == UseCameraType.USE_CAMERA_MAIN)
            {
                cameraStr += "Main\n";
            }
            else if (cameraType == UseCameraType.USE_CAMERA_FREELOOK)
            {
                cameraStr += "FreeLook\n";
            }
            else if( cameraType == UseCameraType.USE_CAMERA_FPS)
            {
                cameraStr += "FPS\n";
            }
            else
            {
                cameraStr += "Invalid\n";
            }
            scr_GUIText.instance.AddText(cameraStr);
        }
    }

    public void SetCameraMode(UseCameraType type)
    {
        if( type == UseCameraType.USE_CAMERA_MAIN )
        {
            EnableMainCamera();
        }
        else if( type == UseCameraType.USE_CAMERA_FREELOOK )
        {
            EnableFreeLookCamera();
        }
        else if( type == UseCameraType.USE_CAMERA_FPS )
        {
            EnableFPSCamera();
        }      
    }

    public void EnableMainCamera()
    {
        if( !mainCamera || !freeLookCamera || !fpsCamera ) return;
        mainCamera.enabled = true;
        freeLookCamera.enabled = false;
        fpsCamera.enabled = false;
        cameraType = UseCameraType.USE_CAMERA_MAIN;
    }

    public void EnableFreeLookCamera()
    {
        if( !mainCamera || !freeLookCamera || !fpsCamera ) return;
        mainCamera.enabled = false;
        freeLookCamera.enabled = true;
        fpsCamera.enabled = false;
        cameraType = UseCameraType.USE_CAMERA_FREELOOK;
    }

    public void EnableFPSCamera()
    {
        if( !mainCamera || !freeLookCamera || !fpsCamera ) return;
        mainCamera.enabled = false;
        freeLookCamera.enabled = false;
        fpsCamera.enabled = true;
        cameraType = UseCameraType.USE_CAMERA_FPS;
    }

    public void ProcSetupVRMode()
    {
        Debug.Log("------------- Scene Manager Begin SetupHMD Device");
        VRManager.instance.SetupHMDDevice();
    }
}
