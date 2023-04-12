using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AsteroidsDeluxe {
	public class RendererVisibilityPositionSolver : MonoBehaviour {
		#region Public/Private Variables
		[SerializeField] private Transform solverTarget;
		#endregion

		#region Runtime Variables

		#endregion

		#region Native Methods
		private void OnBecameInvisible() {
			if (!solverTarget) { return; }
			Vector3 _position = solverTarget.position;
			_position.x *= -1;
			_position.y *= -1;
			solverTarget.position = _position;
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