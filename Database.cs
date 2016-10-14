using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace RoverScience
{
    public class Database
    {
        public int levelMaxDistance;
        public int levelPredictionAccuracy;
        public int levelAnalyzedDecay;

        public List<string> console_x_y_show;
        public List<string> anomaliesAnalyzed;

        public Database()
        {
            levelMaxDistance = 1;
            levelPredictionAccuracy = 1;
            levelAnalyzedDecay = 2;

            console_x_y_show = new List<string>();
            anomaliesAnalyzed = new List<string>();
        }
    }
}