using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

namespace AsteroidsDeluxe {
	public class ActorCombat : MonoBehaviour {
		#region Public/Private Variables
		[Serializable]
		public class WeaponSystem {
			public ProjectileActor ProjectilePrefab;
			public Transform ShootPoint;
			public float FireRate;
		}
		[SerializeField] protected WeaponSystem[] weaponSystems;
		#endregion

		#region Runtime Variables
		[Foldout("Runtime Debug")]
		[SerializeField] protected bool canFire;
		protected Actor localActor;
		#endregion

		#region Native Methods
		protected virtual void Start() {
			localActor = GetComponent<Actor>();
		}
		#endregion

		#region Callback Methods

		#endregion

		#region Static Methods

		#endregion

		#region Public Methods
		public void SetFire(bool value) {
			canFire = value;
		}
		#endregion

		#region Private Methods
		private IEnumerator FireProjectileProcess(float fireRate) {
			yield return new WaitForSeconds(fireRate);
			canFire = true;
		}

		protected void FireProjectiles() {
			if (canFire) {
				canFire = false;
				var _longestWeaponSystem = weaponSystems.OrderBy(x => x.FireRate).ToArray()[0];
				foreach (WeaponSystem weaponSystem in weaponSystems) {
					ProjectileActor _projectile = Instantiate(weaponSystem.ProjectilePrefab, weaponSystem.ShootPoint.position, weaponSystem.ShootPoint.rotation, GameStateManager.Instance.RuntimeFolder);
					_projectile.Engage(localActor);
				}
				StartCoroutine(FireProjectileProcess(_longestWeaponSystem.FireRate));
			}
		}
		#endregion
	}
}