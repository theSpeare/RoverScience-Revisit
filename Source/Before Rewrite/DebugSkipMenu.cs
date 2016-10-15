using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace RoverScience
{
    [KSPAddon(KSPAddon.Startup.MainMenu, false)]
    public class DebugSkipMenu : MonoBehaviour
    {
        public static bool first = false;

        public void FixedUpdate()
        {
            if (first)
            {
                first = false;
                HighLogic.SaveFolder = "default";
                var game = GamePersistence.LoadGame("persistent", HighLogic.SaveFolder, true, false);
                if (game != null && game.flightState != null && game.compatible)
                {
                    HighLogic.LoadScene(GameScenes.TRACKSTATION);
                }

            }
        }

    }
}
