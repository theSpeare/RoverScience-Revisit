using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace RoverScience
{

	static class GUIStyles
	{
		static public GUIStyle consoleArea = new GUIStyle(HighLogic.Skin.textArea);

	}


	public partial class RoverScienceGUI
	{

		public System.Random rand = new System.Random();

		public RandomConsolePrintOuts randomConsolePrintOuts = new RandomConsolePrintOuts();
		public Vector2 scrollPosition = new Vector2 ();

		private List<string> consolePrintOut = new List<string>();

        private RoverScience roverScience
		{
			get{
				return RoverScience.Instance;
			}

		}
       

		private Vessel vessel
		{
			get{
				return FlightGlobals.ActiveVessel;
			}
		}

		private Rover rover
		{
			get{
				return RoverScience.Instance.rover;
			}
		}
        

        public RoverScienceGUI()
		{
            Debug.Log ("RoverScienceGUI started");
			consoleGUI.rect.width = 250;
			debugGUI.rect.width = 230;
            upgradeGUI.rect.width = 500;

            // center consoleGUI
            consoleGUI.rect.x = (Screen.width / 2) - (consoleGUI.rect.width / 2);
            consoleGUI.rect.y = (Screen.height / 2) - (250);

            upgradeGUI.rect.x = consoleGUI.rect.x + consoleGUI.rect.width + 50;
            upgradeGUI.rect.y = consoleGUI.rect.y;
        }

        public void drawGUI()
		{
            if (consoleGUI.isOpen)
            {
				consoleGUI.rect = GUILayout.Window(25639814, consoleGUI.rect, drawRoverConsoleGUI, "Rover Terminal - " + roverScience.RSVersion);

                if (upgradeGUI.isOpen)
                {
                    upgradeGUI.rect = GUILayout.Window(2389233, upgradeGUI.rect, drawUpgradeGUI, "Upgrade Menu");
                }

            }
            
			if (debugGUI.isOpen) {
			    debugGUI.rect = GUILayout.Window (9358921, debugGUI.rect, drawDebugGUI, "Debug");
			}
		}

        public void setWindowPos (GUIClass guiWindow, float x, float y)
        {
            guiWindow.rect.x = x;
            guiWindow.rect.y = y;
        }

		public void addToConsole (string line)
		{
			if (consolePrintOut.Count >= 50) {
				consolePrintOut.Clear ();
			}
			consolePrintOut.Add ("> " + line);
			scrollPosition.y = 10000;
		}

		public void addRandomConsoleJunk()
		{
			addToConsole (randomConsolePrintOuts.getRandomPrint());
		}

		public void clearConsole()
		{
			consolePrintOut.Clear ();
		}
			


	}
}

