using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemRocket : ItemBase {

    private float RocketAngle = 60.0f;
//    private float RocketPower = 150.0f;
    private float RocketPower = (150.0f * 1000.0f) / (60.0f*60.0f);
    private float RocketGravity = 20.0f;
 
	// Use this for initialization
	new public void Start () {
        m_Angle = RocketAngle;
        m_Power = RocketPower;
        m_Gravity = RocketGravity;
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
}
