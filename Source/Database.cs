using System;

namespace RoverScience
{
	public static class Database
	{
        // main database to be accessed from everywhere
        // this class will contain upgrades and settings and other information
        
        public static Upgrades upgrades = new Upgrades();
        public static Anomalies anomalies = new Anomalies();
	}
}
