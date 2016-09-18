using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace RoverScience
{
    [KSPAddon(KSPAddon.Startup.Flight, true)]
    public class RoverScienceDB : MonoBehaviour
    {
        public static RoverScienceDB Instance;
        public RoverScienceDB()
        {
            Instance = this;
        }

        private RoverScience roverScience
        {
            get
            {
                return RoverScience.Instance;
            }
        }

        private RoverScienceGUI GUI
        {
            get
            {
                return RoverScience.Instance.roverScienceGUI;
            }
        }

        public int levelMaxDistance = 1;
        public int levelPredictionAccuracy = 1;
        public int levelAnalyzedDecay = 2;

        public List<string> console_x_y_show = new List<string>();
        public List<string> anomaliesAnalyzed = new List<string>();

        public void updateRoverScience()
        {
            // load all values above to roverscience stuff.
            // this should be called from RoverScience class
            roverScience.levelMaxDistance = levelMaxDistance;
            roverScience.levelPredictionAccuracy = levelPredictionAccuracy;
            roverScience.levelAnalyzedDecay = levelAnalyzedDecay;

            GUI.setWindowPos(GUI.consoleGUI, (float)Convert.ToDouble(console_x_y_show[0]), (float)Convert.ToDouble(console_x_y_show[1]));
            GUI.consoleGUI.isOpen = Convert.ToBoolean(console_x_y_show[2]);

            roverScience.rover.anomaliesAnalyzed = anomaliesAnalyzed;
        }

        public void updateDB()
        {
            levelMaxDistance = roverScience.levelMaxDistance;
            levelPredictionAccuracy = roverScience.levelPredictionAccuracy;
            levelAnalyzedDecay = roverScience.levelAnalyzedDecay;

            console_x_y_show[0] = GUI.consoleGUI.rect.x.ToString();
            console_x_y_show[1] = GUI.consoleGUI.rect.y.ToString();
            console_x_y_show[2] = GUI.consoleGUI.isOpen.ToString();

            anomaliesAnalyzed = roverScience.rover.anomaliesAnalyzed;
        }

        public void debugPrintAll(string title = "")
        {
            Debug.Log("======== "+title+" ========");
            Debug.Log("(From RoverScience DB: debugPrintAll @ " + DateTime.Now);
            Debug.Log("levelMaxDistance: " + levelMaxDistance);
            Debug.Log("levelPredictionAccuracy: " + levelPredictionAccuracy);
            Debug.Log("levelAnalyzedDecay: " + levelAnalyzedDecay);


            

            Debug.Log("console_x_y_show: " + string.Join(",", console_x_y_show.ToArray()));
            Debug.Log("anomaliesAnalyzed: " + string.Join(",", anomaliesAnalyzed.ToArray()));
            Debug.Log("======================================");
        }
    }


    // This will handle future saving of upgrades
    [KSPScenario(ScenarioCreationOptions.AddToAllGames, new GameScenes[] { GameScenes.FLIGHT })]
    public class RoverScienceScenario : ScenarioModule
    {

        private RoverScienceDB DB
        {
            get { return RoverScienceDB.Instance; }
        }

        private RoverScience roverScience
        {
            get
            {
                return RoverScience.Instance;
            }
        }
       

        private RoverScienceGUI.GUIClass consoleGUI
        {
            get
            {
                return RoverScience.Instance.roverScienceGUI.consoleGUI;
            }
        }



        public override void OnLoad(ConfigNode node)
        {
            Debug.Log("Scenario.OnLoad Called @ " + DateTime.Now);

            Debug.Log("#### RS: Attempted to LOAD FILE VIA KSPSCENARIO v2");

            loadAnomaliesAnalyzed(node); // load anomalies
            

            // LEVELMAXDISTANCE
            if (node.HasValue("levelMaxDistance"))
            {
                DB.levelMaxDistance = Convert.ToInt32(node.GetValue("levelMaxDistance"));
            }
            else
            {
                node.AddValue("levelMaxDistance", DB.levelMaxDistance.ToString());
            }

            // LEVELPREDICTIONACCURACY
            if (node.HasValue("levelPredictionAccuracy"))
            {
                DB.levelPredictionAccuracy = Convert.ToInt32(node.GetValue("levelPredictionAccuracy"));
            }
            else
            {
                node.AddValue("levelPredictionAccuracy", DB.levelPredictionAccuracy.ToString());
            }

            // LEVELANALYZEDDECAY
            if (node.HasValue("levelAnalyzedDecay"))
            {
                DB.levelAnalyzedDecay = Convert.ToInt32(node.GetValue("levelAnalyzedDecay"));
            }
            else
            {
                node.AddValue("levelAnalyzedDecay", DB.ToString());
            }

            // CONSOLEGUI
            if (node.HasValue("console_x_y_show"))
            {
                string loadedString = node.GetValue("console_x_y_show");
                List<string> loadedStringList = new List<string>(loadedString.Split(','));

                DB.console_x_y_show = loadedStringList;

                //consoleGUI.rect.x = (float)Convert.ToDouble(loadedStringList[0]);
                //consoleGUI.rect.y = (float)Convert.ToDouble(loadedStringList[1]);
                //consoleGUI.isOpen = Convert.ToBoolean(loadedStringList[2]);
            }
            else
            {
                //string consoleDetailString = consoleGUI.rect.x + "," + consoleGUI.rect.y + "," + consoleGUI.isOpen;
                node.AddValue("console_x_y_show", string.Join(",", DB.console_x_y_show.ToArray()));
            }
        }

        public override void OnSave(ConfigNode node)
        {
            
            saveAnomaliesAnalyzed(node);

            node.SetValue("levelMaxDistance", DB.levelMaxDistance.ToString(), true);
            node.SetValue("levelPredictionAccuracy", DB.levelPredictionAccuracy.ToString(), true);
            node.SetValue("levelAnalyzedDecay", DB.levelAnalyzedDecay.ToString(), true);
            node.SetValue("console_x_y_show", string.Join(",", DB.console_x_y_show.ToArray()), true);

            //node.SetValue("levelMaxDistance", roverScience.getUpgradeLevel(RSUpgrade.maxDistance).ToString(), true);
            //node.SetValue("levelPredictionAccuracy", roverScience.getUpgradeLevel(RSUpgrade.predictionAccuracy).ToString(), true);
            //node.SetValue("levelAnalyzedDecay", roverScience.getUpgradeLevel(RSUpgrade.analyzedDecay).ToString(), true);
        }

        public void saveAnomaliesAnalyzed(ConfigNode node)
        {
            Debug.Log("Attempting to save anomalies analyzed");
            List<string> anomaliesAnalyzed = new List<string>();
            
            try
            {
				anomaliesAnalyzed = RoverScience.Instance.rover.anomaliesAnalyzed;
			}catch{
				Debug.Log("anomaliesAnalyzed came out as NULL in saveAnomaliesAnalyzed");
			}
            anomaliesAnalyzed = DB.anomaliesAnalyzed;

            if (anomaliesAnalyzed.Any())
            {
                if (anomaliesAnalyzed.Count > 1)
                {
                    node.SetValue("anomalies_visited_id", string.Join(",", anomaliesAnalyzed.ToArray()), true);
                } else
                {
                    node.SetValue("anomalies_visited_id", anomaliesAnalyzed[0], true);
                }
            } else
            {
                Debug.Log("no anomalies id to save");
            }
        }

        public void loadAnomaliesAnalyzed(ConfigNode node)
        {
            if (node.HasValue("anomalies_visited_id"))
            {
                string loadedString = node.GetValue("anomalies_visited_id");
                List<string> loadedStringList = new List<string>(loadedString.Split(','));

                Debug.Log("loadedString: " + loadedString);
                foreach (string s in loadedStringList)
                {
                    Debug.Log("ID: " + s);
                }
                Debug.Log("ID LOAD END");
                DB.anomaliesAnalyzed = loadedStringList; // load in new values in anomalies
                
            }
            else
            {
                Debug.Log("No anomalies have been analyzed");
            }

        }
    }
}
