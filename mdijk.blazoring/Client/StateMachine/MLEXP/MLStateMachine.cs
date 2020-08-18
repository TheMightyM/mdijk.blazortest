using mdijk.blazoring.Shared.ML;
using mdijk.blazoring.Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

//MLEXP staat voor MACHINE LEARNING EXPERIMENT!
namespace mdijk.blazoring.Client.StateMachine.MLEXP
{
	/// <summary>
	/// Dit is niet zo zeer alleen een statemachine, maar ook een onthouder van data!
	/// dus totaal geen geheugenvreter ofzo!
	/// </summary>
	public class MLStateMachine
	{
		public enum MachineStates
		{
			Initial,
			SelectSeason,
			SelectRace,
			SelectDrivers,
			ShowResults
		}

		public MachineStates CurrentState { get; set; }

		public Season SelectedSeason { get; set; }
		public int SelectedRaceEventId { get; set; }
		public List<int> SelectedDriversIds { get; set; }
		public MLDriverCircuitRequest DriverCircuitRequest { get; set;}
		public MLDriverCircuitResponse DriverCircuitResponse { get; set; }

		public IDictionary<int, Driver> SeasonsDrivers { get;set; }

		public void NextState()
		{
			switch(CurrentState)
			{
				case MachineStates.Initial:
					CurrentState = MachineStates.SelectSeason;
					break;

				case MachineStates.SelectSeason:
					CurrentState = MachineStates.SelectRace;
					break;

				case MachineStates.SelectRace:
					CurrentState = MachineStates.SelectDrivers;
					break;

				case MachineStates.SelectDrivers:
					CurrentState = MachineStates.ShowResults;
					break;
			}
		}
	}
}
