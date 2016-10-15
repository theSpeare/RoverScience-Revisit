using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using UnityEngine;

namespace RoverScience
{
    public class Anomalies
    {
        // anomaliesDict is <body_name, list of anomalies>
        private Dictionary<string, List<Anomaly>> anomaliesDict = new Dictionary<string, List<Anomaly>>();

        public class Anomaly
        {
            public string name = "anomaly";
            public string id = "NA";
            public Coord location;
        }

        public Dictionary<string, List<Anomaly>> GetAnomaliesDict()
        {
            return anomaliesDict;
        }

        public void LoadAnomaliesFromNode(ConfigNode mainNode)
        {
            // anomaliesDict contains a list of [Anomaly]s that each contain longitude/latitude, name and id
            // load anomalies from Anomalies.cfg
            try
            {
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
