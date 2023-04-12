using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

namespace AsteroidsDeluxe {
	[RequireComponent(typeof(ActorCollision))]
	public class ProjectileActor : Actor {
		#region Public/Private Variables
		[SerializeField] private float aliveTime;
		[SerializeField] private float moveSpeed;
		#endregion

		#region Runtime Variables
		public Actor Owner { get { return owner; } }
		[Foldout("Runtime Debug")]
		[SerializeField] private Actor owner;
		public ActorCollision ActorCollision { get { return actorCollision; } }
		private ActorCollision actorCollision;
		#endregion

		#region Native Methods
		protected override void Awake() {
			base.Awake();
			actorCollision = GetComponent<ActorCollision>();
		}

		protected override void Start() {
			base.Start();
			actorCollision.OnCollision += OnActorCollision;
		}

		private void Update() {
			MoveActor();
		}

		private void OnDestroy() {
			actorCollision.OnCollision -= OnActorCollision;
		}
		#endregion

		#region Callback Methods
		private void OnActorCollision(Collision2D collision) {
			if (collision.collider.gameObject.GetComponent<Actor>() && !collision.collider.gameObject.GetComponent<ProjectileActor>()) {
				if (collision.collider.gameObject.GetComponent<Actor>() != owner) {
					collision.collider.gameObject.GetComponent<Actor>().ProjectileHit(owner);
					Destroy(this.gameObject);
				}
			}
		}
		#endregion

		#region Static Methods

		#endregion

		#region Public Methods
		public void Engage(Actor owner) {
			this.owner = owner;
			Destroy(this.gameObject, aliveTime);
		}
		#endregion

		#region Private Methods
		private void MoveActor() {
			transform.Translate(Vector3.up * moveSpeed * Time.deltaTime);
		}
		#endregion
	}
}