using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager
{
    private Camera socialCamera = null;
    private Camera freeLookCamera = null;
    private Camera fpsCamera = null;

    public enum UseCameraType
    {
        USE_CAMERA_SOCIAL = 0,
        USE_CAMERA_FREELOOK,
        USE_CAMERA_FPS,
        USE_CAMERA_NUM,

        USE_CAMERA_INVALID = 0xff,

        USE_CAMERA_MAX,
    };
    private UseCameraType cameraType;
    public UseCameraType CameraType
    {
        set { cameraType = value; }
        get { return cameraType; }
    }


    public void Initialize()
    {
        if( GameObject.Find("SocialScreenCamera") )
        {
		    socialCamera = GameObject.Find("SocialScreenCamera").GetComponent<Camera>();
            if( !socialCamera )
            {
                Debug.Log("MainCamera Object Not Find\n");
            }
        }

        if( GameObject.Find("FreeCamera") )
        {
            freeLookCamera = GameObject.Find("FreeCamera").GetComponent<Camera>(); 
            if( !freeLookCamera )
            {
                Debug.Log("FreeLookCamera Object Not Find\n");
            }
        }

        if( GameObject.Find("FPSCamera") )
        {
            fpsCamera = GameObject.Find("FPSCamera").GetComponent<Camera>();
            if( !fpsCamera )
            {
                Debug.Log("FPS Camera Object Not Find\n");
            }
        }

        cameraType = UseCameraType.USE_CAMERA_INVALID;
        EnableFPSCamera();
    }

    public void ChangeCameraMode()
    {
        if( Input.GetKeyDown(KeyCode.C) )
        {
            cameraType++;
            if( cameraType >= UseCameraType.USE_CAMERA_NUM )
            {
                cameraType = UseCameraType.USE_CAMERA_SOCIAL;
            }

            if( cameraType == UseCameraType.USE_CAMERA_SOCIAL )
            {
                EnableSocialCamera();
            }
            else if( cameraType == UseCameraType.USE_CAMERA_FREELOOK )
            {
                EnableFreeLookCamera();
            }
            else if( cameraType == UseCameraType.USE_CAMERA_FPS )
            {
                EnableFPSCamera();
            }
        }
    }

     public void EnableSocialCamera()
    {
        if( socialCamera )
        {
            socialCamera.enabled = true;
            cameraType = UseCameraType.USE_CAMERA_SOCIAL;
        }
        if( freeLookCamera )
        {
            freeLookCamera.enabled = false;
        }
        if( fpsCamera )
        {
            fpsCamera.enabled = false;
        }
    }

    public void EnableFreeLookCamera()
    {
        if( socialCamera )
        {
            socialCamera.enabled = false;
        }

        if( freeLookCamera )
        {
            freeLookCamera.enabled = true;
            cameraType = UseCameraType.USE_CAMERA_FREELOOK;
        }
        if( fpsCamera )
        {
            fpsCamera.enabled = false;
        }
    }

    public void EnableFPSCamera()
    {
        if( socialCamera )
        {
            socialCamera.enabled = false;
        }
        if( freeLookCamera )
        {
            freeLookCamera.enabled = false;
        }
        if( fpsCamera )
        {
            fpsCamera.enabled = true;
            cameraType = UseCameraType.USE_CAMERA_FPS;
        }
    }

    public Camera GetCurrentCameraComponent()
    {
        switch( cameraType )
        {
        case UseCameraType.USE_CAMERA_FPS:
            return fpsCamera;
        case UseCameraType.USE_CAMERA_FREELOOK:
            return freeLookCamera;
        case UseCameraType.USE_CAMERA_SOCIAL:
            return socialCamera;
        }

        return null;
    }
}
