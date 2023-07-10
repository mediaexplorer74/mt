namespace GameManager.GameContent.LootSimulation
{
	public interface ISimulationConditionSetter
	{
		int GetTimesToRunMultiplier(SimulatorInfo info);

		void Setup(SimulatorInfo info);

		void TearDown(SimulatorInfo info);
	}
}
