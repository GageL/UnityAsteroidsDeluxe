using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

namespace AsteroidsDeluxe {
	public class ActorWaveManager : MonoBehaviour {
		#region Public/Private Variables
		public static ActorWaveManager Instance { get; private set; }
		[SerializeField] private ActorWaveData[] finiteWaves;
		[SerializeField] private ActorWaveData infiniteWaveBase;
		[SerializeField] private int infiniteWaveDivisor;
		[SerializeField] private int startSpawnRateThreshold;
		[SerializeField] private float startSpawnRateOverrideMin;
		[SerializeField] private float startSpawnRateOverrideMax;
		[SerializeField] private float nextWaveDelay;
		[SerializeField] private AudioClip waveClearedClip;
		#endregion

		#region Runtime Variables
		[Foldout("Runtime Debug")]
		[SerializeField] private bool infiniteMode;
		[Foldout("Runtime Debug")]
		[SerializeField] private int currentInfiniteWaveMultiplier;
		[Foldout("Runtime Debug")]
		[SerializeField] private int currentWaveIdx;
		[Foldout("Runtime Debug")]
		[SerializeField] private int spawnedWaveActors;
		[Foldout("Runtime Debug")]
		[SerializeField] private List<Actor> waveActors = new List<Actor>();
		[Foldout("Runtime Debug")]
		[SerializeField] private List<Actor> additionalWaveActors = new List<Actor>();
		private Coroutine waveTimerProcess;
		private Coroutine spawnWaveProcess;
		#endregion

		#region Native Methods
		private void Awake() {
			Instance = this;
		}

		private void Start() {

		}
		#endregion

		#region Callback Methods

		#endregion

		#region Static Methods

		#endregion

		#region Public Methods
		public void SetInfiniteMode(bool value) {
			infiniteMode = value;
		}

		public void StartWaves() {
			currentWaveIdx = 0;
			ClearWaveActors();
			if (infiniteMode) {
				StartNextInfiniteWave();
			} else {
				StartNextFiniteWave();
			}
		}

		public void AddAdditionalWaveActor(Vector3 originPoint, Actor actor) {
			Actor _actor = Instantiate(actor, originPoint, Quaternion.Euler(0, 0, Random.Range(0, 360)), GameStateManager.Instance.RuntimeFolder);
			_actor.SetIsAdditionalWaveActor();
			additionalWaveActors.Add(_actor);
		}

		public void DestroyWaveActor(Actor actor) {
			if (waveActors.Contains(actor)) {
				waveActors.Remove(actor);
			}
		}

		public void DestroyAdditionalWaveActor(Actor actor) {
			if (additionalWaveActors.Contains(actor)) {
				additionalWaveActors.Remove(actor);
			}
		}

		public void EndGame() {
			if (waveTimerProcess != null) {
				StopCoroutine(waveTimerProcess);
				waveTimerProcess = null;
			}
			if (spawnWaveProcess != null) {
				StopCoroutine(spawnWaveProcess);
				spawnWaveProcess = null;
			}
		}

		public void ClearWaveActors() {
			for (int i = 0; i < waveActors.Count; i++) {
				Destroy(waveActors[i].gameObject);
			}
			waveActors.Clear();
			for (int i = 0; i < additionalWaveActors.Count; i++) {
				Destroy(additionalWaveActors[i].gameObject);
			}
			additionalWaveActors.Clear();
		}
		#endregion

		#region Private Methods
		private void StartNextFiniteWave() {
			if (finiteWaves.Length >= currentWaveIdx + 1) {
				IngameGameplayMenu.Instance.StartNextWave();
				spawnedWaveActors = 0;
				if (waveTimerProcess != null) {
					StopCoroutine(waveTimerProcess);
					waveTimerProcess = null;
				}
				if (spawnWaveProcess != null) {
					StopCoroutine(spawnWaveProcess);
					spawnWaveProcess = null;
				}
				waveTimerProcess = StartCoroutine(FiniteWaveTimerProcess());
				spawnWaveProcess = StartCoroutine(FiniteSpawnWaveProcess());
			} else {
				GameStateManager.Instance.EndGame();
				IngameGameplayMenu.Instance.GameComplete();
				Debug.LogWarning($"There are not enough defined waves to support the requested level ({currentWaveIdx})");
			}
		}

		private IEnumerator FiniteWaveTimerProcess() {
			bool _waveCleared = false;
			while (!_waveCleared) {
				yield return new WaitForEndOfFrame();
				if (spawnedWaveActors == finiteWaves[currentWaveIdx].SpawnAmount) {
					if (waveActors.Count == 0 && additionalWaveActors.Count == 0) {
						_waveCleared = true;
						AudioManager.Instance.PlayOneShot(waveClearedClip);
						IngameGameplayMenu.Instance.WaveCleared();
					}
				}
			}
			yield return new WaitForSeconds(nextWaveDelay);
			currentWaveIdx++;
			StartNextFiniteWave();
		}

