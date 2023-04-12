using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AsteroidsDeluxe {
	[RequireComponent(typeof(CircleCollider2D))]
	public class EnemyActor : Actor {
		#region Public/Private Variables

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
		}
		#endregion

		#region Private Methods

		#endregion
	}
}