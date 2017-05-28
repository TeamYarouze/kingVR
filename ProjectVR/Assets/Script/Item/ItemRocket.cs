using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemRocket : ItemBase {

    private float RocketAngle = 60.0f;
    private float RocketPower = (200.0f * 1000.0f) / (60.0f*60.0f);
    private float RocketGravity = 1.0f;
 
	// Use this for initialization
	new public void Start () {
        base.Start();

        m_Angle = RocketAngle;
        m_Power = RocketPower;
        m_Gravity = RocketGravity;

        m_type = GameDefine.ITEM_TYPE.ITEM_TYPE_ROCKET;
        m_useType = (GameDefine.ItemUseType_UseAgain | GameDefine.ItemUseType_Reload | GameDefine.ItemUseType_BtnTrigger);
	}
	
	// Update is called once per frame
	new public void Update ()
    {
		if( !IsAttachedObject() )
        {
            return;
        }


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
            m_Angle = RocketAngle;
            m_Power = RocketPower;
            objScript.SetupBlowoffParam(m_Angle, m_Power, ForceMode.VelocityChange);
            m_state = EItemUseState.ITEM_STAT_USING;
            return true;
        }

        return false;
    }

    //---------------------------------------------------------------
    /*
        @brief      パラメータ設定
    */
    //---------------------------------------------------------------
    new public Vector3 SetParameter()
    {



        return Vector3.zero;
    }
}
