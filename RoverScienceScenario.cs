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
        public override void OnLoad(ConfigNode node)
        {
            Debug.Log("#### RS: Attempted to LOAD FILE VIA KSPSCENARIO v2");

            loadAnomaliesAnalyzed(node);

            // LEVELMAXDISTANCE
            if (node.HasValue("levelMaxDistance"))
            {
                RoverScience.Instance.levelMaxDistance = Convert.ToInt32(node.GetValue("levelMaxDistance"));
            }
            else
            {
                node.AddValue("levelMaxDistance", RoverScience.Instance.levelMaxDistance.ToString());
            }

            // LEVELPREDICTIONACCURACY
            if (node.HasValue("levelPredictionAccuracy"))
            {
                RoverScience.Instance.levelPredictionAccuracy = Convert.ToInt32(node.GetValue("levelPredictionAccuracy"));
            }
            else
            {
                node.AddValue("levelPredictionAccuracy", RoverScience.Instance.levelPredictionAccuracy.ToString());
            }
        }

        public override void OnSave(ConfigNode node)
        {
            saveAnomaliesAnalyzed(node);

            node.SetValue("levelMaxDistance", RoverScience.Instance.getUpgradeLevel(RSUpgrade.maxDistance).ToString(), true);
            node.SetValue("levelPredictionAccuracy", RoverScience.Instance.getUpgradeLevel(RSUpgrade.predictionAccuracy).ToString(), true);
        }

        public void saveAnomaliesAnalyzed(ConfigNode node)
        {
            Debug.Log("Attempting to save anomalies analyzed");
            List<string> anomaliesAnalyzed = RoverScience.Instance.rover.anomaliesAnalyzed;

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

                RoverScience.Instance.rover.anomaliesAnalyzed = loadedStringList; // load in new values in anomalies
            }
            else
            {
                Debug.Log("No anomalies loaded / have been analyzed");
            }

           
        }
    }
}
