using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KSP;
using UnityEngine;
using System.Text.RegularExpressions;

namespace RoverScience
{
    [KSPAddon(KSPAddon.Startup.Flight, true)]
    public class Anomalies : MonoBehaviour
    {
        public static Anomalies Instance = null;
        public static Dictionary<string, List<Anomaly>> anomaliesDict = new Dictionary<string, List<Anomaly>>();

        public class Anomaly
        {
            public string name = "anomaly";
            public COORDS location = new COORDS();
            //public double longitude = 0;
            //public double latitude = 0;
            public string id = "NA";
            // surface altitude is determined by DrawWaypoint
        }

        public Anomalies()
        {
            Instance = this;
            Debug.Log("RS: Attempted to load anomaly coordinates");
            loadAnomalies();
        }

        public bool hasCurrentAnomalyBeenAnalyzed()
        {
            Rover rover = RoverScience.Instance.rover;
            string closestAnomalyID = rover.closestAnomaly.id;
            if (rover.anomaliesAnalyzed.Contains(closestAnomalyID))
            {
                return true;
            } else
            {
                return false;
            }

        }

        public List<Anomaly> getAnomalies(string bodyName)
        {
            if (anomaliesDict.ContainsKey(bodyName))
            {
                return anomaliesDict[bodyName];
            } else
            {
                Debug.Log("###### RS: getAnomalies KEY DOES NOT EXIST!");
                return null;
            }
        }

        public bool hasAnomalies(string bodyName)
        {
            if (anomaliesDict.ContainsKey(bodyName))
            {
                return true;
            } else
            {
                return false;
            }
        }

        private void loadAnomalies()
        {
            // anomaliesDict contains a list of [Anomaly]s that each contain longitude/latitude, name and id
            // load anomalies from Anomalies.cfg
            try
            {
                string fileName = KSPUtil.ApplicationRootPath + "GameData/RoverScience/Anomalies.cfg";
                Debug.Log("loadAnomlies HAS ATTEMPTED TO LOAD FROM THIS PATH: " + fileName);

                ConfigNode mainNode = ConfigNode.Load(fileName);

                foreach (ConfigNode bodyNode in mainNode.GetNodes("BODY"))
                {
                    List<Anomaly> _anomalyList = new List<Anomaly>();
                    
                    foreach (ConfigNode anomalyNode in bodyNode.GetNodes("anomaly"))
                    {
                        Anomaly _anomaly = new Anomaly();
                        string[] latlong = Regex.Split(anomalyNode.GetValue("position"), " : ");
                        _anomaly.location.latitude = Convert.ToDouble(latlong[0]);
                        _anomaly.location.longitude = Convert.ToDouble(latlong[1]);

                        if (anomalyNode.HasValue("name"))
                        {
                            _anomaly.name = anomalyNode.GetValue("name");
                        }

                        if (anomalyNode.HasValue("id"))
                        {
                            _anomaly.id = anomalyNode.GetValue("id");
                        }


                        _anomalyList.Add(_anomaly);
                    }

                    anomaliesDict.Add(bodyNode.GetValue("name"), _anomalyList);
                }
            }
            catch
            {
                Debug.Log("Catch Anomaly Coordinates Problem");
            }
        }
    }
}
