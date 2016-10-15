using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace RoverScience
{
    [KSPAddon(KSPAddon.Startup.SpaceCentre, true)]
    class ConfigReader : MonoBehaviour
    {
        // CONFIG READER READS STUFF FROM CFG FILE, SENDS TO STATIC DATABASE

        ConfigNode RoverScience_CFG;
        private string fileName = KSPUtil.ApplicationRootPath + "GameData/RoverScience/Settings.cfg";

        public ConfigReader()
        {
            RSDebug.Log("ConfigReader constructed", 2);

            RoverScience_CFG = ConfigNode.Load(fileName).GetNode("ROVERSCIENCE");

            LoadAnomalies();
            LoadUpgrades();
        }

        private void LoadAnomalies()
        {
            // anomaliesDict contains a list of [Anomaly]s that each contain longitude/latitude, name and id
            // load anomalies from Anomalies.cfg
            try
            {

                ConfigNode anomaliesNode = RoverScience_CFG.GetNode("ANOMALIES");
                Database.anomalies.LoadAnomaliesFromNode(anomaliesNode);
            }
            catch { }
        }

        private void LoadUpgrades()
        {
            try
            {
                ConfigNode upgradesNode = RoverScience_CFG.GetNode("UPGRADES");
                Database.upgrades.GetUpgradeValues(upgradesNode);
            }
            catch { }
        }
    }
}
