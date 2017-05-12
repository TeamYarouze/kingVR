using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameDefine
{
    //----------------------------------------------------------------
    // FPS
    //----------------------------------------------------------------
    private static int baseFPS;
    public static int BaseFPS
    {
        get { return baseFPS; }
    }
    public static float FPSDeltaScale()
    {
        return baseFPS * Time.deltaTime;
    }

    static GameDefine()
    {
        baseFPS = 60;
    }

}
