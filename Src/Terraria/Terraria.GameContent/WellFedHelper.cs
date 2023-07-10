namespace GameManager.GameContent
{
	public struct WellFedHelper
	{
		private const int MAXIMUM_TIME_LEFT_PER_COUNTER = 72000;

		private int _timeLeftRank1;

		private int _timeLeftRank2;

		private int _timeLeftRank3;

		public int TimeLeft => _timeLeftRank1 + _timeLeftRank2 + _timeLeftRank3;

		public int Rank
		{
			get
			{
				if (_timeLeftRank3 > 0)
				{
					return 3;
				}
				if (_timeLeftRank2 > 0)
				{
					return 2;
				}
				if (_timeLeftRank1 > 0)
				{
					return 1;
				}
				return 0;
			}
		}

		public void Eat(int foodRank, int foodBuffTime)
		{
			int timeLeftToAdd = foodBuffTime;
			if (foodRank >= 3)
			{
				AddTimeTo(_timeLeftRank3, timeLeftToAdd, 72000);
			}
			if (foodRank >= 2)
			{
				AddTimeTo(_timeLeftRank2, timeLeftToAdd, 72000);
			}
			if (foodRank >= 1)
			{
				AddTimeTo(_timeLeftRank1, timeLeftToAdd, 72000);
			}
		}

		public void Update()
		{
			ReduceTimeLeft();
		}

		public void Clear()
		{
			_timeLeftRank1 = 0;
			_timeLeftRank2 = 0;
			_timeLeftRank3 = 0;
		}

		private void AddTimeTo(int foodTimeCounter, int timeLeftToAdd, int counterMaximumTime)
		{
			if (timeLeftToAdd != 0)
			{
				int num = timeLeftToAdd;
				if (foodTimeCounter + num > counterMaximumTime)
				{
					num = counterMaximumTime - foodTimeCounter;
				}
				foodTimeCounter += num;
				timeLeftToAdd -= num;
			}
		}

		private void ReduceTimeLeft()
		{
			if (_timeLeftRank3 > 0)
			{
				_timeLeftRank3--;
			}
			else if (_timeLeftRank2 > 0)
			{
				_timeLeftRank2--;
			}
			else if (_timeLeftRank1 > 0)
			{
				_timeLeftRank1--;
			}
		}
	}
}
