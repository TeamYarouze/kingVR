using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemRocket : ItemBase {

    [SerializeField]
    private float RocketInitialAngle = 60.0f;
    [SerializeField]
    private float RocketInitialPower = 150.0f;
//    private float RocketInitialPower = (200.0f * 1000.0f) / (60.0f*60.0f);
    [SerializeField]
    private float RocketGravity = 10.0f;

    [SerializeField]
    private int ReloadFirstStep = 2;
    [SerializeField]
    private int ReloadSecondStep = 4;
    [SerializeField]
    private int ReloadeThiredStep = 6;

    [SerializeField]
    private float ReloadShortAngle = 45.0f;
    [SerializeField]
    private float ReloadShortPower = 50.0f;
    [SerializeField]
    private float ReloadMiddleAngle = 60.0f;
    [SerializeField]
    private float ReloadMiddlePower = 30.0f;
    [SerializeField]
    private float ReloadLongAngle = 75.0f;
    [SerializeField]
    private float ReloadLongPower = 10.0f;
 
	// Use this for initialization
	new public void Start () {
        base.Start();

        m_Angle = RocketInitialAngle;
        m_Power = RocketInitialPower;
        m_Gravity = RocketGravity;

        m_type = GameDefine.ITEM_TYPE.ITEM_TYPE_ROCKET;
        m_state = EItemUseState.ITEM_STAT_READY;
        m_useType = (GameDefine.ItemUseType_UseAgain | GameDefine.ItemUseType_Reload | GameDefine.ItemUseType_BtnTrigger);
	}
	
	// Update is called once per frame
	new public void Update ()
    {
		if( !IsAttachedObject() )
        {
            return;
        }

        UpdateReloadTime();

        OnFire();
	}

    //---------------------------------------------------------------
    /*
        @brief      更新
    */
    //---------------------------------------------------------------
    public void FixedUpdate()
    {
        if( !IsAttachedObject() )
        {
            return;
        }
    }

    //---------------------------------------------------------------
    /*
        @brief      発射処理
    */
    //---------------------------------------------------------------
    new public bool OnFire()
    {
        if( Input.GetButtonDown("Circle") )
        {
            Vector3 outVelocity;
            Vector3 vForward = Vector3.zero;

            float power = 0.0f;

            switch( m_state )
            {
            case EItemUseState.ITEM_STAT_READY:
                vForward = attachedObject.transform.forward;
                m_Angle = RocketInitialAngle;
                m_Power = RocketInitialPower;
                power = (m_Power * 1000.0f) / (60.0f * 60.0f);
                break;
            case EItemUseState.ITEM_STAT_USING:
            
                vForward = objScript.RigidBody.velocity;
                if( m_reloadTime >= ReloadeThiredStep )
                {
                    m_Angle = ReloadLongAngle;
                    m_Power = ReloadLongPower;
                }
                else if( m_reloadTime >= ReloadSecondStep )
                {
                    m_Angle = ReloadMiddleAngle;
                    m_Power = ReloadMiddlePower;
                }
                else if( m_reloadTime >= ReloadFirstStep )
                {
                    m_Angle = ReloadShortAngle;
                    m_Power = ReloadShortPower;
                }
                else
                {
                    return false;
                }

                power = objScript.RigidBody.velocity.magnitude + m_Power;

                break;
            }

            bool bLaunch = SetupRocketOrbit(out outVelocity, vForward, m_Angle, power);

            if( bLaunch )
            {
                objScript.SetupBlowoffParam(outVelocity, ForceMode.VelocityChange);
            }

            m_state = EItemUseState.ITEM_STAT_USING;

            base.OnFire();

            return true;
        }
        // ウンコー
        else if( Input.GetButtonDown("Triangle") )
        {
            m_state = EItemUseState.ITEM_STAT_READY;
        }

        return false;
    }

    //---------------------------------------------------------------
    /*
        @brief      ロケット軌道設定
    */
    //---------------------------------------------------------------
    public bool SetupRocketOrbit(out Vector3 vOut, Vector3 vBase, float angle, float power)
    {
        if( !IsAttachedObject() )
        {
            vOut = Vector3.zero;
            return false;
        }

        Vector3 moveVector = Vector3.zero;

        Quaternion rot = Quaternion.AngleAxis(-angle, attachedObject.transform.right);

        vBase = Vector3.Normalize(vBase);
        vBase = Vector3.Scale(vBase, new Vector3(1.0f, 0.0f, 0.0f));

        moveVector = rot * vBase;
        moveVector *= power;

        vOut = moveVector;
        return true;
    }


    //-----------------------------------------------------------------
    void OnGUI()
    {
        GUI.TextField(new Rect(720.0f, 400.0f, 300, 80), "ReloadTime: " + m_reloadTime + "\n" +
                                                   "SpeedVec: " + objScript.RigidBody.velocity.ToString() + "\n" +
                                                   "Speed : " + objScript.RigidBody.velocity.magnitude + "\n" +  
                                                   "State: " + m_state );
                                                    
    }
}
