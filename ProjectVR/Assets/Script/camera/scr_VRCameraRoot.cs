﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VR;
#if UNITY_PS4
using UnityEngine.PS4;
using UnityEngine.PS4.VR;
#endif  

public class scr_VRCameraRoot : MonoBehaviour {

    public GameObject king = null;
    private float cameraHeight = 2.3f;
    private float cameraForward = -0.35f;

    private Quaternion cameraRotation;
    public Quaternion CameraRotation
    {
        get { return cameraRotation; }
    }

    private Vector3 forward;
    public Vector3 Forward
    {
        get { return forward; }
    }

    private Vector3 outhmdPositionRaw;
    private Vector3 outhmdPositionUnity;

    private Vector3 offsetPos;

	// Use this for initialization
	void Start () {
        if( !king )
        {
    		king = GameObject.Find("kings");

            InputTracking.Recenter();
            offsetPos = Vector3.zero;
            // 初期位置を固定したい
            MoveExe();
        }

	}
	
	// Update is called once per frame
	void Update ()
    {
        /*
        if( VRSettings.enabled )
        {
            Quaternion trackingRot = InputTracking.GetLocalRotation(VRNode.CenterEye);
            cameraRotation = trackingRot;

            Vector3 worldPos = transform.position + InputTracking.GetLocalPosition(VRNode.CenterEye);
            Vector3 transformedPos = trackingRot * worldPos;
            
            forward = Vector3.Normalize(transformedPos);
        }
        */

        if( VRSettings.enabled )
        {
            cameraHeight = 2.3f;
            cameraForward = -0.8f;

            UpdateHmdPosition();
        }
        else
        {
            cameraForward = -0.35f;
            cameraHeight = 2.3f;
        }

	}

    void LateUpdate()
    {
        MoveExe();

        Vector3 trackingPos = InputTracking.GetLocalPosition(VRNode.CenterEye);
        {
            string strInfo = "";
            strInfo += "Tracking Pos:";
            strInfo += trackingPos.ToString() + "\n";

            Vector3 camPos = Camera.main.transform.position;
            strInfo += "Camera Pos:";
            strInfo += camPos.ToString() + Camera.main.name + "\n";

            Vector3 camRootPos = transform.position;
            strInfo += "CameraRoot Pos:";
            strInfo += camRootPos.ToString() + "\n";

            strInfo += "Raw: " + outhmdPositionRaw.ToString() + "\n";
            strInfo += "Unity: " + outhmdPositionUnity.ToString() + "\n";

            scr_GUIText.instance.AddText(strInfo);
        }
    }

    /**
     *  @brief      アタッチ済みの王様オブジェクトに付随して動く
     */
    void MoveExe()
    {
        if( !king ) return;

//        float horizontal = Input.GetAxis("Horizontal");      
//        offsetPos.x += (2.0f * (horizontal * GameDefine.FPSDeltaScale()));

        Vector3 kingPos = king.transform.position;
        Vector3 objPos = transform.position;
        objPos = kingPos;
        objPos.x += offsetPos.x;
        objPos.y += cameraHeight;

        transform.position = objPos;

        // HMDの位置を固定する
//        Vector3 trackingPos = InputTracking.GetLocalPosition(VRNode.CenterEye);
//        transform.position = objPos - trackingPos;

    }

#if UNITY_PS4
    /**
     *  @brief      PSVR HMDの位置
     */
    private void UpdateHmdPosition()
    {
        int hmdHandle = PlayStationVR.GetHmdHandle();
        
        Tracker.GetTrackedDevicePosition(hmdHandle, PlayStationVRSpace.Raw, out outhmdPositionRaw);
                
        Tracker.GetTrackedDevicePosition(hmdHandle, PlayStationVRSpace.Unity, out outhmdPositionUnity);

    }
#endif  //

}
