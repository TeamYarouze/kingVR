using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemRocket : ItemBase {

    public float RocketAngle = 70.0f;
    public float RocketPower = 0.4f;
 
	// Use this for initialization
	new public void Start () {
        m_Angle = RocketAngle;
        m_Power = RocketPower;
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

            objScript.SetupBlowoffParame(m_Angle, m_Power, 0.0f);
            return true;
        }

        return false;
    }
}
