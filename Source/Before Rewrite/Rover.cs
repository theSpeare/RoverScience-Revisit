using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace RoverScience
{

	public class COORDS
	{
		public double latitude;
		public double longitude;
	}


	// Much of the coordinate work with latitude/longitude in this source is only functional with the work here:
	// http://www.movable-type.co.uk/scripts/latlong.html

	public class Rover
	{

		public System.Random rand = new System.Random();

		public ScienceSpot scienceSpot;
		public LandingSpot landingSpot;

		public COORDS location = new COORDS ();
		public double distanceTraveled = 0;
		public double distanceCheck = 20;
		public double distanceTraveledTotal = 0;

		public int minRadius = 40;
		public int maxRadius = 100;

        public List<string> anomaliesAnalyzed = new List<string>();

		public double distanceFromLandingSpot
		{
			get{
				return getDistanceBetweenTwoPoints (location, landingSpot.location);
			}
		}

		public double distanceFromScienceSpot
		{
			get{
                return getDistanceBetweenTwoPoints(location, scienceSpot.location);
			}
		}

		public double bearingToScienceSpot
		{
			get {
                return getBearingFromCoords(scienceSpot.location);
			}
		}   

		Vessel vessel
		{
			get{
				return FlightGlobals.ActiveVessel;
			}
		}

        RoverScience roverScience
        {
            get
            {
                return RoverScience.Instance;
            }
        }

		public double heading
		{
			get{
				return getRoverHeading ();
			}
		}

		public bool scienceSpotReached
		{
			get {
				if (scienceSpot.established) {
					if (distanceFromScienceSpot <= scienceSpot.minDistance) {
						return true;
					}
				}
				return false;
			}
		}

        public bool anomalySpotReached
        {
            get
            {
                if (scienceSpot.established)
                {
                    if (distanceToClosestAnomaly <= scienceSpot.minDistance)
                    {
                        return true;
                    }
                }
                return false;
            }
        }

        public bool anomalyPresent
        {
            get
            {
                return ((distanceToClosestAnomaly <= 100) && !Anomalies.Instance.hasCurrentAnomalyBeenAnalyzed());
            }
        }

        public int numberWheelsLanded
		{
			get
			{
				return getWheelsLanded();
			}
		}

        public int numberWheels
        {
            get
            {
                return getWheelCount();
            }
        }

        public bool validStatus
        {
            get
            {
                return checkRoverValidStatus();
            }
        }

        public double distanceToClosestAnomaly
        {
            get
            {
                return getDistanceBetweenTwoPoints(location, closestAnomaly.location);
            }
        }

        public void calculateDistanceTraveled(double deltaTime)
		{
			distanceTraveled += (roverScience.vessel.srfSpeed) * deltaTime;
            if (!scienceSpot.established) distanceTraveledTotal += (roverScience.vessel.srfSpeed) * deltaTime;
		}

        public void setRoverLocation()
        {
            location.latitude = vessel.latitude;
            location.longitude = vessel.longitude;
        }

		public double getDistanceBetweenTwoPoints(COORDS _from, COORDS _to)
		{
            
            double bodyRadius = vessel.mainBody.Radius;
			double dLat = (_to.latitude - _from.latitude).ToRadians();
			double dLon = (_to.longitude - _from.longitude).ToRadians();
			double lat1 = _from.latitude.ToRadians();
			double lat2 = _to.latitude.ToRadians();

			double a = Math.Sin(dLat / 2) * Math.Sin(dLat / 2) +
				Math.Sin(dLon / 2) * Math.Sin(dLon / 2) * Math.Cos(lat1) * Math.Cos(lat2);
			double c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
			double d = bodyRadius * c;

			return Math.Round(d, 4);
		}


		public double getBearingFromCoords(COORDS target)
		{
			// Rover x,y position

			double dLat = (target.latitude - location.latitude).ToRadians();
			double dLon = (target.longitude - location.longitude).ToRadians();
			double lat1 = location.latitude.ToRadians();
			double lat2 = target.latitude.ToRadians();

			double y = Math.Sin(dLon) * Math.Cos(lat2);
			double x = Math.Cos(lat1) * Math.Sin(lat2) -
				Math.Sin(lat1) * Math.Cos(lat2) * Math.Cos(dLon);

			double bearing = Math.Atan2(y, x).ToDegrees();
			//bearing = (bearing + 180) % 360;

			//return bearing % 360;
			return (bearing + 360) % 360;
		}

		public void resetDistanceTraveled()
		{
			distanceTraveled = 0;
		}

        private bool checkRoverValidStatus()
        {   
            // Checks if rover is landed with at least one wheel on with no time-warp.
            return ((TimeWarp.CurrentRate == 1) && (vessel.horizontalSrfSpeed > (double)0.01) && (numberWheelsLanded > 0));
        }

        private double getRoverHeading()
		{
            //Vector3d coM = vessel.findLocalCenterOfMass();
            Vector3d coM = vessel.localCoM;
            Vector3d up = (coM - vessel.mainBody.position).normalized;
			Vector3d north = Vector3d.Exclude(up, (vessel.mainBody.position + 
				(Vector3d)vessel.mainBody.transform.up * vessel.mainBody.Radius) - coM).normalized;

			Quaternion rotationSurface = Quaternion.LookRotation(north, up);
			Quaternion rotationVesselSurface = Quaternion.Inverse(Quaternion.Euler(90, 0, 0) * Quaternion.Inverse(vessel.GetTransform().rotation) * rotationSurface);
			return rotationVesselSurface.eulerAngles.y;
		}



        private int getWheelCount()
		{
			int wheelCount = 0;

			List<Part> vesselParts = FlightGlobals.ActiveVessel.Parts;

			foreach (Part part in vesselParts) {
				foreach (PartModule module in part.Modules) {
					if (module.moduleName == "ModuleWheelBase") {
						wheelCount++;

					}
				}
			}
			return wheelCount;
		}


		private int getWheelsLanded()
		{
			int count = 0;
			List<Part> vesselParts = FlightGlobals.ActiveVessel.Parts; 
			foreach (Part part in vesselParts) {
				foreach (PartModule module in part.Modules) {
                    if ((module.moduleName.IndexOf("wheel", StringComparison.OrdinalIgnoreCase) >= 0)) {
                        if (part.GroundContact)
                            {
                                count++;
                            }
                        }
					}
			}
			return count;
		}

        List<Anomalies.Anomaly> anomaliesList = new List<Anomalies.Anomaly>();
        public Anomalies.Anomaly closestAnomaly = new Anomalies.Anomaly();

        public void setClosestAnomaly(string bodyName)
        {
            // this is run on establishing landing spot (to avoid expensive constant foreach loops

            setRoverLocation(); // (update rover location)
            double distanceClosest = 0;
            double distanceCheck = 0;

            if (Anomalies.Instance.hasAnomalies(bodyName))
            {
                anomaliesList = Anomalies.Instance.getAnomalies(bodyName);

                closestAnomaly = anomaliesList[0]; // set initial

                // check and find closest anomaly
                int i = 0;
                foreach (Anomalies.Anomaly anomaly in anomaliesList)
                {
                    distanceClosest = getDistanceBetweenTwoPoints(location, closestAnomaly.location);
                    distanceCheck = getDistanceBetweenTwoPoints(location, anomaly.location);

                    //Debug.Log("========" + i + "========");
                    //Debug.Log("distanceClosest: " + distanceClosest);
                    //Debug.Log("distanceCheck: " + distanceCheck);

                    //Debug.Log("Current lat/long: " + location.latitude + "/" + location.longitude);
                    //Debug.Log("Closest Anomaly lat/long: " + closestAnomaly.location.latitude + "/" + closestAnomaly.location.longitude);
                    //Debug.Log("Check Anomaly lat/long: " + anomaly.location.latitude + "/" + anomaly.location.longitude);

                    //Debug.Log("==========<END>==========");


                    if (distanceCheck < distanceClosest)
                    {
                        closestAnomaly = anomaly;
                    }
                    i++;
                }

                distanceClosest = getDistanceBetweenTwoPoints(location, closestAnomaly.location);
                Debug.Log("======= RS: closest anomaly details =======");
                Debug.Log("long/lat: " + closestAnomaly.location.longitude + "/" + closestAnomaly.location.latitude);
                Debug.Log("instantaneous distance: " + getDistanceBetweenTwoPoints(location, closestAnomaly.location));
                Debug.Log("id: " + closestAnomaly.id);
                Debug.Log("name: " + closestAnomaly.name);
                Debug.Log("=== RS: closest anomaly details <<END>>====");
            }
        }

    }


	public static class NumericExtensions
	{
		public static double ToRadians(this double val)
		{
			return (Math.PI / 180) * val;
		}

		public static double ToDegrees(this double val)
		{
			return (180 / Math.PI) * val;
		}
	}

}

