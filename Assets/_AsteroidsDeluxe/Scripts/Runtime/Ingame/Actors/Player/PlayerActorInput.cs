using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

namespace AsteroidsDeluxe {
	public class PlayerActorInput : MonoBehaviour {
		#region Public/Private Variables
		public Action OnFire;
		[SerializeField] private float inputInterpolationSpeed;
		#endregion

		#region Runtime Variables
		public bool CanControl { get { return canControl; } }
		public float ForwardInputDelta { get { return forwardInputDelta; } }
		public float RotateInputDelta { get { return rotateInputDelta; } }
		[Foldout("Runtime Debug")]
		[SerializeField] private bool canControl;
		[Foldout("Runtime Debug")]
		[SerializeField] private float forwardInputDelta;
		[Foldout("Runtime Debug")]
		[SerializeField] private float rotateInputDelta;
		private PlayerActor localPlayerActor;
		#endregion

		#region Native Methods
		private void Start() {
			localPlayerActor = GetComponent<PlayerActor>();
		}

		private void Update() {
			GetForwardInput();
			GetRotateInput();
			GetFireInput();
		}
		#endregion

		#region Callback Methods

		#endregion

		#region Static Methods

		#endregion

		#region Public Methods
		public void SetControl(bool value) {
			canControl = value;
		}
		#endregion

		#region Private Methods
		private void GetForwardInput() {
			if ((Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow)) && canControl) {
				forwardInputDelta = Mathf.Lerp(forwardInputDelta, 1f, inputInterpolationSpeed * Time.deltaTime);
			} else if ((Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow)) && canControl) {
				forwardInputDelta = Mathf.Lerp(forwardInputDelta, -1f, inputInterpolationSpeed * Time.deltaTime);
			} else {
				forwardInputDelta = Mathf.Lerp(forwardInputDelta, 0, inputInterpolationSpeed * Time.deltaTime);
			}
		}

		private void GetRotateInput() {
			if ((Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow)) && canControl) {
				rotateInputDelta = Mathf.Lerp(rotateInputDelta, 1f, inputInterpolationSpeed * Time.deltaTime);
			} else if ((Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow)) && canControl) {
				rotateInputDelta = Mathf.Lerp(rotateInputDelta, -1f, inputInterpolationSpeed * Time.deltaTime);
			} else {
				rotateInputDelta = Mathf.Lerp(rotateInputDelta, 0, inputInterpolationSpeed * Time.deltaTime);
			}
		}

		private void GetFireInput() {
			if ((Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.RightControl)) && canControl) {
				OnFire?.Invoke();
			}
		}
		#endregion
	}
}