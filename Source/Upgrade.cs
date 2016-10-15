using System;
using System.Collections.Generic;



namespace RoverScience
{
    public class Upgrades
    {
        public Upgrade maxDistanceScan;
        public Upgrade predictionAccuracy;
        public Upgrade analyzeRepeatEffeciency;

        public class Upgrade
        {
            public string name { get; private set; }

            private int _level;
            public int level
            {
                get
                {
                    return _level;
                }
                set
                {
                    _level = value;
                    if (_level > max)
                    {
                        _level = max;
                    }
                }
            }

            public int max { get; private set; }

            private Dictionary<int, double> upgradeValues;
            private Dictionary<int, double> upgradeCosts;

            public Upgrade(string _name, Dictionary<int, double> _upgradeValues, Dictionary<int, double> _upgradeCosts)
            {
                name = _name;
                upgradeValues = _upgradeValues;
                upgradeCosts = _upgradeCosts;

                level = 1;
                max = 5;
            }

            public bool AddLevel(int _add)
            {
                level += _add;
                return true;
            }

            public bool SubtractLevel(int _add)
            {
                level -= _add;
                return true;
            }

            public bool ResetLevel()
            {
                level = 1;
                return true;
            }

            public double GetUpgradeValue(int level)
            {
                return upgradeValues[level];
            }
        }

        public void GetUpgradeValues(ConfigNode node)
        {
            try
            {
                ////////////////////////////////////////////////////////////////////////////////////////////////////////

                ConfigNode upgradeNode = node.GetNode("MaxDistanceScan");
                Dictionary<int, double> upgradeValues = new Dictionary<int, double>();
                Dictionary<int, double> upgradeCosts = new Dictionary<int, double>();

                foreach (string value in upgradeNode.GetValues("upgrade"))
                {
                    string[] numberStrings = value.Split(',');
                    upgradeValues.Add(Convert.ToInt32(numberStrings[0]), Convert.ToDouble(numberStrings[1]));
                    upgradeCosts.Add(Convert.ToInt32(numberStrings[0]), Convert.ToDouble(numberStrings[2]));
                }

                maxDistanceScan = new Upgrade("Max Distance Scan", upgradeValues, upgradeCosts);

                ////////////////////////////////////////////////////////////////////////////////////////////////////////

                upgradeNode = node.GetNode("PredictionAccuracy");
                upgradeValues = new Dictionary<int, double>();
                upgradeCosts = new Dictionary<int, double>();

                foreach (string value in upgradeNode.GetValues("upgrade"))
                {
                    string[] numberStrings = value.Split(',');
                    upgradeValues.Add(Convert.ToInt32(numberStrings[0]), Convert.ToDouble(numberStrings[1]));
                    upgradeCosts.Add(Convert.ToInt32(numberStrings[0]), Convert.ToDouble(numberStrings[2]));
                }

                predictionAccuracy = new Upgrade("Prediction Accuracy", upgradeValues, upgradeCosts);

                ////////////////////////////////////////////////////////////////////////////////////////////////////////

                upgradeNode = node.GetNode("AnalyzeRepeatEfficiency");
                upgradeValues = new Dictionary<int, double>();
                upgradeCosts = new Dictionary<int, double>();

                foreach (string value in upgradeNode.GetValues("upgrade"))
                {
                    string[] numberStrings = value.Split(',');
                    upgradeValues.Add(Convert.ToInt32(numberStrings[0]), Convert.ToDouble(numberStrings[1]));
                    upgradeCosts.Add(Convert.ToInt32(numberStrings[0]), Convert.ToDouble(numberStrings[2]));
                }

                analyzeRepeatEffeciency = new Upgrade("Analyze Repeat Efficiency", upgradeValues, upgradeCosts);

                ////////////////////////////////////////////////////////////////////////////////////////////////////////
            } catch
            {
                RSDebug.Log("Upgrade.GetUpgradeValues() - ERROR");
            }
        }
    }

}
