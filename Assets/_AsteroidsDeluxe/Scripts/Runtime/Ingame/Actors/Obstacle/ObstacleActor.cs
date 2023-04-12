using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AsteroidsDeluxe {
	[RequireComponent(typeof(ObstacleActorMovement))]
	[RequireComponent(typeof(CircleCollider2D))]
	public class ObstacleActor : Actor {
		#region Public/Private Variables

		#endregion

		#region Runtime Variables
		public ObstacleActorMovement ObstacleActorMovement { get { return obstacleActorMovement; } }
		private ObstacleActorMovement obstacleActorMovement;
		#endregion

		#region Native Methods
		protected override void Awake() {
			base.Awake();
			obstacleActorMovement = GetComponent<ObstacleActorMovement>();
		}
		#endregion

		#region Callback Methods

		#endregion

		#region Static Methods

		#endregion

		#region Public Methods
		public override void ProjectileHit(Actor owner) {
			base.ProjectileHit(owner);
		}
		#endregion

		#region Private Methods

		#endregion
	}
}