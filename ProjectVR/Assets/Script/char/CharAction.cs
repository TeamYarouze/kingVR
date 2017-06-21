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
    public Rigidbody RigidBody
    {
        get { return rb; }
    }

    private Animator anim = null;							// キャラにアタッチされるアニメーターへの参照
//	private AnimatorStateInfo currentBaseState;			// base layerで使われる、アニメーターの現在の状態の参照

    private UpdateStage updater = null;
    private Camera m_camera = null;
    private GameObject m_VRCameraRoot;

    private ACTION_MODE m_actMode;

    // 点火回数
    private int m_fireCount;
    // アイテム装備情報
    private const int EQUIP_ITEM_MAX = 8;           // アイテム装備最大数
    private GameObject[] m_arrayEquipItem = new GameObject[EQUIP_ITEM_MAX];    // 装備しているアイテム
    private int m_equipNum;                         // 装備しているアイテムの数

    private float m_gravity;                        // 重力
    private float m_power;                          // 前に進む力
    private float m_angle;                          // 吹っ飛ぶ角度
    private Vector3 m_vectorToMove;                 // 移動ベクトル
    private Vector3 m_vectorToMoveInverse;          // 逆移動ベクトル

    private Vector3 m_StartPos;
    private Quaternion m_StartRot;

    private float BASE_GRAVITY = 5.0f;

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

    // ロケット角度
    private float rocketRot;

    private GameObject AfterBurner = null;

    private GameObject SceneUpdater = null;

    //---------------------------------------------------------------
    private float elapsedTime;
    private Vector3 startPos;
    private Vector3 distant;

	// Use this for initialization
    //---------------------------------------------------------------
    /*
        @breif      初期化処理
    */
    //---------------------------------------------------------------
	void Start () {

        updater = GameSceneManager.Instance.GetSceneUpdater() as UpdateStage;




        // RigidBodyの取得
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
        rb.useGravity = true;
        rb.isKinematic = false;

        m_camera = null;
  
        if( updater )
        {             
            m_camera = updater.CameraMngr.GetCurrentCameraComponent();
        }

        m_VRCameraRoot = GameObject.Find("VRCameraRoot");
        yaw = 0.0f;

        m_gravity = BASE_GRAVITY;

        m_vectorToMove = Vector3.zero;

        m_StartPos = transform.position;
        m_StartRot = transform.rotation;

        bBlowOff = false;

        GameObject rocket = (GameObject)Instantiate(Resources.Load(GameResourcePath.GetRocketPath()), transform.position, transform.rotation);
        EquipItem(rocket);

        GameObject weight = (GameObject)Instantiate(Resources.Load(GameResourcePath.GetWeightPath()), transform.position, transform.rotation);
        EquipItem(weight);

        elapsedTime = 0;

        rocketRot = 180.0f;
        
        SetMoveMode(ACTION_MODE.ACT_MODE_BLOWOFF);
	}
	
	// Update is called once per frame
    //---------------------------------------------------------------
    //---------------------------------------------------------------
	void Update ()
    {
        // ロケット回転
        RotateRocket();

        ChangeMoveMode();

        // リセット処理
        ResetPosition();
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
            if( updater.CameraMngr.CameraType == CameraManager.UseCameraType.USE_CAMERA_FPS )
            {
                ExecMoveFPS(lh, lv);
            }
            else if( updater.CameraMngr.CameraType == CameraManager.UseCameraType.USE_CAMERA_FREELOOK )
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
        Vector3 cameraForward = Vector3.Scale(m_camera.transform.forward, new Vector3(1.0f, 0.0f, 1.0f));
        Vector3 direction = cameraForward * v + m_camera.transform.right * h;

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
            float yaw = m_camera.GetComponent<scr_CameraFPS>().Angle_Yaw;
            transform.rotation = Quaternion.AngleAxis(yaw, Vector3.up);
        }
    }

    void LateUpdate()
    {
        /*
        // FPSカメラの時のキャラの向き制御
        if( scr_SceneManager.instance.CameraType == scr_SceneManager.UseCameraType.USE_CAMERA_FPS )
        {
            RotateChar();
        }
        */

        CheckWorldClip();

        {
            string infoStr = "----------------- King Param\n";
            infoStr += "Pos:" + transform.position.ToString() + "\n";
            infoStr += "Gravity:" + m_gravity + "\n";
            infoStr += "BlowoffVector: " + m_vectorToMove.ToString() + "Speed:" + m_vectorToMove.magnitude + "\n";
            infoStr += "Velocity:" + rb.velocity.ToString() + "Speed:" + rb.velocity.magnitude + "\n";
            infoStr += "Action Mode: " + m_actMode + "BlowOff:" + bBlowOff + "\n";
            infoStr += "Dist / Sec : " + distant.magnitude + "\n";
            scr_GUIText.instance.AddText(infoStr);
        }
    }

    //---------------------------------------------------------------
    /*
        @brief      衝突検知の瞬間
    */
    //---------------------------------------------------------------
    void OnCollisionEnter(Collision collision)
    {
        /*
        bBlowOff = false;
        m_vectorToMove = Vector3.zero;
        rb.AddForce(m_vectorToMove, ForceMode.VelocityChange);
        */

        // ゴール処理
        if( collision.gameObject.name == "Goal" )
        {
            Debug.Log("Goooooaaaaallllll !!!!!!!!!!");
            rb.useGravity = false;
            rb.isKinematic = true;
            rb.velocity = m_vectorToMove;
            rb.angularVelocity = m_vectorToMove;


            PlayAfterBurner(false);

            if( updater )
            {
                if( updater.IsPlayDustStorm() )
                {
                    updater.PlayDustStorm(false);
                }
            }

        }
    }

    //---------------------------------------------------------------
    /*
        @brief      吹っ飛ぶ
    */
    //---------------------------------------------------------------
    void ExecBlowoff()
    {
        // 加速度の適用

        // 重力の適用
//        ApplyGravity();

        elapsedTime += Time.fixedDeltaTime;
        if( elapsedTime >= 1.0f )
        {
            distant = transform.position - startPos;

            elapsedTime = 0;
            startPos = transform.position;
        }
    }
    
    //---------------------------------------------------------------
    /*
        @brief      ぶっ飛ぶ設定
    */
    //---------------------------------------------------------------
    public void SetupBlowoffParam(Vector3 velocity, ForceMode mode)
    {
        m_vectorToMove = velocity;

        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;

        rb.AddForce(m_vectorToMove, mode);

        elapsedTime = 0;
        startPos = transform.position;
        distant = transform.position - startPos;

        // パーティクルテスト
        if( !IsAfterBurner() )
        {
            PlayAfterBurner(true);
        }

        if( updater )
        {
            if( !updater.IsPlayDustStorm() )
            {
                updater.PlayDustStorm(true);
            }
        }
    
        bBlowOff = true;
    }

    //---------------------------------------------------------------
    /*
        @brief      重力の適用
    */
    //---------------------------------------------------------------
    private void ApplyGravity()
    {
        rb.AddForce(0.0f, -m_gravity, 0.0f, ForceMode.Acceleration);
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

        m_gravity += item.GetComponent<ItemBase>().Gravity;

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
            m_gravity -= item.GetComponent<ItemBase>().Gravity;
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

        m_actMode = actMode;
    }

    //---------------------------------------------------------------
    /*
        @brief      移動モードの変更
    */
    //---------------------------------------------------------------
    void ChangeMoveMode()
    {
        /*
        if( Input.GetKeyDown(KeyCode.M) )
        {
            m_actMode++;
            if( m_actMode >= ACTION_MODE.ACT_MODE_MAX )
            {
                m_actMode = ACTION_MODE.ACT_MODE_BLOWOFF;
            }
            if( m_actMode == ACTION_MODE.ACT_MODE_BLOWOFF )
            {
                scr_SceneManager.instance.SetCameraMode(scr_SceneManager.UseCameraType.USE_CAMERA_FPS);
            }
            else if( m_actMode == ACTION_MODE.ACT_MODE_FREEWALK )
            {
                scr_SceneManager.instance.SetCameraMode(scr_SceneManager.UseCameraType.USE_CAMERA_FREELOOK);
            }
            else if( m_actMode == ACTION_MODE.ACT_MOVE_TPS_BLOWOFF )
            {
                scr_SceneManager.instance.SetCameraMode(scr_SceneManager.UseCameraType.USE_CAMERA_FREELOOK);
            }
            
        }
        */
    }

    //---------------------------------------------------------------
    /*
        @brief      リセット処理
    */
    //---------------------------------------------------------------
    void ResetPosition()
    {
        if( Input.GetButtonDown("L3") )
        {
            m_vectorToMove = Vector3.zero;
            rb.AddForce(m_vectorToMove, ForceMode.VelocityChange);

            rb.velocity = m_vectorToMove;
            rb.angularVelocity = m_vectorToMove;

            transform.position = m_StartPos;
            transform.rotation = m_StartRot;

            rb.useGravity = true;
            rb.isKinematic = false;

            for(int id = 0; id < m_equipNum; id++)
            {
                m_arrayEquipItem[id].GetComponent<ItemBase>().ResetItem();
            }

            if( IsAfterBurner() )
            {
                PlayAfterBurner(false);
            }

            if( updater )
            {
                if( updater.IsPlayDustStorm() )
                {
                    updater.PlayDustStorm(false);
                }
            }

        }
    }

    void CheckWorldClip()
    {
        Vector3 pos = transform.position;

        if( pos.y < 0.0f )
        {
            pos.y = 0.0f;
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
            rb.AddForce(Vector3.zero, ForceMode.VelocityChange);
        }

        transform.position = pos;

    }

    void RotateRocket()
    {
        float lh = Input.GetAxis("Horizontal");

        if(lh > 0.25f || lh < -0.25f)
        {

            rocketRot += (lh * 1.0f) * GameDefine.FPSDeltaScale();

            Quaternion rot = Quaternion.AngleAxis(rocketRot, transform.up);

            transform.FindChild("chairs").FindChild("chairs:root").FindChild("chairs:barrel").rotation = rot;
        }       
    }

    void PlayAfterBurner(bool bFire)
    {
        if( !AfterBurner )
        {
            AfterBurner = transform.FindChild("chairs/chairs:root/chairs:barrel/Afterburner").gameObject;
        }

        if( bFire )
        {
            AfterBurner.GetComponent<ParticleSystemBase>().Play();
        }
        else
        {
            AfterBurner.GetComponent<ParticleSystemBase>().Stop();
        }
    }

    bool IsAfterBurner()
    {
        if( !AfterBurner )
        {
            return false;
        }

        return AfterBurner.GetComponent<ParticleSystemBase>().IsPlay();
    }
}
