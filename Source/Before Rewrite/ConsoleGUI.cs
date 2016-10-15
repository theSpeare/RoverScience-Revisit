using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace RoverScience
{

	public partial class RoverScienceGUI
	{

        private bool analyzeButtonPressedOnce = false;
		private string inputMaxDistance = "100";

        GUIStyle boldFont = new GUIStyle();
        GUIStyle noWrapFont = new GUIStyle();


        
        private string setRichColor(string s, string color)
        {
            // color to be inputted as "#xxxxxx";
            if (color == "green")
            {
                color = "#00ff00ff";
            }
            else if (color == "red")
            {
                color = "#ff0000ff";
            } else if (color == "blue")
            {
                color = "#add8e6ff";
            } else if (color == "yellow")
            {
                color = "#ffff00ff";
            } else if (color == "orange")
            {
                color = "#ffa500ff";
            }

            return ("<color=" + color + ">" + s + "</color>");
        }

        private string potentialFontColor(string name)
        {
            if (name == "Very High!" || name == "High")
            {
                return setRichColor(name, "green");
            } else if (name == "Normal")
            {
                return setRichColor(name, "blue");
            } else if (name == "Low")
            {
                return setRichColor(name, "yellow");
            } else
            {
                return setRichColor(name, "red");
            }
        }

        private string predictionFontColor(double percentage)
        {
            if (percentage > 70) return setRichColor(percentage.ToString() + "%", "green");
            else if (percentage >= 50) return setRichColor(percentage.ToString() + "%", "yellow");
            else return setRichColor(percentage.ToString() + "%", "red");
        }

        private string decayFontColor (double percentage)
        {
            if (percentage > 70) return setRichColor(percentage.ToString() + "%", "red");
            else if (percentage >= 50) return setRichColor(percentage.ToString() + "%", "yellow");
            else return setRichColor(percentage.ToString() + "%", "green");
        }


        private void drawRoverConsoleGUI(int windowID)
        {
            if (rover.scienceSpot.established && rover.scienceSpotReached)
            {
                consoleGUI.rect.height = 559;
            } else if (rover.scienceSpot.established)
            {
                consoleGUI.rect.height = 495;
            } else if (!rover.scienceSpot.established)
            {
                consoleGUI.rect.height = 466;
            }


            boldFont = new GUIStyle(GUI.skin.label);
            noWrapFont = new GUIStyle(GUI.skin.label);

            boldFont.fontStyle = FontStyle.Bold;
            boldFont.wordWrap = false;

            noWrapFont.wordWrap = false;

            GUILayout.BeginVertical(GUIStyles.consoleArea);
            scrollPosition = GUILayout.BeginScrollView(scrollPosition, new GUILayoutOption[] { GUILayout.Width(240), GUILayout.Height(340) });

            GUILayout.BeginHorizontal(); GUILayout.FlexibleSpace();
            GUILayout.BeginHorizontal();
            GUILayout.Label("Science Spots Analyzed: ", boldFont);
            GUILayout.Label(roverScience.amountOfTimesAnalyzed.ToString(), boldFont);
            GUILayout.EndHorizontal();
            GUILayout.FlexibleSpace(); GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal(); GUILayout.FlexibleSpace();
            GUILayout.Label("Science Loss due to re-use: " + decayFontColor(roverScience.scienceDecayPercentage), boldFont);
            GUILayout.FlexibleSpace(); GUILayout.EndHorizontal();

            GUICenter("_____________________");
            GUIBreakline();

            if (!rover.landingSpot.established)
            {
                GUILayout.Label("> No landing spot established!");
                GUILayout.Label("> You must first establish a landing spot by landing somewhere. Make sure you have wheels!");
                GUIBreakline();
                GUILayout.Label("> Rover wheels detected: " + rover.numberWheels);
                GUILayout.Label("> Rover wheels landed: " + rover.numberWheelsLanded);

            } else {
                if (!rover.scienceSpot.established)
                {
                    // PRINT OUT CONSOLE CONTENTS

                    if (roverScience.scienceDecayPercentage >= 100)
                    {
                        GUILayout.Label(setRichColor("> You have analyzed too many times.\n> Science loss is now at 100%.\n> Send another rover.", "red"));
                    } else {
                        GUILayout.Label("> Drive around to search for science spots . . .");
                        GUILayout.Label("> Currently scanning at range: " + rover.maxRadius + "m");
                        //GUILayout.Label("> Total dist. traveled searching: " + Math.Round(rover.distanceTraveledTotal, 2));
                        GUIBreakline();
                        foreach (string line in consolePrintOut)
                        {
                            GUILayout.Label(line);
                        }

                        if (vessel.mainBody.bodyName == "Kerbin")
                        {
                            GUILayout.Label(setRichColor("> WARNING - there is very little rover science for Kerbin!", "red"));
                        }
                    }

                } else {
                    if (!rover.scienceSpotReached)
                    {
                        double relativeBearing = rover.heading - rover.bearingToScienceSpot;

                        GUILayout.BeginVertical();
                        GUILayout.BeginHorizontal(); GUILayout.FlexibleSpace();
                        if (!rover.anomalyPresent)
                        {
                            GUILayout.Label(setRichColor("[POTENTIAL SCIENCE SPOT]", "yellow"));
                        } else
                        {
                            GUILayout.Label(setRichColor("[ANOMALY DETECTED]", "yellow"));
                        }

                        GUILayout.FlexibleSpace(); GUILayout.EndHorizontal();

                        GUILayout.Label("> Distance to spot (m): " + Math.Round(rover.distanceFromScienceSpot, 1));
                        //GUILayout.Label("Bearing of Site (degrees): " + Math.Round(rover.bearingToScienceSpot, 1));
                        //GUILayout.Label("Rover Bearing (degrees): " + Math.Round(rover.heading, 1));
                        //GUILayout.Label("Rel. Bearing (degrees): " + Math.Round(relativeBearing, 1));

                        if (!rover.anomalyPresent)
                        {
                            //GUILayout.Label("> Science Potential: " + rover.scienceSpot.predictedSpot + " (" + roverScience.currentPredictionAccuracy + "% confident)");
                            GUILayout.Label("> Science Prediction: " + potentialFontColor(rover.scienceSpot.predictedSpot));
                            GUILayout.Label("> Prediction is " + predictionFontColor(roverScience.currentPredictionAccuracy) + " confident");
                        }
                        else
                        {
                            GUILayout.Label("> ANOMALY DETECTED. Something interesting is nearby . . . the science potential should be very significant");
                        }
                        //GUIBreakline();
                        //GUIBreakline();

                        //This block handles writing getDriveDirection
                        //GUILayout.BeginHorizontal ();
                        //GUILayout.FlexibleSpace ();
                        //GUILayout.Label(getDriveDirection(rover.bearingToScienceSpot, rover.heading));
                        //GUILayout.FlexibleSpace ();
                        //GUILayout.EndHorizontal ();
                        GUILayout.EndVertical();
                    }
                    else
                    {

                        GUILayout.BeginHorizontal(); GUILayout.FlexibleSpace();
                        GUILayout.Label(setRichColor("[SCIENCE SPOT REACHED]", "green"));
                        GUILayout.FlexibleSpace(); GUILayout.EndHorizontal();

                        //GUILayout.Label("Total dist. traveled for this spot: " + Math.Round(rover.distanceTraveledTotal, 1));
                        //GUILayout.Label("Distance from landing site: " +
                        //Math.Round(rover.getDistanceBetweenTwoPoints(rover.scienceSpot.location, rover.landingSpot.location), 1));
                        GUILayout.BeginHorizontal();
                        GUILayout.Label("> Science Potential: " + potentialFontColor(rover.scienceSpot.potentialGenerated) + " (actual)");
                        GUILayout.EndHorizontal();

                        GUIBreakline();

                        GUILayout.Label("> NOTE: The more you analyze, the less you will get each time!");
                    }

                }
            }

            GUILayout.EndScrollView();
            GUILayout.EndVertical();

            // ACTIVATE ROVER BUTTON
            if (rover.scienceSpotReached)
            {
               
                if (!analyzeButtonPressedOnce)
                {
                    if (GUILayout.Button("Analyze Science", GUILayout.Height(60)))
                    {
                        if (roverScience.container.GetStoredDataCount() == 0)
                        {
                            if (rover.scienceSpotReached)
                            {
                                analyzeButtonPressedOnce = true;
                                consolePrintOut.Clear();

                            }
                            else if (!rover.scienceSpotReached)
                            {
                                ScreenMessages.PostScreenMessage("Cannot analyze - Get to the science spot first!", 3, ScreenMessageStyle.UPPER_CENTER);
                            }
                        }
                        else
                        {
                            ScreenMessages.PostScreenMessage("Cannot analyze - Rover Brain already contains data!", 3, ScreenMessageStyle.UPPER_CENTER);
                        }
                    }
                }
                else
                {
                    GUILayout.BeginHorizontal();
                    if (GUILayout.Button("Confirm"))
                    {
                        analyzeButtonPressedOnce = false;
                        roverScience.analyzeScienceSample();
                    }
                    if (GUILayout.Button("Cancel"))
                    {
                        analyzeButtonPressedOnce = false;
                    }
                    GUILayout.EndHorizontal();
                }
            }

            if (rover.scienceSpot.established)
            {
                if (GUILayout.Button("Reset Science Spot"))
                {
                    rover.scienceSpot.established = false;
                    rover.resetDistanceTraveled();
                    DrawWaypoint.Instance.DestroyInterestingObject();
                    DrawWaypoint.Instance.hideMarker();
                    consolePrintOut.Clear();

                }
            }

            //if (GUILayout.Button ("Reorient from Part")) {
            //roverScience.command.MakeReference ();
            //}
            GUIBreakline();
            GUIBreakline();
            if (roverScience.scienceDecayPercentage < 100)
            {
            GUILayout.BeginHorizontal();
            inputMaxDistance = GUILayout.TextField(inputMaxDistance, 5, new GUILayoutOption[] { GUILayout.Width(40) });

            
                if (GUILayout.Button("Set Scan Range [Max: " + roverScience.currentMaxDistance + "]")) {

                    int inputMaxDistanceInt;
                    bool isNumber = int.TryParse(inputMaxDistance, out inputMaxDistanceInt);


                    if ((isNumber) && (inputMaxDistanceInt <= roverScience.currentMaxDistance) && (inputMaxDistanceInt >= 40))
                    {
                        rover.maxRadius = inputMaxDistanceInt;
                        Debug.Log("Set maxRadius to input: " + rover.maxRadius);
                        ScreenMessages.PostScreenMessage("Now scanning for science spots at range: " + rover.maxRadius, 3, ScreenMessageStyle.UPPER_CENTER);
                    }

                    if (inputMaxDistanceInt > roverScience.currentMaxDistance)
                    {
                        ScreenMessages.PostScreenMessage("Cannot set scan range over max distance of: " + roverScience.currentMaxDistance, 3, ScreenMessageStyle.UPPER_CENTER);
                    } else if (inputMaxDistanceInt < 40)
                    {
                        ScreenMessages.PostScreenMessage("Minimum of 40m scan range is required", 3, ScreenMessageStyle.UPPER_CENTER);
                    }
                }
            }

            GUILayout.EndHorizontal();

			if (GUILayout.Button ("Upgrade Menu")) {
				upgradeGUI.toggle ();
			}

			GUILayout.Space (5);
			if (GUILayout.Button ("Close and Shutdown")) {
				rover.scienceSpot.established = false;
				rover.resetDistanceTraveled ();
				consolePrintOut.Clear ();

                DrawWaypoint.Instance.hideMarker();

				consoleGUI.hide ();
				upgradeGUI.hide ();
			}
			GUI.DragWindow ();
		}


        private string getDriveDirection(double destination, double currentHeading)
        {
            // This will calculate the closest angle to the destination, given a current heading.
            // Everything here will be in degrees, not radians
           
            // Shift destination angle to 000 bearing. Apply this shift to the currentHeading in the same direction.
            double delDestAngle = 0;
            double shiftedCurrentHeading = 0;

            if (destination > 180) {
                // Delta will be clockwise
                delDestAngle = 360 - destination;
                shiftedCurrentHeading = currentHeading + delDestAngle;

                if (shiftedCurrentHeading > 360) shiftedCurrentHeading -= 360;
            } else {
                // Delta will be anti-clockwise
                delDestAngle = destination;
                shiftedCurrentHeading = currentHeading - delDestAngle;

                if (shiftedCurrentHeading < 0) shiftedCurrentHeading += 360;
            }

            double absShiftedCurrentHeading = Math.Abs(shiftedCurrentHeading);

			if (absShiftedCurrentHeading < 6) {
                return "DRIVE FORWARD";
            }

			if ((absShiftedCurrentHeading > 174) && (absShiftedCurrentHeading < 186)) {
                return "TURN AROUND";
            }

			if (absShiftedCurrentHeading < 180) {
                return "TURN LEFT";
            }

			if (absShiftedCurrentHeading > 180) {
                return "TURN RIGHT";
            }



            return "ERROR: FAILED TO RESOLVE DRIVE DIRECTION";

        }


	}


}

