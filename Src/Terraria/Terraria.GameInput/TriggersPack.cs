using System.Linq;

namespace GameManager.GameInput
{
	public class TriggersPack
	{
		public TriggersSet Current = new TriggersSet();

		public TriggersSet Old = new TriggersSet();

		public TriggersSet JustPressed = new TriggersSet();

		public TriggersSet JustReleased = new TriggersSet();

		public void Initialize()
		{
			Current.SetupKeys();
			Old.SetupKeys();
			JustPressed.SetupKeys();
			JustReleased.SetupKeys();
		}

		public void Reset()
		{
			Old = Current.Clone();
			Current.Reset();
		}

		public void Update()
		{
			CompareDiffs(JustPressed, Old, Current);
			CompareDiffs(JustReleased, Current, Old);
		}

		public void CompareDiffs(TriggersSet Bearer, TriggersSet oldset, TriggersSet newset)
		{
			Bearer.Reset();
			foreach (string item in Bearer.KeyStatus.Keys.ToList())
			{
				Bearer.KeyStatus[item] = newset.KeyStatus[item] && !oldset.KeyStatus[item];
			}
		}
	}
}
