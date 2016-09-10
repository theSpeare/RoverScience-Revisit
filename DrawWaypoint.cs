using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using KSP;
using KSP.UI.Screens;
using Contracts;
using FinePrint;
using FinePrint.Utilities;

namespace RoverScience
{
    // All code taken from Waypoint Manager mod
    // Still in experimental as I try to figure out what I'm doing here
    [KSPAddon(KSPAddon.Startup.Flight, false)]
    public class DrawWaypoint : MonoBehaviour
    {
        private GameObject marker = null;
        float markerSize = 30;
        float markerSizeMax = 30;

        float markerAlpha = 0.7f;
        float maxAlpha = 0.7f;
        float minAlpha = 0.5f;

        public static DrawWaypoint Instance = null;
        Color markerColorRed = Color.red;
        Color markerColorGreen = Color.green;

        private void Start()
        {
            Debug.Log("Attempting to create sphere");
            Instance = this;

            marker = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            Destroy(marker.GetComponent("SphereCollider"));
            // Set initial position
            //marker.transform.localScale = new Vector3(markerSize, markerSize, markerSize);
            marker.transform.localScale = new Vector3(markerSize, markerSize, markerSize);
            marker.transform.position = FlightGlobals.currentMainBody.GetWorldSurfacePosition(0, 0, 0);

            hideMarker(); // do not render marker yet

            // Set marker material, color and alpha
            marker.GetComponent<MeshRenderer>().material = new Material(Shader.Find("Transparent/Diffuse"));

            markerColorRed.a = markerAlpha; // max alpha
            markerColorGreen.a = markerAlpha; // max alpha

            marker.GetComponent<MeshRenderer>().material.color = markerColorRed; // set to red on awake
            Debug.Log("Reached end of marker creation");
        }

        public void setMarkerLocation(double longitude, double latitude)
        {
            Vector3 bottomPoint = FlightGlobals.currentMainBody.GetWorldSurfacePosition(latitude, longitude, 0);
            Vector3 topPoint = FlightGlobals.currentMainBody.GetWorldSurfacePosition(latitude, longitude, 1000);

            double surfaceAltitude = getSurfaceAltitude(longitude, latitude);
            Debug.Log("Drawing marker @ (long/lat/alt): " + longitude.ToString() + " " + latitude.ToString() + " " + surfaceAltitude.ToString());
            marker.transform.position = FlightGlobals.currentMainBody.GetWorldSurfacePosition(latitude, longitude, surfaceAltitude);

            //marker.transform.up = cylinderDirectionUp;

            marker.transform.localScale = new Vector3(markerSizeMax, markerSizeMax, markerSizeMax);
            markerColorRed.a = maxAlpha;

            //attempt to get raycast surface altitude
            
        }

        public double getSurfaceAltitude(double longitude, double latitude)
        {
            double altitude = 20000;
            RaycastHit hit;
            Vector3d topPoint = FlightGlobals.currentMainBody.GetWorldSurfacePosition(latitude, longitude, altitude);
            Vector3d bottomPoint = FlightGlobals.currentMainBody.GetWorldSurfacePosition(latitude, longitude, -altitude);

            Debug.Log("RSR: attempting raycast");
            if (Physics.Raycast(topPoint, (bottomPoint-topPoint), out hit, Mathf.Infinity, 1<<15))
            {
                return (altitude - hit.distance);
            } else
            {
                Debug.Log("RSR: No collision detected!");
            }

            return -1;
        }

        public void showMarker()
        {
            if (RoverScience.Instance.rover.scienceSpot.established)
            {
                marker.GetComponent<MeshRenderer>().enabled = true;
            }
        }

        public void hideMarker()
        {
            marker.GetComponent<MeshRenderer>().enabled = false;
        }

        public void toggleMarker()
        {

            if (!marker.GetComponent<MeshRenderer>().enabled)
            {
                showMarker();
            } else
            {
                hideMarker();
            }
        }

        private void changeSpherewithDistance(Rover rover)
        {
            float distance = (float)rover.distanceFromScienceSpot;

            if ((distance < markerSizeMax) && (distance > 5))
            {
                // Reduce sphere size with proximity
                markerSize = distance;
                marker.transform.localScale = new Vector3(markerSize, 2000, markerSize);

                // Reduce alpha with proximity
                markerAlpha = (float)(distance / markerSizeMax);
                if (markerAlpha >= maxAlpha)
                {
                    markerAlpha = maxAlpha;
                } else if (markerAlpha <= minAlpha)
                {
                    markerAlpha = minAlpha;
                }

                markerColorRed.a = markerAlpha;
                markerColorGreen.a = markerAlpha;

            }

            if ((distance <= 3) && (distance >= 0))
            {
                marker.GetComponent<MeshRenderer>().material.color = markerColorGreen;
            } else {
                marker.GetComponent<MeshRenderer>().material.color = markerColorRed;
            }

            //Debug.Log("dist, dist/50, alpha: [" + distance + " / " + distance / 50 + " / " + markerAlpha + "]");
        }
        
        private void Update()
        {
            if (marker.GetComponent<MeshRenderer>().enabled)
            {
                changeSpherewithDistance(RoverScience.Instance.rover);

                // avoid rendering if rover is somehow flown up super high by player
                if (FlightGlobals.ActiveVessel.altitude > 5000)
                {
                    hideMarker();
                }
                else
                {
                    showMarker();
                }

            }
        }

        private void OnDestroy()
        {
            Destroy(marker);
        }


    }
}
