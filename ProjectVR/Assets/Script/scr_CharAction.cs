using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scr_CharAction : MonoBehaviour {

    public float animSpeed = 1.5f;
    public float moveSpeed = 1.0f;

    private Rigidbody rb = null;
    private Animator anim = null;							// キャラにアタッチされるアニメーターへの参照
//	private AnimatorStateInfo currentBaseState;			// base layerで使われる、アニメーターの現在の状態の参照

    private Camera m_camera;
    private Camera m_fpsCamera;

    private scr_SceneManager m_SceneManager;

    private float yaw;
    public float Yaw { get { return yaw; } }

	// Use this for initialization
	void Start () {
        // RigidBodyの取得
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = false;
        // Animatorの取得
        anim = GetComponent<Animator>();

        m_camera = GameObject.Find("FreeCamera").GetComponent<Camera>();
        m_fpsCamera = GameObject.Find("FPSCamera").GetComponent<Camera>();
        yaw = 0.0f;

        // シーンマネージャーの保存
        m_SceneManager = GameObject.Find("SceneManager").GetComponent<scr_SceneManager>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void FixedUpdate()
    {
        float lh = Input.GetAxis("Horizontal");
        float lv = Input.GetAxis("Vertical");

        if( anim )
        {
            anim.SetFloat("Speed", lv);
            anim.SetFloat("Direction", lh);
            anim.speed = animSpeed;
//            currentBaseState = anim.GetCurrentAnimatorStateInfo(0);
        }

        if( m_SceneManager.CameraType == scr_SceneManager.UseCameraType.USE_CAMERA_FPS )
        {
            ExecMoveFPS(lh, lv);
        }
        else if( m_SceneManager.CameraType == scr_SceneManager.UseCameraType.USE_CAMERA_FREELOOK )
        {
            ExecMoveTPS(lh, lv);
        }
    }

    // キャラの移動処理 TPS
    void ExecMoveTPS(float h, float v)
    {
        Vector3 cameraForward = Vector3.Scale(m_camera.transform.forward, new Vector3(1.0f, 0.0f, 1.0f));
        Vector3 direction = cameraForward * v + m_camera.transform.right * h;

        transform.position += direction;

        if( direction != Vector3.zero )
        {
            transform.rotation = Quaternion.LookRotation(direction);
        }
    }

    // キャラの移動処理 FPS
    void ExecMoveFPS(float h, float v)
    {
        Vector3 cameraForward = Vector3.Scale(m_fpsCamera.transform.forward, new Vector3(1.0f, 0.0f, 1.0f));
        Vector3 direction = cameraForward * v + m_fpsCamera.transform.right * h;

        transform.position += direction;   
    }

    // キャラの向きを制御(FPS Only)
    void RotateChar()
    {
        float yaw = m_fpsCamera.GetComponent<scr_CameraFPS>().Angle_Yaw;
        transform.rotation = Quaternion.AngleAxis(yaw, Vector3.up);
    }

    void LateUpdate()
    {
        // FPSカメラの時のキャラの向き制御
        if( m_SceneManager.CameraType == scr_SceneManager.UseCameraType.USE_CAMERA_FPS )
        {
            RotateChar();
        }
    }

    //---------------------------------------------------------------
    /*
        @brief      カメラタイプを変更する
    */
    //---------------------------------------------------------------
    void ChangeCamera()
    {
    }
}
