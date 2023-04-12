using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AsteroidsDeluxe {
	[Serializable]
	public class ActorWaveData {
		#region Public/Private Variables
		public int SpawnAmount;
		public List<Actor> Actors = new List<Actor>();
		public float SpawnRateMin;
		public float SpawnRateMax;
		#endregion

		#region Runtime Variables

		#endregion

		#region Native Methods

		#endregion

		#region Callback Methods

		#endregion

		#region Static Methods

		#endregion

		#region Public Methods
		public float GetSpawnRate() {
			return UnityEngine.Random.Range(SpawnRateMin, SpawnRateMax);
		}
		#endregion

		#region Private Methods

		#endregion
	}
}