using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace RoverScience
{
	public enum RSUpgrade
	{
		maxDistance, predictionAccuracy
	}


    // This will handle future saving of upgrades
    [KSPScenario(ScenarioCreationOptions.AddToAllGames, new GameScenes[] { GameScenes.FLIGHT })]
    public class RSUpgradeSystem : ScenarioModule
    {
        public override void OnLoad(ConfigNode node)
        {
            Debug.Log("#### RS: Attempted to LOAD FILE VIA KSPSCENARIO v2");

            // Check for existing node, (otherwise create)
            if (node.HasValue("levelMaxDistance"))
            {
                RoverScience.Instance.levelMaxDistance = Convert.ToInt32(node.GetValue("levelMaxDistance"));
                Debug.Log("------------------------------------>#1 : Found levelMaxDistance node as" + node.GetValue("levelMaxDistance"));
            }
            else
            {
                node.AddValue("levelMaxDistance", RoverScience.Instance.levelMaxDistance.ToString());
                Debug.Log("------------------------------------>#1 : FAILED TO FIND PREVIOUS VALUE");
            }

            // Check for existing node, (otherwise create)
            if (node.HasValue("levelPredictionAccuracy"))
            {
                RoverScience.Instance.levelPredictionAccuracy = Convert.ToInt32(node.GetValue("levelPredictionAccuracy"));
                Debug.Log("------------------------------------>#2 : Found levelDetectionAccuracy node as" + node.GetValue("levelPredictionAccuracy"));
            }
            else
            {
                node.AddValue("levelPredictionAccuracy", RoverScience.Instance.levelPredictionAccuracy.ToString());
                Debug.Log("------------------------------------>#2 : FAILED TO FIND PREVIOUS VALUE");
            }


            //RoverScience.Instance.levelMaxDistance = Convert.ToInt32(node.GetValue("maxDistance"));
            //RoverScience.Instance.levelPredictionAccuracy = Convert.ToInt32(node.GetValue("levelPredictionAccuracy"));

            Debug.Log("Loaded in values (dist, accuracy): " + RoverScience.Instance.levelMaxDistance.ToString() + " " + RoverScience.Instance.levelPredictionAccuracy.ToString());
        }

        public override void OnSave(ConfigNode node)
        {
            Debug.Log("#### RS: Attempted to SAVE FILE VIA KSPSCENARIO");
            Debug.Log("Values to be saved (dist, accuracy): " + RoverScience.Instance.levelMaxDistance.ToString() + " " + RoverScience.Instance.levelPredictionAccuracy.ToString());
            node.SetValue("levelMaxDistance", RoverScience.Instance.getUpgradeLevel(RSUpgrade.maxDistance).ToString(), true);
            node.SetValue("levelPredictionAccuracy", RoverScience.Instance.getUpgradeLevel(RSUpgrade.predictionAccuracy).ToString(), true);
        }


    }

}

