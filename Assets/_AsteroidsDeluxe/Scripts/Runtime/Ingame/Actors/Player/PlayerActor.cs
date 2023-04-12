using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AsteroidsDeluxe {
	[RequireComponent(typeof(PlayerActorInput))]
	[RequireComponent(typeof(PlayerActorCombat))]
	[RequireComponent(typeof(PlayerActorFlight))]
	[RequireComponent(typeof(ActorCollision))]
	public class PlayerActor : Actor {
		#region Public/Private Variables

		#endregion

		#region Runtime Variables
		public PlayerActorInput PlayerActorInput { get { return playerActorInput; } }
		private PlayerActorInput playerActorInput;
		public PlayerActorCombat PlayerActorCombat { get { return playerActorCombat; } }
		private PlayerActorCombat playerActorCombat;
		public PlayerActorFlight PlayerActorFlight { get { return playerActorFlight; } }
		private PlayerActorFlight playerActorFlight;
		public ActorCollision ActorCollision { get { return actorCollision; } }
		private ActorCollision actorCollision;
		#endregion

		#region Native Methods
		protected override void Awake() {
			base.Awake();
			playerActorInput = GetComponent<PlayerActorInput>();
			playerActorCombat = GetComponent<PlayerActorCombat>();
			playerActorFlight = GetComponent<PlayerActorFlight>();
			actorCollision = GetComponent<ActorCollision>();
		}

		protected override void Start() {
			base.Start();
			actorCollision.OnCollision += OnActorCollision;
		}

		private void OnDestroy() {
			actorCollision.OnCollision -= OnActorCollision;
		}
		#endregion

		#region Callback Methods
		private void OnActorCollision(Collision2D collision) {
			if (!canBeHit) { return; }
			if (collision.collider.gameObject.GetComponent<EnemyActor>() || collision.collider.gameObject.GetComponent<ObstacleActor>()) {
				Instantiate(destroyEffect, transform.position, Quaternion.identity, GameStateManager.Instance.RuntimeFolder);
				AudioManager.Instance.PlayOneShot(destroyClip);
				GameStateManager.Instance.KillPlayer();
			}
		}
		#endregion

		#region Static Methods

		#endregion

		#region Public Methods
		public override void ProjectileHit(Actor owner) {
			base.ProjectileHit(owner);
			GameStateManager.Instance.KillPlayer();
		}
		#endregion

		#region Private Methods

		#endregion
	}
}