using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AsteroidsDeluxe {
	public class PlayerActorCombat : ActorCombat {
		#region Public/Private Variables

		#endregion

		#region Runtime Variables
		private PlayerActor localPlayerActor;
		#endregion

		#region Native Methods
		protected override void Start() {
			base.Start();
			localPlayerActor = GetComponent<PlayerActor>();
			localPlayerActor.PlayerActorInput.OnFire += OnFire;
		}

		private void OnDisable() {
			localPlayerActor.PlayerActorInput.OnFire -= OnFire;
		}
		#endregion

		#region Callback Methods
		private void OnFire() {
			FireProjectiles();
		}
		#endregion

		#region Static Methods

		#endregion

		#region Public Methods

		#endregion

		#region Private Methods

		#endregion
	}
}