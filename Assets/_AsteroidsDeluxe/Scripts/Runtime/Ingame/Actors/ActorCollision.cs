using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AsteroidsDeluxe {
	[RequireComponent(typeof(Rigidbody2D))]
	[RequireComponent(typeof(CircleCollider2D))]
	public class ActorCollision : MonoBehaviour {
		#region Public/Private Variables
		public Action<Collision2D> OnCollision;
		#endregion

		#region Runtime Variables
		protected Actor localActor;
		#endregion

		#region Native Methods
		private void Start() {
			localActor = GetComponent<Actor>();
		}
		#endregion

		#region Callback Methods

		#endregion

		#region Static Methods

		#endregion

		#region Public Methods
		public void OnCollisionEnter2D(Collision2D collision) {
			OnCollision?.Invoke(collision);
		}
		#endregion

		#region Private Methods

		#endregion
	}
}