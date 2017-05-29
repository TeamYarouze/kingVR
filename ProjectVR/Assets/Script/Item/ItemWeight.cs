using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemWeight : ItemBase {

	// Use this for initialization
	void Start () {
		base.Start();
	}
	
	// Update is called once per frame
	void Update () {

        if( !IsAttachedObject() )
        {
            return;
        }

        UpdateReloadTime();
		
        OnFire();
	}

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
        return false;
    }

}
