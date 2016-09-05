using System;
using UnityEngine;

namespace RoverScience
{
	//SKIPS THROUGH MAIN MENU
	[KSPAddonFixed(KSPAddon.Startup.MainMenu, true, typeof(Debug_AutoLoadQuicksaveOnStartup))]
	public class Debug_AutoLoadQuicksaveOnStartup: MonoBehaviour
	{
		public static bool first = false;

		public void FixedUpdate()
		{
			if (first) {
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

