using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VR;

public class CharAction : MonoBehaviour {

    // 移動モード
    public enum ACTION_MODE
    {
        ACT_MODE_BLOWOFF,           // 吹っ飛ぶ
        ACT_MODE_FREEWALK,          // 歩く
        ACT_MOVE_TPS_BLOWOFF,       
        ACT_MODE_MAX
    };

    public float animSpeed = 1.5f;
    public float moveSpeed = 1.0f;

    private Rigidbody rb = null;
    private Animator anim = null;							// キャラにアタッチされるアニメーターへの参照
//	private AnimatorStateInfo currentBaseState;			// base layerで使われる、アニメーターの現在の状態の参照

    private Camera m_camera;
    private Camera m_fpsCamera;
    private GameObject m_VRCameraRoot;

    private scr_SceneManager m_SceneManager = null;

    private ACTION_MODE m_actMode;

    // 点火回数
    private int m_fireCount;
    // アイテム装備情報
    private const int EQUIP_ITEM_MAX = 8;           // アイテム装備最大数
    private GameObject[] m_arrayEquipItem = new GameObject[EQUIP_ITEM_MAX];    // 装備しているアイテム
    private int m_equipNum;                         // 装備しているアイテムの数

    private float m_gravity;                        // 重力
    private float m_baseGravity;
    private float m_power;                          // 前に進む力
//    private float m_angle;                          // 吹っ飛ぶ角度
    private Vector3 m_vectorToMove;                 // 移動ベクトル

    public float BASE_GRAVITY = 0.1f;

    public float BlowoffPower
    {
        get { return m_power; }
    }
    public Vector3 VectorToMove
    {
        get { return m_vectorToMove; }
    }

    private bool bBlowOff;

    private float yaw;
    public float Yaw { get { return yaw; } }


	// Use this for initialization
    //---------------------------------------------------------------
    /*
        @breif      初期化処理
    */
    //---------------------------------------------------------------
	void Start () {
        // RigidBodyの取得
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = false;
        // Animatorの取得
 //       anim = GetComponent<Animator>();

        m_camera = GameObject.Find("FreeCamera").GetComponent<Camera>();
        m_fpsCamera = GameObject.Find("FPSCamera").GetComponent<Camera>();
        m_VRCameraRoot = GameObject.Find("VRCameraRoot");
        yaw = 0.0f;

        // シーンマネージャーの保存
//        m_SceneManager = GameObject.Find("SceneManager").GetComponent<scr_SceneManager>();
//        SetMoveMode(ACTION_MODE.ACT_MODE_BLOWOFF);

        m_vectorToMove = Vector3.zero;

        bBlowOff = false;
        m_baseGravity = BASE_GRAVITY;
        // 試しにRocketをセット
        EquipItem(GameObject.Find("Rocket"));
	}
	
	// Update is called once per frame
    //---------------------------------------------------------------
    //---------------------------------------------------------------
	void Update () {

        if( !m_SceneManager )
        {
            m_SceneManager = GameObject.Find("SceneManager").GetComponent<scr_SceneManager>();
            SetMoveMode(ACTION_MODE.ACT_MODE_BLOWOFF);
        }

        m_baseGravity = BASE_GRAVITY;

        ChangeMoveMode();
	}

    //---------------------------------------------------------------
    /*
        @brief      更新
    */
    //---------------------------------------------------------------
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

        switch( m_actMode )
        {
        // 吹っ飛ぶ動作
        case ACTION_MODE.ACT_MODE_BLOWOFF:

            ExecBlowoff();

            break;
        // 自由歩行
        case ACTION_MODE.ACT_MODE_FREEWALK:
            // FPS風移動
            if( m_SceneManager.CameraType == scr_SceneManager.UseCameraType.USE_CAMERA_FPS )
            {
                ExecMoveFPS(lh, lv);
            }
            // TPS風移動
            else if( m_SceneManager.CameraType == scr_SceneManager.UseCameraType.USE_CAMERA_FREELOOK )
            {
                ExecMoveTPS(lh, lv);
            }
            break;
        case ACTION_MODE.ACT_MOVE_TPS_BLOWOFF:
            ExecBlowoff();
            break;
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
        if( VRSettings.enabled )
        {
            /*
            Quaternion targetRot = m_VRCameraRoot.GetComponent<scr_VRCameraRoot>().CameraRotation;
            Quaternion currentRot = transform.rotation;

            transform.rotation = Quaternion.RotateTowards(currentRot, targetRot, 1.0f);
            
            Vector3 cameraForward = m_VRCameraRoot.GetComponent<scr_VRCameraRoot>().Forward;
            transform.rotation = Quaternion.LookRotation(cameraForward);
            */
        }
        else
        {
            float yaw = m_fpsCamera.GetComponent<scr_CameraFPS>().Angle_Yaw;
            transform.rotation = Quaternion.AngleAxis(yaw, Vector3.up);
        }
    }

