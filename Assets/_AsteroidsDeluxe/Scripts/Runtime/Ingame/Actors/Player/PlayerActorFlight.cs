using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

namespace AsteroidsDeluxe {
	public class PlayerActorFlight : MonoBehaviour {
		#region Public/Private Variables
		[SerializeField] private float moveSpeed;
		[SerializeField] private float moveAccelerationInterpolation;
		[SerializeField] private float moveDecelerationInterpolation;
		[SerializeField] private float turnSpeed;
		[SerializeField] private float turnAccelerationInterpolation;
		[SerializeField] private float turnDecelerationInterpolation;
		#endregion

		#region Runtime Variables
		[Foldout("Runtime Debug")]
		[SerializeField] private float moveAcceleration;
		[Foldout("Runtime Debug")]
		[SerializeField] private float turnAcceleration;
		private PlayerActor localPlayerActor;
		#endregion

		#region Native Methods
		private void Start() {
			localPlayerActor = GetComponent<PlayerActor>();
		}

		private void Update() {
			GetMoveAcceleration();
			MoveActor();
			GetTurnAcceleration();
			TurnActor();
		}
		#endregion

		#region Callback Methods

		#endregion

		#region Static Methods

		#endregion

		#region Public Methods

		#endregion

		#region Private Methods
		private void GetMoveAcceleration() {
			float _f = localPlayerActor.PlayerActorInput.ForwardInputDelta;
			if (_f >= 0.1f || _f <= -0.01f) {
				moveAcceleration = Mathf.Lerp(moveAcceleration, _f, moveAccelerationInterpolation * Time.deltaTime);
			} else {
				moveAcceleration = Mathf.Lerp(moveAcceleration, _f, moveDecelerationInterpolation * Time.deltaTime);
			}
		}

		private void MoveActor() {
			this.transform.Translate(Vector3.up * moveSpeed * moveAcceleration);
		}

		private void GetTurnAcceleration() {
			float _f = localPlayerActor.PlayerActorInput.RotateInputDelta;
			if (_f >= 0.1f || _f <= -0.01f) {
				turnAcceleration = Mathf.Lerp(turnAcceleration, _f, turnAccelerationInterpolation * Time.deltaTime);
			} else {
				turnAcceleration = Mathf.Lerp(turnAcceleration, _f, turnDecelerationInterpolation * Time.deltaTime);
			}
		}

		private void TurnActor() {
			this.transform.Rotate(Vector3.forward * turnSpeed * turnAcceleration);
		}
		#endregion
	}
}