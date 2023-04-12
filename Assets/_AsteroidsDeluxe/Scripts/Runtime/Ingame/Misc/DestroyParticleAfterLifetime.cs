using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AsteroidsDeluxe {
	[RequireComponent(typeof(ParticleSystem))]
	public class DestroyParticleAfterLifetime : MonoBehaviour {
		#region Public/Private Variables

		#endregion

		#region Runtime Variables

		#endregion

		#region Native Methods
		private void Start() {
			if (!this.GetComponent<ParticleSystem>().main.loop) {
				Destroy(this.gameObject, this.GetComponent<ParticleSystem>().main.duration);
			}
		}
		#endregion

		#region Callback Methods

		#endregion

		#region Static Methods

		#endregion

		#region Public Methods

		#endregion

		#region Private Methods

		#endregion
	}
}