    void LateUpdate()
    {
        // FPSカメラの時のキャラの向き制御
        if( m_SceneManager.CameraType == scr_SceneManager.UseCameraType.USE_CAMERA_FPS )
        {
            RotateChar();
        }

        {
            string infoStr = "";
            infoStr += "BlowoffVector: " + m_vectorToMove.ToString() + "\n";
            infoStr += "Action Mode: " + m_actMode + "\n";
            scr_GUIText.instance.AddText(infoStr);
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        bBlowOff = false;
        m_vectorToMove = Vector3.zero;
        m_gravity = 0.0f;
    }

    //---------------------------------------------------------------
    /*
        @brief      吹っ飛ぶ
    */
    //---------------------------------------------------------------
    void ExecBlowoff()
    {
        
        Vector3 oldPos = transform.position;
        Vector3 position = oldPos + m_vectorToMove;

        if( position.y < 0.0f )
        {
            position.y = 0.0f;
            m_vectorToMove = Vector3.zero;
        }

        transform.position = position;
        // 重力を適用
        if( bBlowOff )
        {
            m_vectorToMove.y -= (m_baseGravity + m_gravity);
        }
    }

    //---------------------------------------------------------------
    /*
        @brief      ぶっ飛ぶ設定
    */
    //---------------------------------------------------------------
    public void SetupBlowoffParame(float angle, float power, float gravity)
    {
        Vector3 vForward = Vector3.Normalize(transform.forward);

        Quaternion rot = Quaternion.AngleAxis(-angle, transform.right);

        m_vectorToMove = rot * vForward;
        m_vectorToMove *= power;

        m_power = power;
//        m_angle = angle;
        m_gravity = gravity;

        bBlowOff = true;
    }

    //---------------------------------------------------------------
    /*
        @brief      重力の設定
    */
    //---------------------------------------------------------------
    public void SetGravity(float g)
    {
        m_gravity = g;
    }

    //---------------------------------------------------------------
    /*
        @brief      装備アイテムのリセット
    */
    //---------------------------------------------------------------
    public void ResetEquipItem()
    {
        for(int i = 0; i < m_arrayEquipItem.Length; i++)
        {
            m_arrayEquipItem[i] = null;
        }
        m_equipNum = 0;
    }

    //---------------------------------------------------------------
    /*
        @brief      アイテムの装備
    */
    //---------------------------------------------------------------
    public bool EquipItem(GameObject item)
    {
        if( m_equipNum >= EQUIP_ITEM_MAX )
        {
            return false;
        }
        if( !item )
        {
            return false;
        }

        item.GetComponent<ItemBase>().AttachObject(gameObject);

        m_arrayEquipItem[m_equipNum] = item;
        m_equipNum++;
        if( m_equipNum > EQUIP_ITEM_MAX ) m_equipNum = EQUIP_ITEM_MAX;
        return true;
    }

    //---------------------------------------------------------------
    /*
        @brief      装備アイテムを外す
    */
    //---------------------------------------------------------------
    public void RemoveItem(int idx)
    {
        GameObject item = m_arrayEquipItem[idx];
        if( item )
        {
            item.GetComponent<ItemBase>().RemoveObject();
        }

        m_arrayEquipItem[idx] = null;
        m_equipNum--;
        if( m_equipNum < 0 ) m_equipNum = 0;
    }

    //---------------------------------------------------------------
    /*
        @brief      装備アイテムを詰める
    */
    //---------------------------------------------------------------
    private void CloseItem()
    {
        
    }

    //---------------------------------------------------------------
    /*
        @brief      移動モードの設定
    */
    //---------------------------------------------------------------
    public void SetMoveMode(ACTION_MODE actMode)
    {
        if( !m_SceneManager ) return;

        m_actMode = actMode;
        /*
        if( actMode == ACTION_MODE.ACT_MODE_BLOWOFF )
        {
            m_SceneManager.SetCameraMode(scr_SceneManager.UseCameraType.USE_CAMERA_FPS);
        }
        */
    }

    //---------------------------------------------------------------
    /*
        @brief      移動モードの変更
    */
    //---------------------------------------------------------------
    void ChangeMoveMode()
    {
        if( Input.GetKeyDown(KeyCode.M) )
        {
            m_actMode++;
            if( m_actMode >= ACTION_MODE.ACT_MODE_MAX )
            {
                m_actMode = ACTION_MODE.ACT_MODE_BLOWOFF;
            }

            if( m_actMode == ACTION_MODE.ACT_MODE_BLOWOFF )
            {
                m_SceneManager.SetCameraMode(scr_SceneManager.UseCameraType.USE_CAMERA_FPS);
            }
            else if( m_actMode == ACTION_MODE.ACT_MODE_FREEWALK )
            {
                m_SceneManager.SetCameraMode(scr_SceneManager.UseCameraType.USE_CAMERA_FREELOOK);
            }
            else if( m_actMode == ACTION_MODE.ACT_MOVE_TPS_BLOWOFF )
            {
                m_SceneManager.SetCameraMode(scr_SceneManager.UseCameraType.USE_CAMERA_FREELOOK);
            }
        }
    }
}
