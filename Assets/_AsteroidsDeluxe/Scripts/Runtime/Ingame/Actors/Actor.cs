using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

namespace AsteroidsDeluxe {
	public class Actor : MonoBehaviour {
		#region Public/Private Variables
		[SerializeField] protected int scoreWorth;
		[SerializeField] private AudioClip spawnSound;
		[SerializeField] protected AudioClip destroyClip;
		[SerializeField] protected GameObject destroyEffect;
		[SerializeField] protected float spawnInvulnerableTime;
		#endregion

		#region Runtime Variables
		[Foldout("Runtime Debug")]
		[SerializeField] protected bool canBeHit;
		[Foldout("Runtime Debug")]
		[SerializeField] protected bool isAdditionalWaveActor;
		#endregion

		#region Native Methods
		protected virtual void Awake() { }
		protected virtual void Start() {
			AudioManager.Instance.PlayOneShot(spawnSound);
			StartCoroutine(SpawnInvulnerableProcess());
		}
		#endregion

		#region Callback Methods

		#endregion

		#region Static Methods

		#endregion

		#region Public Methods
		public void SetIsAdditionalWaveActor() {
			isAdditionalWaveActor = true;
		}

		public virtual void ProjectileHit(Actor owner) {
			if (!canBeHit) { return; }
			AudioManager.Instance.PlayOneShot(destroyClip);
			Instantiate(destroyEffect, transform.position, Quaternion.identity, GameStateManager.Instance.RuntimeFolder);
			if (isAdditionalWaveActor) {
				ActorWaveManager.Instance.DestroyAdditionalWaveActor(this);
			} else {
				ActorWaveManager.Instance.DestroyWaveActor(this);
			}
			Destroy(this.gameObject);
		}
		#endregion

		#region Private Methods
		protected IEnumerator SpawnInvulnerableProcess() {
			yield return new WaitForSeconds(spawnInvulnerableTime);
			canBeHit = true;
		}
		#endregion
	}
}