		private IEnumerator FiniteSpawnWaveProcess() {
			while (spawnedWaveActors < finiteWaves[currentWaveIdx].SpawnAmount) {
				if (spawnedWaveActors > startSpawnRateThreshold) {
					yield return new WaitForSeconds(finiteWaves[currentWaveIdx].GetSpawnRate());
				} else {
					yield return new WaitForSeconds(Random.Range(startSpawnRateOverrideMin, startSpawnRateOverrideMax));
				}
				Actor _actor = Instantiate(finiteWaves[currentWaveIdx].Actors[Random.Range(0, finiteWaves[currentWaveIdx].Actors.Count)], GetWaveActorSpawnPoint(), Quaternion.identity, GameStateManager.Instance.RuntimeFolder);
				SetWaveActorRotation(_actor, GetWaveActorLookPoint());
				waveActors.Add(_actor);
				spawnedWaveActors++;
			}
		}

		private void StartNextInfiniteWave() {
			IngameGameplayMenu.Instance.StartNextWave();
			spawnedWaveActors = 0;
			if (waveTimerProcess != null) {
				StopCoroutine(waveTimerProcess);
				waveTimerProcess = null;
			}
			if (spawnWaveProcess != null) {
				StopCoroutine(spawnWaveProcess);
				spawnWaveProcess = null;
			}
			float _d = Mathf.Floor(currentWaveIdx / infiniteWaveDivisor);
			currentInfiniteWaveMultiplier = _d == 0 ? 1 : (int)Mathf.Floor(_d);
			waveTimerProcess = StartCoroutine(InfiniteWaveTimerProcess());
			spawnWaveProcess = StartCoroutine(InfiniteSpawnWaveProcess());
		}

		private IEnumerator InfiniteWaveTimerProcess() {
			bool _waveCleared = false;
			while (!_waveCleared) {
				yield return new WaitForEndOfFrame();
				if (spawnedWaveActors == (infiniteWaveBase.SpawnAmount * currentInfiniteWaveMultiplier)) {
					if (waveActors.Count == 0 && additionalWaveActors.Count == 0) {
						_waveCleared = true;
						AudioManager.Instance.PlayOneShot(waveClearedClip);
						IngameGameplayMenu.Instance.WaveCleared();
					}
				}
			}
			yield return new WaitForSeconds(nextWaveDelay);
			currentWaveIdx++;
			StartNextInfiniteWave();
		}

		private IEnumerator InfiniteSpawnWaveProcess() {
			while (spawnedWaveActors < (infiniteWaveBase.SpawnAmount * currentInfiniteWaveMultiplier)) {
				if (spawnedWaveActors > startSpawnRateThreshold) {
					yield return new WaitForSeconds(infiniteWaveBase.GetSpawnRate());
				} else {
					yield return new WaitForSeconds(Random.Range(startSpawnRateOverrideMin, startSpawnRateOverrideMax));
				}
				Actor _actor = Instantiate(infiniteWaveBase.Actors[Random.Range(0, infiniteWaveBase.Actors.Count)], GetWaveActorSpawnPoint(), Quaternion.identity, GameStateManager.Instance.RuntimeFolder);
				SetWaveActorRotation(_actor, GetWaveActorLookPoint());
				waveActors.Add(_actor);
				spawnedWaveActors++;
			}
		}

		private Vector3 GetWaveActorSpawnPoint() {
			Vector3 _screenPoint = new Vector3(0, Random.Range(0, Screen.height), 0);
			return Camera.main.ScreenToWorldPoint(_screenPoint) + (Vector3.forward * 5);
		}

		private void SetWaveActorRotation(Actor actor, Vector3 lookPoint) {
			Vector3 _relative = actor.transform.InverseTransformPoint(lookPoint);
			float _angle = Mathf.Atan2(_relative.x, _relative.y) * Mathf.Rad2Deg;
			actor.transform.Rotate(0, 0, -_angle);
		}

		private Vector3 GetWaveActorLookPoint() {
			float _pixelSpacing = Screen.height / 100;
			int _pixelMultiplier = Random.Range(1, 101);
			Vector3 _screenPoint = new Vector3(0, _pixelSpacing * _pixelMultiplier, 0);
			Vector3 _viewPoint = Camera.main.ScreenToViewportPoint(_screenPoint);
			Vector3 _worldPoint = Camera.main.ViewportToWorldPoint(_viewPoint);
			_worldPoint.x = 0;
			_worldPoint.z = 5;
			return _worldPoint;
		}
		#endregion
	}
}