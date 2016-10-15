using System;
namespace RoverScience
{
	public class Upgrades
	{
		Upgrade maxDistanceScan;
		Upgrade predictionAccuracy;
		Upgrade analyzeRepeatEffect;

		public Upgrades()
		{
			
			maxDistanceScan = new Upgrade(1, 5);
			predictionAccuracy = new Upgrade(1, 5);
			analyzeRepeatEffect = new Upgrade(1, 5);
		}

	}

	public class Upgrade
	{
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

		public Upgrade(int _level, int _max)
		{
			level = _level;
			max = _max;
		}

		public bool AddLevel(int _add)
		{
			level += _add;
			return true;
		}
	}

}
