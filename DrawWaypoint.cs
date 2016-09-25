using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using KSP;

namespace RoverScience
{
    // All code taken from Waypoint Manager mod
    // Still in experimental as I try to figure out what I'm doing here
    [KSPAddon(KSPAddon.Startup.Flight, false)]
    public class DrawWaypoint : MonoBehaviour
    {
        System.Random rand = new System.Random();

        private GameObject marker = null;
        private GameObject interestingObject;

        float markerSize = 30;
        float markerSizeMax = 30;

        float markerAlpha = 0.4f;
        float maxAlpha = 0.4f;
        float minAlpha = 0.05f;

        public static DrawWaypoint Instance = null;
        Color markerRed= Color.red;
        Color markerGreen = Color.green;

        string[] rockObjectNames = {"rock", "rock2"};

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

            markerRed.a = markerAlpha; // max alpha
            markerGreen.a = markerAlpha; // max alpha

            marker.GetComponent<MeshRenderer>().material.color = markerRed; // set to red on awake
            Debug.Log("Reached end of marker creation");
        }

        public void DestroyInterestingObject()
        {
            if (interestingObject != null) Destroy(interestingObject);
        }

        public void setMarkerLocation(double longitude, double latitude, bool spawningObject = true)
        {
            DestroyInterestingObject();

            Vector3 bottomPoint = FlightGlobals.currentMainBody.GetWorldSurfacePosition(latitude, longitude, 0);
            Vector3 topPoint = FlightGlobals.currentMainBody.GetWorldSurfacePosition(latitude, longitude, 1000);

            double surfaceAltitude = getSurfaceAltitude(longitude, latitude);
            Debug.Log("Drawing marker @ (long/lat/alt): " + longitude.ToString() + " " + latitude.ToString() + " " + surfaceAltitude.ToString());
            marker.transform.position = FlightGlobals.currentMainBody.GetWorldSurfacePosition(latitude, longitude, surfaceAltitude);

            //marker.transform.up = cylinderDirectionUp;

            marker.transform.localScale = new Vector3(markerSizeMax, markerSizeMax, markerSizeMax);
            markerRed.a = maxAlpha;

            //attempt to get raycast surface altitude

            if (spawningObject) spawnObject(longitude, latitude);
        }

        private Vector3 getUpDown(double longitude, double latitude, bool up = true)
        {
            double altitude = 20000;

            Vector3d topPoint = FlightGlobals.currentMainBody.GetWorldSurfacePosition(latitude, longitude, altitude);
            Vector3d bottomPoint = FlightGlobals.currentMainBody.GetWorldSurfacePosition(latitude, longitude, -altitude);

            if (up)
            {
                return topPoint - bottomPoint;
            } else {
                return bottomPoint - topPoint;
            }

        }

        public double getSurfaceAltitude(double longitude, double latitude)
        {
            double altitude = 20000;
            RaycastHit hit;
            Vector3d topPoint = FlightGlobals.currentMainBody.GetWorldSurfacePosition(latitude, longitude, altitude);
            Vector3d bottomPoint = FlightGlobals.currentMainBody.GetWorldSurfacePosition(latitude, longitude, -altitude);
            

            if (Physics.Raycast(topPoint, (bottomPoint - topPoint), out hit, Mathf.Infinity, 1 << 15))
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

            float distanceToRover = (float)rover.distanceFromScienceSpot;

            // distance to rover 10
            // min distance 3
            // +2 = 5
            // will keep reducing size as long as distance is over 5
            if ((distanceToRover < markerSizeMax) && ((distanceToRover > (rover.scienceSpot.minDistance+4))))
            {
                // Reduce sphere size with proximity
                markerSize = distanceToRover;
                marker.transform.localScale = new Vector3(markerSize, markerSize, markerSize);

                // Reduce alpha with proximity
                markerAlpha = (float)(distanceToRover / markerSizeMax);
                if (markerAlpha >= maxAlpha)
                {
                    markerAlpha = maxAlpha;
                } else if (markerAlpha <= minAlpha)
                {
                    markerAlpha = minAlpha;
                }

                markerRed.a = markerAlpha;
                markerGreen.a = markerAlpha;

            }



            if ((distanceToRover <= (rover.scienceSpot.minDistance)) && (distanceToRover >= 0))
            {
                marker.GetComponent<MeshRenderer>().material.color = markerGreen;
            } else {
                marker.GetComponent<MeshRenderer>().material.color = markerRed;
            }

            //Debug.Log("dist, dist/50, alpha: [" + distance + " / " + distance / 50 + " / " + markerAlpha + "]");
        }


        public void spawnObject(double longitude, double latitude)
        {
            try
            {
                Debug.Log("RSR: Attempting to spawn object");
                string randomRockName = rockObjectNames[rand.Next(rockObjectNames.Length)];
                GameObject test = GameDatabase.Instance.GetModel("RoverScience/rock/" + randomRockName);
                Debug.Log("Random rock name: " + randomRockName);
                test.SetActive(true);

                interestingObject = GameObject.Instantiate(test) as GameObject;
                GameObject.Destroy(test);

                GameObject.Destroy(interestingObject.GetComponent("MeshCollider"));
                double srfAlt = DrawWaypoint.Instance.getSurfaceAltitude(longitude, latitude);
                interestingObject.transform.position = FlightGlobals.currentMainBody.GetWorldSurfacePosition(latitude, longitude, srfAlt);
                interestingObject.transform.up = getUpDown(longitude, latitude, true);
            } catch
            {
                Debug.Log("rock model couldn't be found");
            }
        }
        
        private void Update()
        {
            if (marker.GetComponent<MeshRenderer>().enabled)
            {
                changeSpherewithDistance(RoverScience.Instance.rover);
            }
        }

        private void OnDestroy()
        {
            Destroy(marker);
            DestroyInterestingObject();
        }


    }
}
