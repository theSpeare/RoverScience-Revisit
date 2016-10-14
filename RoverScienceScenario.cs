using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace RoverScience
{
    // This will handle future saving of upgrades
    [KSPScenario(ScenarioCreationOptions.AddToAllGames, new GameScenes[] { GameScenes.FLIGHT })]
    public class RoverScienceScenario : ScenarioModule
    {

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

        public static RoverScienceScenario Instance = null;

        public Database DB = new Database();

        public RoverScienceScenario()
        {
            Debug.Log("--------------------------------> RS-Scenario Init @" + DateTime.Now);
            Instance = this;
        }

        public override void OnLoad(ConfigNode node)
        {
            Debug.Log("--------------------------------> RS-Scenario OnLoad @" + DateTime.Now);
            Debug.Log("#### RS: Attempted to LOAD FILE VIA KSPSCENARIO v2");

            // RoverScienceScenario will only update DB. RoverScience should pull from it.
            Debug.Log("[A]");
            LoadAnomaliesAnalyzed(node); // load anomalies
            Debug.Log("[B]");
            LoadDBValues(node);
            Debug.Log("[C]");
        }

        public override void OnSave(ConfigNode node)
        {
            Debug.Log("--------------------------------> RS-Scenario OnSave @" + DateTime.Now);

            Debug.Log("[1]");
            // Need to update DB here:
            if (RoverScience.Exists)
            {
                // If RoverScience is active, grab values off RS, update DB
                // Then save everything to file
                Debug.Log("[2]");
                UpdateDB();
                Debug.Log("[3]");
                SaveAnomaliesAnalyzed(node);
                Debug.Log("[4]");
                SaveDBValues(node);
                Debug.Log("[5]");
            }

        }

        public void UpdateDB()
        {
            DB.levelMaxDistance = roverScience.levelMaxDistance;
            DB.levelPredictionAccuracy = roverScience.levelPredictionAccuracy;
            DB.levelAnalyzedDecay = roverScience.levelAnalyzedDecay;

            DB.console_x_y_show = new List<string>();
            DB.console_x_y_show.Add(consoleGUI.rect.x.ToString());
            DB.console_x_y_show.Add(consoleGUI.rect.y.ToString());
            DB.console_x_y_show.Add(consoleGUI.isOpen.ToString());

            DB.anomaliesAnalyzed = roverScience.rover.anomaliesAnalyzed;
        }

        public void LoadDBValues(ConfigNode node)
        {

            // LEVELMAXDISTANCE
            if (node.HasValue("levelMaxDistance"))
            {
                DB.levelMaxDistance = Convert.ToInt32(node.GetValue("levelMaxDistance"));
            }

            // LEVELPREDICTIONACCURACY
            if (node.HasValue("levelPredictionAccuracy"))
            {
                DB.levelPredictionAccuracy = Convert.ToInt32(node.GetValue("levelPredictionAccuracy"));
            }

            // LEVELANALYZEDDECAY
            if (node.HasValue("levelAnalyzedDecay"))
            {
                DB.levelAnalyzedDecay = Convert.ToInt32(node.GetValue("levelAnalyzedDecay"));
            }

            // CONSOLEGUI
            if (node.HasValue("console_x_y_show"))
            {
                string loadedString = node.GetValue("console_x_y_show");
                List<string> loadedStringList = new List<string>(loadedString.Split(','));
                DB.console_x_y_show = loadedStringList;
            }
        }

        public void SaveDBValues(ConfigNode node)
        {
            node.SetValue("levelMaxDistance", DB.levelMaxDistance.ToString(), true);
            node.SetValue("levelPredictionAccuracy", DB.levelPredictionAccuracy.ToString(), true);
            node.SetValue("levelAnalyzedDecay", DB.levelAnalyzedDecay.ToString(), true);
            node.SetValue("console_x_y_show", string.Join(",", DB.console_x_y_show.ToArray()), true);
        }

        public void SaveAnomaliesAnalyzed(ConfigNode node)
        {
            Debug.Log("Attempting to save anomalies analyzed");
            List<string> anomaliesAnalyzed = DB.anomaliesAnalyzed;

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

        public void LoadAnomaliesAnalyzed(ConfigNode node)
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
