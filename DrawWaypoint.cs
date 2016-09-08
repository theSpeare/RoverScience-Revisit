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
        Color sphereColorRed = Color.red;
        Color sphereColorGreen = Color.green;

        private void Start()
        {
            Debug.Log("Attempting to create sphere");
            Instance = this;

            marker = GameObject.CreatePrimitive(PrimitiveType.Capsule);
            Destroy(marker.GetComponent("CapsuleCollider"));
            // Set initial position
            //marker.transform.localScale = new Vector3(markerSize, markerSize, markerSize);
            marker.transform.localScale = new Vector3(markerSize, 2000, markerSize);
            marker.transform.position = FlightGlobals.currentMainBody.GetWorldSurfacePosition(0, 0, 0);

            //Quaternion rotationAngleUp = Quaternion.FromToRotation(marker.transform.transform)
            Debug.Log("RSR: trying to rotate cylinder");
            Debug.Log("before marker rotation:");
            Debug.Log(marker.transform.rotation);

            //marker.transform.rotation = marker.transform.rotation * Quaternion.FromToRotation(marker.transform.forward, FlightGlobals.ActiveVessel.terrainNormal);
            //marker.transform.forward = FlightGlobals.ActiveVessel.terrainNormal;

            Debug.Log("marker transform vector forward:");
            Debug.Log(marker.transform.forward);

            Debug.Log("marker transform vector up:");
            Debug.Log(marker.transform.up);

            Debug.Log("marker transform vector terrain normal:");
            Debug.Log(FlightGlobals.ActiveVessel.terrainNormal);

            Debug.Log("after marker rotation:");
            Debug.Log(marker.transform.rotation);

            Debug.Log("RSR: ended rotate cylinder");

            hideMarker(); // do not render marker yet

            // Set marker material, color and alpha
            marker.GetComponent<MeshRenderer>().material = new Material(Shader.Find("Transparent/Diffuse"));

            sphereColorRed.a = markerAlpha; // max alpha
            sphereColorGreen.a = markerAlpha; // max alpha

            marker.GetComponent<MeshRenderer>().material.color = sphereColorRed;
            Debug.Log("Reached end of marker creation");
        }

        public void setMarkerLocation(double longitude, double latitude)
        {
            double altitude = FlightGlobals.ActiveVessel.altitude;

            Debug.Log("Drawing marker @ (long/lat/alt): " + longitude.ToString() + " " + latitude.ToString() + " " + altitude.ToString());
            marker.transform.position = FlightGlobals.currentMainBody.GetWorldSurfacePosition(latitude, longitude, FlightGlobals.ActiveVessel.altitude);
            //marker.transform.rotation = Quaternion.FromToRotation(marker.transform.forward, FlightGlobals.ActiveVessel.terrainNormal);
            //marker.transform.forward = FlightGlobals.ActiveVessel.terrainNormal;

            Vector3 bottomPoint = FlightGlobals.currentMainBody.GetWorldSurfacePosition(latitude, longitude, 0);
            Vector3 topPoint = FlightGlobals.currentMainBody.GetWorldSurfacePosition(latitude, longitude, 1000);
            Vector3 cylinderDirection = topPoint - bottomPoint;

            marker.transform.up = cylinderDirection;

            marker.transform.localScale = new Vector3(markerSizeMax, 2000, markerSizeMax);
            sphereColorRed.a = maxAlpha;

        }

        public void showMarker()
        {
            Debug.Log("RS: showing marker");

            if (RoverScience.Instance.rover.scienceSpot.established)
            {
                marker.GetComponent<MeshRenderer>().enabled = true;
            }
        }

        public void hideMarker()
        {
            Debug.Log("RS: hiding marker");
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

                sphereColorRed.a = markerAlpha;
                sphereColorGreen.a = markerAlpha;

            }

            if ((distance <= 3) && (distance >= 0))
            {
                marker.GetComponent<MeshRenderer>().material.color = sphereColorGreen;
            } else {
                marker.GetComponent<MeshRenderer>().material.color = sphereColorRed;
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
