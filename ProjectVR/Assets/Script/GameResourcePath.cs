using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameResourcePath {

    private static string BootSequence = "Scene/bootSequecne";
    private static string Stage000 = "Scene/Stage000/Stage000";

    public static string GetSceneName(GameModeData.GAMEMODE mode)
    {
        switch( mode )
        {
        case GameModeData.GAMEMODE.GAME_MODE_BOOT:
            return BootSequence;
        case GameModeData.GAMEMODE.GAME_MODE_STAGE:
            return Stage000;
        }

        return BootSequence;
    }

}
