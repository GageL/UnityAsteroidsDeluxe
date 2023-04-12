using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AsteroidsDeluxe {
	public class AsteroidObstacle : ObstacleActor {
		#region Public/Private Variables
		[SerializeField] private AsteroidObstacle[] hitFractures;
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
		public override void ProjectileHit(Actor owner) {
			base.ProjectileHit(owner);
			if (owner is PlayerActor) {
				GameStateManager.Instance.AddScore(scoreWorth);
			}
			foreach (AsteroidObstacle fracture in hitFractures) {
				ActorWaveManager.Instance.AddAdditionalWaveActor(this.transform.position, fracture);
			}
		}
		#endregion

		#region Private Methods

		#endregion
	}
}