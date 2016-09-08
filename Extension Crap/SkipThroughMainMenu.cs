using System;
using UnityEngine;

namespace RoverScience
{
	//SKIPS THROUGH MAIN MENU
	[KSPAddon(KSPAddon.Startup.MainMenu, true)]
	public class SkipThroughMainMenu: MonoBehaviour
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

