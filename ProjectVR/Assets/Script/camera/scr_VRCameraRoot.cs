using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VR;

public class scr_VRCameraRoot : MonoBehaviour {

    public GameObject king = null;
    private float cameraHeight = 1.5f;
    private float cameraForward = 2.0f;

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

	// Use this for initialization
	void Start () {
        if( !king )
        {
    		king = GameObject.Find("kings");
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
	}

    void LateUpdate()
    {
        MoveExe();

        Vector3 trackingPos = InputTracking.GetLocalPosition(VRNode.CenterEye);
        {
            string strInfo = "Tracking Pos:";
            strInfo += trackingPos.ToString() + "\n";

            Vector3 camPos = Camera.main.transform.position;
            strInfo += "Camera Pos:";
            strInfo += camPos.ToString() + Camera.main.name + "\n";

            scr_GUIText.instance.AddText(strInfo);
        }
    }

    /**
     *  @brief      アタッチ済みの王様オブジェクトに付随して動く
     */
    void MoveExe()
    {
        if( !king ) return;

        Vector3 kingPos = king.transform.position;
        Vector3 objPos = transform.position;
        objPos = kingPos;
        objPos.y += cameraHeight;
        objPos.x += cameraForward;
        transform.position = objPos;        

    }

}
