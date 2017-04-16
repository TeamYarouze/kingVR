using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemBase : MonoBehaviour {

    protected GameObject attachedObject;    // 自分がアタッチしているOBJ
    protected CharAction objScript;         // OBJのスクリプトコンポーネント

    protected float m_Angle;
    protected float m_Power;
    
    private int m_ID;
    public int ID
    {
        set { m_ID = value; }
        get { return m_ID; }
    }

	// Use this for initialization
	public void Start () {
		attachedObject = null;
        objScript = null;
	}
	
	// Update is called once per frame
	public void Update () {
		
	}

    //---------------------------------------------------------------
    /*
        @brief      発射
    */
    //---------------------------------------------------------------
    public bool OnFire()
    {
        return false;
    }

    //---------------------------------------------------------------
    /*
        @brief      オブジェクトへのアタッチ
    */
    //---------------------------------------------------------------
    public void AttachObject(GameObject target)
    {
        if( attachedObject || !target )
        {
            return;
        }
        attachedObject = target;
        objScript = attachedObject.GetComponent<CharAction>();

        transform.parent = target.transform;
    }

    //---------------------------------------------------------------
    /*
        @brief      オブジェクトから外す
    */
    //---------------------------------------------------------------
    public void RemoveObject()
    {
        attachedObject = null;
        objScript = null;

        transform.parent = null;
    }

    //---------------------------------------------------------------
    /*
        @brief      オブジェクトにアタッチされているのかどうか？
    */
    //---------------------------------------------------------------
    public bool IsAttachedObject()
    {
        if( attachedObject ) return true;
        return false;
    }

}
