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
        private GameObject sphere = null;
        int sphereSize = 50;
        public static DrawWaypoint Instance = null;

        private void Start()
        {
            Debug.Log("Attempting to create sphere");
            Instance = this;

            sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            Destroy(sphere.GetComponent("SphereCollider")); // Remove collider from sphere

            // Set initial position
            sphere.transform.localScale = new Vector3(sphereSize, sphereSize, sphereSize);
            sphere.transform.position = FlightGlobals.currentMainBody.GetWorldSurfacePosition(0, 0, FlightGlobals.ActiveVessel.altitude);
            hideMarker(); // do not render marker yet

            // Set sphere material, color and alpha
            sphere.GetComponent<MeshRenderer>().material = new Material(Shader.Find("Transparent/Diffuse"));
            Color sphereColor = Color.red;
            sphereColor.a = 0.7f;
            sphere.GetComponent<MeshRenderer>().material.color = sphereColor;
            Debug.Log("Reached end of sphere creation");
        }

        public void setMarkerLocation(double longitude, double latitude, double altitude)
        {
            Debug.Log("Drawing sphere @ (long/lat/alt): " + longitude.ToString() + " " + latitude.ToString() + " " + altitude.ToString());
            sphere.transform.position = FlightGlobals.currentMainBody.GetWorldSurfacePosition(latitude, longitude, altitude);
        }

        public void showMarker()
        {
            Debug.Log("RS: showing marker");
            sphere.GetComponent<MeshRenderer>().enabled = true;
        }

        public void hideMarker()
        {
            Debug.Log("RS: hiding marker");
            sphere.GetComponent<MeshRenderer>().enabled = false;
        }


        private void OnDestroy()
        {
            Destroy(sphere);
        }


    }
}
