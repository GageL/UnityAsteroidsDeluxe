using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

namespace AsteroidsDeluxe {
	public class ObstacleActorMovement : MonoBehaviour {
		#region Public/Private Variables
		[SerializeField] private float driftSpeedMin;
		[SerializeField] private float driftSpeedMax;
		[SerializeField] private Transform childRotator;
		[SerializeField] private float driftTurnSpeedMin;
		[SerializeField] private float driftTurnSpeedMax;
		#endregion

		#region Runtime Variables
		[Foldout("Runtime Debug")]
		[SerializeField] private float driftSpeed;
		[Foldout("Runtime Debug")]
		[SerializeField] private float driftTurnSpeed;
		#endregion

		#region Native Methods
		private void Start() {
			Initialize();
		}

		private void Update() {
			DriftObstacle();
		}
		#endregion

		#region Callback Methods

		#endregion

		#region Static Methods

		#endregion

		#region Public Methods

		#endregion

		#region Private Methods
		private void Initialize() {
			driftSpeed = Random.Range(driftSpeedMin, driftSpeedMax);
			driftTurnSpeed = Random.Range(driftTurnSpeedMin, driftTurnSpeedMax);
			System.Random r = new System.Random();
			if (r.NextDouble() >= 0.5) {
				driftTurnSpeed *= -1;
			}
		}

		private void DriftObstacle() {
			this.transform.Translate(Vector3.up * driftSpeed * Time.deltaTime);
			if (childRotator) {
				childRotator.Rotate(Vector3.forward * driftTurnSpeed * Time.deltaTime);
			}
		}
		#endregion
	}
}