using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace RoverScience
{
	public partial class RoverScienceGUI
	{
        private string anomalyVisitedAdd = "1";

		private void drawDebugGUI (int windowID)
		{

			GUILayout.BeginVertical ();

			GUILayout.Label (roverScience.RSVersion);
            
            GUILayout.Label ("# Data Stored: " + roverScience.container.GetStoredDataCount ());
			GUILayout.Label ("distCheck: " + Math.Round(rover.distanceCheck, 2));
			GUILayout.Label ("distTrav: " + Math.Round(rover.distanceTraveled));
			GUILayout.Label ("distTravTotal: " + Math.Round(rover.distanceTraveledTotal));
			GUIBreakline ();
            GUILayout.Label("levelAnalyzedDecay: " + roverScience.levelAnalyzedDecay);
            GUILayout.Label ("currentScalarDecay: " + roverScience.scienceDecayScalar);
			GUILayout.Label ("scienceDistanceScalarBoost: " + roverScience.scienceMaxRadiusBoost);

			GUILayout.Label ("ScienceSpot potential: " + rover.scienceSpot.potentialGenerated);

			GUILayout.Label ("generatedScience: " + rover.scienceSpot.potentialScience);
			GUILayout.Label ("with decay: " + rover.scienceSpot.potentialScience * roverScience.scienceDecayScalar);
			GUILayout.Label ("with distanceScalarBoost & decay & bodyScalar: " + rover.scienceSpot.potentialScience * 
				roverScience.scienceDecayScalar * roverScience.scienceMaxRadiusBoost * roverScience.bodyScienceScalar);

            GUIBreakline();
            GUILayout.Label("Distance travelled for spot: " + rover.distanceTraveledTotal);
            
            GUIBreakline();
            GUILayout.Label("consoleGUI height: " + consoleGUI.rect.height);

            GUIBreakline();
            GUILayout.Label("Closest Anomaly ID: " + roverScience.rover.closestAnomaly.id);
            GUILayout.Label("Closest Anomaly Name: " + roverScience.rover.closestAnomaly.name);
            GUILayout.Label("Has current anomaly been analyzed? " + "[" + Anomalies.Instance.hasCurrentAnomalyBeenAnalyzed() + "]");



            GUILayout.BeginHorizontal();
            anomalyVisitedAdd = GUILayout.TextField(anomalyVisitedAdd, 3, new GUILayoutOption[] { GUILayout.Width(50) });
            if (GUILayout.Button("add anomaly ID"))
            {
                rover.anomaliesAnalyzed.Add(anomalyVisitedAdd);
            }
            if (GUILayout.Button("X"))
            {
                rover.anomaliesAnalyzed.Clear();
            }
            GUILayout.EndHorizontal();
            GUILayout.Label("anomaly id visited: " + string.Join(",", rover.anomaliesAnalyzed.ToArray()));




			if (GUILayout.Button ("Find Science Spot")) {
				rover.scienceSpot.setLocation (random: true);
			}
            


            if (GUILayout.Button ("Cheat Spot Here")) {
				if ((!rover.scienceSpot.established) && (consoleGUI.isOpen)) {
					rover.scienceSpot.setLocation (vessel.longitude, vessel.latitude);
				} else if (rover.scienceSpot.established){
					rover.scienceSpot.reset ();
				}
			}

			if (GUILayout.Button ("CLEAR CONSOLE")) {
				consolePrintOut.Clear ();
			}

			GUIBreakline ();

			GUILayout.Label("Times Analyzed: " + roverScience.amountOfTimesAnalyzed);

			GUILayout.BeginHorizontal ();
			if (GUILayout.Button ("-")) {
				if (roverScience.amountOfTimesAnalyzed > 0)
					roverScience.amountOfTimesAnalyzed--;
			}

			if (GUILayout.Button ("+")) {
				roverScience.amountOfTimesAnalyzed++;
			}

			if (GUILayout.Button("0")){
				roverScience.amountOfTimesAnalyzed = 0;
			}
			GUILayout.EndHorizontal ();

			GUIBreakline ();
			GUIBreakline ();


			GUILayout.Label("Dist. Upgraded Level: " + roverScience.levelMaxDistance);

			GUILayout.BeginHorizontal ();
			if (GUILayout.Button ("-")) {
				if (roverScience.levelMaxDistance > 1)
					roverScience.levelMaxDistance--;
			}

			if (GUILayout.Button ("+")) {
				roverScience.levelMaxDistance++;
			}

			if (GUILayout.Button("0")){
				roverScience.levelMaxDistance = 1;
			}
			GUILayout.EndHorizontal ();



			GUILayout.Label("Acc. Upgraded Level: " + roverScience.levelPredictionAccuracy);

			GUILayout.BeginHorizontal ();
			if (GUILayout.Button ("-")) {
				if (roverScience.levelPredictionAccuracy > 1)
					roverScience.levelPredictionAccuracy--;
			}

			if (GUILayout.Button ("+")) {
				roverScience.levelPredictionAccuracy++;
			}

			if (GUILayout.Button("0")){
				roverScience.levelPredictionAccuracy = 1;
			}
			GUILayout.EndHorizontal ();

            GUILayout.Label("levelAnalyzedDecay: " + roverScience.levelAnalyzedDecay);

            GUILayout.BeginHorizontal();
            if (GUILayout.Button("-"))
            {
                if (roverScience.levelAnalyzedDecay > 1)
                    roverScience.levelAnalyzedDecay--;
            }

            if (GUILayout.Button("+"))
            {
                roverScience.levelAnalyzedDecay++;
            }

            if (GUILayout.Button("0"))
            {
                roverScience.levelAnalyzedDecay = 1;
            }
            GUILayout.EndHorizontal();

            if (GUILayout.Button ("+500 Science")) {
                ResearchAndDevelopment.Instance.CheatAddScience(500);
            }

			if (GUILayout.Button ("-500 Science")) {
                ResearchAndDevelopment.Instance.CheatAddScience(-500);
            }

			GUIBreakline ();
			if (GUILayout.Button ("Close Window")) {
				debugGUI.hide ();
			}

			GUILayout.EndVertical ();

			GUI.DragWindow ();
		}

		private void GUIBreakline ()
		{
			GUILayout.BeginHorizontal ();
			GUILayout.EndHorizontal ();
		}

        private void GUICenter(string s)
        {
            GUILayout.BeginHorizontal(); GUILayout.FlexibleSpace();
            GUILayout.Label(s);
            GUILayout.FlexibleSpace(); GUILayout.EndHorizontal();
        }
    }
}