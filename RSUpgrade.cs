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
            }
            else
            {
                node.AddValue("levelMaxDistance", RoverScience.Instance.levelMaxDistance.ToString());
            }

            // Check for existing node, (otherwise create)
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
            node.SetValue("levelMaxDistance", RoverScience.Instance.getUpgradeLevel(RSUpgrade.maxDistance).ToString(), true);
            node.SetValue("levelPredictionAccuracy", RoverScience.Instance.getUpgradeLevel(RSUpgrade.predictionAccuracy).ToString(), true);
        }


    }

}

